﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;
using YiSha.Business.OrganizationManage;
using YiSha.Business.SystemManage;
using YiSha.Cache;
using YiSha.Entity;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.Util.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace YiSha.Web.Controllers
{
    public class HomeController : BaseController
    {
        private MenuBLL menuBLL;
        private UserBLL userBLL;
        private LogLoginBLL logLoginBLL;
        private MenuAuthorizeBLL menuAuthorizeBLL;
        private readonly OperatorCache _operator;

        public HomeController(MenuBLL _menuBLL, UserBLL _userBLL, LogLoginBLL _logLoginBLL,
            MenuAuthorizeBLL _menuAuthorizeBLL, OperatorCache operatorCache)
        {
            menuBLL = _menuBLL;
            userBLL = _userBLL;
            logLoginBLL = _logLoginBLL;
            menuAuthorizeBLL = _menuAuthorizeBLL;
            _operator = operatorCache;
        }

        #region 视图功能
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            OperatorInfo operatorInfo = await _operator.Current();
            if (operatorInfo == null)
            {
                LogHelper.Debug("User Not Login. Redirect to Login page.");
                return RedirectToAction("Login");
            }
            TData<List<MenuEntity>> objMenu = await menuBLL.GetList(null);
            List<MenuEntity> menuList = objMenu.Data;
            menuList = menuList.Where(p => p.MenuStatus == StatusEnum.Yes.ParseToInt()).ToList();

            if (operatorInfo.IsSystem != 1)
            {
                TData<List<MenuAuthorizeInfo>> objMenuAuthorize = await menuAuthorizeBLL.GetAuthorizeList(operatorInfo);
                List<long?> authorizeMenuIdList = objMenuAuthorize.Data.Select(p => p.MenuId).ToList();
                menuList = menuList.Where(p => authorizeMenuIdList.Contains(p.Id)).ToList();
            }

            ViewBag.MenuList = menuList;
            ViewBag.OperatorInfo = operatorInfo;
            return View();
        }

        [HttpGet]
        public IActionResult Welcome()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.UserName = "admin";
            ViewBag.Password = "123456";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoginOff()
        {
            #region 退出系统
            OperatorInfo user = await _operator.Current();
            if (user != null)
            {
                // 如果不允许同一个用户多次登录，当用户登出的时候，就不在线了
                // if (!GlobalContext.SystemConfig.LoginMultiple)
                // {
                //     await userBLL.UpdateUser(new UserEntity { Id = user.UserId, IsOnline = 0 });
                // }

                // 登出日志
                await logLoginBLL.SaveForm(new LogLoginEntity
                {
                    LogStatus = OperateStatusEnum.Success.ParseToInt(),
                    Remark = "退出系统",
                    IpAddress = NetHelper.Ip,
                    IpLocation = string.Empty,
                    Browser = NetHelper.Browser,
                    OS = NetHelper.GetOSVersion(),
                    ExtraRemark = NetHelper.UserAgent,
                    // BaseCreatorId = user.UserId
                });

                // Operator.Instance.RemoveCurrent();
                new CookieHelper().RemoveCookie("RememberMe");
            }
            #endregion
            return View(nameof(Login));
        }

        [HttpGet]
        public IActionResult NoPermission()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpGet]
        public IActionResult Skin()
        {
            return View();
        }
        #endregion

        #region 获取数据
        public IActionResult GetCaptchaImage()
        {
            string sessionId = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext.Session.Id;
            Tuple<string, int> captchaCode = CaptchaHelper.GetCaptchaCode();
            byte[] bytes = CaptchaHelper.CreateCaptchaImage(captchaCode.Item1);
            new SessionHelper().WriteSession("CaptchaCode", captchaCode.Item2);
            return File(bytes, @"image/jpeg");
        }
        #endregion

        #region 提交数据
        [HttpPost]
        public async Task<IActionResult> LoginJson(string userName, string password, string captchaCode)
        {
            TData obj = new TData();
            if (string.IsNullOrEmpty(captchaCode))
            {
                obj.Message = "验证码不能为空";
                return Json(obj);
            }
            if (captchaCode != new SessionHelper().GetSession("CaptchaCode").ParseToString())
            {
                obj.Message = "验证码错误，请重新输入";
                return Json(obj);
            }
            // TData<UserEntity> userObj = await userBLL.CheckLogin(userName, password, (int)PlatformEnum.Web);
            TData<UserEntity> userObj = await userBLL.CheckLogin(userName, password);
            if (userObj.Tag == 1)
            {
                // await new UserBLL().UpdateUser(userObj.Data);
                // await Operator.Instance.AddCurrent(userObj.Data.WebToken);
            }

            await _operator.AddCurrent(userObj.Data.ApiToken);
            string ip = NetHelper.Ip;
            string browser = NetHelper.Browser;
            string os = NetHelper.GetOSVersion();
            string userAgent = NetHelper.UserAgent;

            Action taskAction = async () =>
            {
                LogLoginEntity logLoginEntity = new LogLoginEntity
                {
                    LogStatus = userObj.Tag == 1 ? OperateStatusEnum.Success.ParseToInt() : OperateStatusEnum.Fail.ParseToInt(),
                    Remark = userObj.Message,
                    IpAddress = ip,
                    IpLocation = IpLocationHelper.GetIpLocation(ip),
                    Browser = browser,
                    OS = os,
                    ExtraRemark = userAgent,
                    // BaseCreatorId = userObj.Data?.Id
                };

                // 让底层不用获取HttpContext
                // logLoginEntity.BaseCreatorId = logLoginEntity.BaseCreatorId ?? 0;

                await logLoginBLL.SaveForm(logLoginEntity);
            };
            // AsyncTaskHelper.StartTask(taskAction);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userObj.Data.UserName),
                new Claim("ApiToken", userObj.Data.ApiToken),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();
            await NetHelper.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            obj.Tag = userObj.Tag;
            obj.Message = userObj.Message;
            return Json(obj);
        }
        #endregion
    }
}

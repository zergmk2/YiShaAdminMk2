using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DynamicApiController;
using YiSha.Cache;
using YiSha.Enum;
using YiSha.Enum.SystemManage;
using YiSha.IBusiness.SystemManage;
using YiSha.Model.Result.SystemManage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.WebApi.Areas.HomeManage
{
    /// <summary>
    ///     通用控制器
    /// </summary>
    [ApiDescriptionSettings(SplitCamelCase = false)]
    [Route("HomeManage/[controller]")]
    public class HomeController : IDynamicApiController
    {
        private readonly ImageCache _imageCache;
        private readonly IMenuAuthorizeBLL _menuAuthorizeBLL;
        private readonly IMenuBLL _menuBLL;
        private readonly OperatorCache _operatorCache;

        public HomeController(ImageCache imageCache, OperatorCache operatorCache, IMenuBLL menuBLL,
            IMenuAuthorizeBLL menuAuthorizeBLL)
        {
            _imageCache = imageCache;
            _operatorCache = operatorCache;
            _menuBLL = menuBLL;
            _menuAuthorizeBLL = menuAuthorizeBLL;
        }

        /// <summary>
        ///     获取背景图片
        /// </summary>
        /// <param name="isPC">pc或app</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public TData<string> GetBackgroundImage([FromQuery] bool isPC = true)
        {
            var obj = new TData<string>();

            if (isPC)
                obj.Data = _imageCache.GetPcBackgroundImg();
            else
                obj.Data = _imageCache.GetPhoneBackgroundImg();

            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        ///     图形验证码
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public TData<object> GetCaptchaImage()
        {
            var captchaCode = CaptchaHelper.GetCaptchaCode();
            var bytes = CaptchaHelper.CreateCaptchaImage(captchaCode.Item1);
            var pic = Convert.ToBase64String(bytes);
            var result = captchaCode.Item2;

            var obj = new TData<object>();
            obj.Data = new {pic, result};
            obj.Tag = 1;

            return obj;
        }


        /// <summary>
        ///     获取导航菜单信息和用户信息
        /// </summary>
        [HttpGet]
        public async Task<TData<object>> GetPageListAndUserInfo()
        {
            var operatorInfo = await _operatorCache.Current();

            var objMenu = await _menuBLL.GetList(null);
            var menuList = objMenu.Data;
            menuList = menuList.Where(p => p.MenuStatus == StatusEnum.Yes.ParseToInt()).ToList();

            if (operatorInfo.IsSystem != 1)
            {
                var objMenuAuthorize = await _menuAuthorizeBLL.GetAuthorizeList(operatorInfo);
                var authorizeMenuIdList = objMenuAuthorize.Data.Select(p => p.MenuId).ToList();
                menuList = menuList.Where(p => authorizeMenuIdList.Contains(p.Id)).ToList();
            }

            #region 导航栏数据处理

            var menuResult = new List<MenuResult>();
            foreach (var menu in menuList.Where(p => p.ParentId == 0).OrderBy(p => p.MenuSort))
            {
                var menu_a = new MenuResult();
                menu_a.url = HttpHelper.IsUrl(menu.MenuUrl) ? menu.MenuUrl : "javascript:;";
                menu_a.icon = menu.MenuIcon;
                menu_a.name = menu.MenuName;
                menu_a.subMenus = new List<MenuResult>();

                foreach (var secondMenu in menuList.Where(p => p.ParentId == menu.Id).OrderBy(p => p.MenuSort))
                {
                    var menu_b = new MenuResult();
                    menu_b.url = HttpHelper.IsUrl(secondMenu.MenuUrl) ? secondMenu.MenuUrl : "javascript:;";
                    menu_b.name = secondMenu.MenuName;
                    menu_b.url = secondMenu.MenuUrl;

                    if (menuList.Where(p => p.ParentId == secondMenu.Id && p.MenuType != (int) MenuTypeEnum.Button)
                        .Count() != 0)
                    {
                        menu_b.subMenus = new List<MenuResult>();
                        foreach (var thirdMenu in menuList.Where(p => p.ParentId == secondMenu.Id)
                            .OrderBy(p => p.MenuSort))
                        {
                            var menu_c = new MenuResult();
                            menu_c.url = HttpHelper.IsUrl(thirdMenu.MenuUrl) ? thirdMenu.MenuUrl : "javascript:;";
                            menu_c.name = thirdMenu.MenuName;
                            menu_c.url = thirdMenu.MenuUrl;

                            menu_b.subMenus.Add(menu_c);
                        }
                    }

                    menu_a.subMenus.Add(menu_b);
                }

                menuResult.Add(menu_a);
            }

            #endregion

            var data = new TData<object>();
            data.Tag = 1;
            data.Data = new {operatorInfo, menuResult};
            return data;
        }

        /// <summary>
        ///     获取用户权限信息
        /// </summary>
        [HttpGet]
        public async Task<TData<UserAuthorizeInfo>> GetUserAuthorizeJson()
        {
            var obj = new TData<UserAuthorizeInfo>();
            var operatorInfo = await _operatorCache.Current();

            var objMenuAuthorizeInfo = await _menuAuthorizeBLL.GetAuthorizeList(operatorInfo);
            obj.Data = new UserAuthorizeInfo();
            obj.Data.IsSystem = operatorInfo.IsSystem;
            if (objMenuAuthorizeInfo.Tag == 1) obj.Data.MenuAuthorize = objMenuAuthorizeInfo.Data;
            obj.Tag = 1;
            return obj;
        }
    }
}

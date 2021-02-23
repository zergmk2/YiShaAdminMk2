using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.WebApi.Areas.ToolManage
{
    /// <summary>
    ///     服务器信息
    /// </summary>
    [Route("ToolManage/[controller]")]
    public class ServerController : BaseController
    {
        #region 获取数据（新）
        /// <summary>
        ///     获取服务器状态
        /// </summary>
        [HttpGet]
        public TData<ComputerInfo> GetServerJson()
        {
            var obj = new TData<ComputerInfo>();
            var computerInfo = ComputerHelper.GetComputerInfo();

            obj.Data = computerInfo;
            obj.Tag = 1;
            return obj;
        }
        
        /// <summary>
        ///     获取服务器状态
        /// </summary>
        [HttpGet]
        public TData<ComputerInfo> GetServerStatus()
        {
            var obj = new TData<ComputerInfo>();
            var computerInfo = ComputerHelper.GetComputerInfo();

            obj.Data = computerInfo;
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        ///     获取服务器基本参数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public TData<object> GetServerInfo()
        {
            var ip = NetHelper.GetWanIp(); // 服务器外网IP
            var ipLocation = IpLocationHelper.GetIpLocation(ip); // IP位置
            var serviceName = Environment.MachineName; // 服务器名称
            var systemOs = RuntimeInformation.OSDescription; // 服务器系统
            var lanIp = NetHelper.GetLanIp(); // 局域网IP
            var osArchitecture = RuntimeInformation.OSArchitecture.ToString(); // 系统架构
            var processorCount = Environment.ProcessorCount.ToString(); // CPU核心数
            var frameworkDescription = RuntimeInformation.FrameworkDescription; // .net core版本
            var ramUse = ((double) Process.GetCurrentProcess().WorkingSet64 / 1048576).ToString("N2") + " MB";
            var startTime = Process.GetCurrentProcess().StartTime.ToString("yyyy-MM-dd HH:mm");

            var obj = new TData<object>();
            obj.Data = new
            {
                ip, ipLocation, serviceName, systemOs, lanIp, osArchitecture, processorCount, frameworkDescription,
                ramUse, startTime
            };
            obj.Tag = 1;
            return obj;
        }

        public TData<string> GetServerIpJson()
        {
            TData<string> obj = new TData<string>();
            string ip = NetHelper.GetWanIp();
            string ipLocation = IpLocationHelper.GetIpLocation(ip);
            obj.Data = string.Format("{0} ({1})", ip, ipLocation);
            obj.Tag = 1;
            return obj;
        }
        #endregion
    }
}

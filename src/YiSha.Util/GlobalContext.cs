using System;
using System.Reflection;
using System.Text;
using Furion;
using YiSha.Util.Helper;
using YiSha.Util.Model;
using YiSha.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;

namespace YiSha.Util
{
    public class GlobalContext
    {
        #region 公开属性、方法

        public static IServiceCollection Services { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }

        public static IConfiguration Configuration { get; set; }

        public static IWebHostEnvironment HostingEnvironment { get; set; }

        public static string GetVersion()
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;
            return version.Major + "." + version.Minor;
        }

        /// <summary>
        ///     程序启动时，记录目录
        /// </summary>
        /// <param name="env"></param>
        public static void LogWhenStart(IWebHostEnvironment env)
        {
            var sb = new StringBuilder();
            sb.Append("程序启动");
            sb.Append(" |ContentRootPath:" + env.ContentRootPath);
            sb.Append(" |WebRootPath:" + env.WebRootPath);
            sb.Append(" |IsDevelopment:" + env.IsDevelopment());
            LogHelper.Debug(sb.ToString());
        }

        /// <summary>
        ///     获取系统配置
        /// </summary>
        public static SystemConfig SystemConfig => App.GetOptions<SystemConfig>();
        #endregion
    }
}

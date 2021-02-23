using System.Collections.Generic;
using YiSha.Util;

namespace YiSha.Model.Result.SystemManage
{
    public class MenuResult
    {
        private static readonly string PageFolder = GlobalContext.SystemConfig.PageFolder;

        /// <summary>
        ///     导航栏名称
        /// </summary>
        public string name { set; get; }

        /// <summary>
        ///     路径
        /// </summary>
        public string url { set; get; }

        /// <summary>
        ///     iframe
        /// </summary>
        public string iframe
        {
            get
            {
                if (!string.IsNullOrEmpty(url))
                {
                    // 如果是相关站点，直接用url
                    if (url.StartsWith("http"))
                    {
                        return url;
                    }

                    if (url == "#/api")
                        return "/api";

                    return "/" + PageFolder + url.Replace("#", "") + ".html";
                }

                return string.Empty;
            }
        }

        /// <summary>
        ///     图标
        /// </summary>
        public string icon { set; get; }

        /// <summary>
        ///     子菜单
        /// </summary>
        public List<MenuResult> subMenus { set; get; }
    }
}

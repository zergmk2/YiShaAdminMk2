using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Furion.DependencyInjection;
using YiSha.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Cache
{
    /// <summary>
    ///     图片缓存
    /// </summary>
    public class ImageCache : ITransient
    {
        private readonly ICache _cache;
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly SystemConfig _systemConfig;
        private readonly string _imgPcCacheKey = CacheKeys.ImagePcCache.ParseToString();
        private readonly string _imgPhoneCacheKey = CacheKeys.ImagePhoneCache.ParseToString();

        public ImageCache(Func<string, ISingleton, object> resolveNamed, IOptions<SystemConfig> option,
            IWebHostEnvironment hostingEnvironment)
        {
            _cache = resolveNamed(GlobalContext.SystemConfig.CacheService, default) as ICache;
            _systemConfig = option.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        #region 私有方法

        /// <summary>
        ///     获取图片URL
        /// </summary>
        private string GetPhotoUrl(string imagePath)
        {
            // 随机取配置文件的数据
            var appSettings = _systemConfig.BackgroundGetUrl;
            var configKeys = appSettings.OrderBy(u => Guid.NewGuid()).FirstOrDefault() ?? "";

            return configKeys + imagePath.Replace(Path.DirectorySeparatorChar, '/');
        }

        #endregion

        #region 获取数据

        /// <summary>
        ///     获取PC图片
        /// </summary>
        public string GetPcBackgroundImg()
        {
            var imgs = _cache.Get<List<string>>(_imgPcCacheKey);
            if (imgs == null || imgs.Count() == 0)
            {
                imgs = new List<string>();
                var dir = new DirectoryInfo(_hostingEnvironment.WebRootPath + Path.DirectorySeparatorChar + "Images" +
                                            Path.DirectorySeparatorChar + "Pc");

                if (!dir.Exists)
                    return string.Empty;

                var fil = dir.GetFiles();

                foreach (var f in fil)
                    imgs.Add("/Images/Pc/" + f.Name);
            }

            var img = imgs.OrderBy(u => Guid.NewGuid()).FirstOrDefault() ?? "";
            return GetPhotoUrl(img);
        }

        /// <summary>
        ///     获取移动端图片
        /// </summary>
        public string GetPhoneBackgroundImg()
        {
            var imgs = _cache.Get<List<string>>(_imgPhoneCacheKey);
            if (imgs == null || imgs.Count() == 0)
            {
                imgs = new List<string>();
                var dir = new DirectoryInfo(_hostingEnvironment.WebRootPath + Path.DirectorySeparatorChar + "Images" +
                                            Path.DirectorySeparatorChar + "Phone");
                var fil = dir.GetFiles();

                foreach (var f in fil)
                    imgs.Add("/Images/Phone/" + f.Name);
            }

            var img = imgs.OrderBy(u => Guid.NewGuid()).FirstOrDefault() ?? "";
            return GetPhotoUrl(img);
        }

        #endregion
    }
}

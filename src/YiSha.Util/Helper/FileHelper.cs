using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Util.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YiSha.Enum;

namespace YiSha.Util.Helper
{
    public class FileHelper
    {
        #region 创建文本文件

        /// <summary>
        ///     创建文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void CreateFile(string path, string content)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path))) Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.Write(content);
            }
        }

        #endregion

        #region 删除单个文件

        /// <summary>
        ///     删除单个文件
        /// </summary>
        /// <param name="fileModule"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static TData<string> DeleteFile(int fileModule, string filePath)
        {
            var obj = new TData<string>();
            var dirModule = fileModule.GetDescriptionByEnum<UploadFileType>();

            if (string.IsNullOrEmpty(filePath))
            {
                obj.Message = "请先选择文件！";
                return obj;
            }

            filePath = "Resource" + Path.DirectorySeparatorChar + dirModule + Path.DirectorySeparatorChar + filePath;
            var absoluteDir = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, filePath);
            try
            {
                if (File.Exists(absoluteDir))
                    File.Delete(absoluteDir);
                else
                    obj.Message = "文件不存在";
                obj.Tag = 1;
            }
            catch (Exception ex)
            {
                obj.Message = ex.Message;
            }

            return obj;
        }

        #endregion

        #region 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="delete"></param>
        /// <returns></returns>
        public static TData<FileContentResult> DownloadFile(string filePath, int delete)
        {
            TData<FileContentResult> obj = new TData<FileContentResult>();
            string absoluteFilePath = GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + filePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            byte[] fileBytes = File.ReadAllBytes(absoluteFilePath);
            if (delete == 1)
            {
                File.Delete(absoluteFilePath);
            }
            string fileNamePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string title = string.Empty;
            if (fileNameWithoutExtension.Contains("_"))
            {
                title = fileNameWithoutExtension.Split('_')[1].Trim();
            }
            string fileExtensionName = Path.GetExtension(filePath);
            obj.Data = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = string.Format("{0}_{1}{2}", fileNamePrefix, title, fileExtensionName)
            };
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        ///     下载文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="delete"></param>
        /// <returns></returns>
        public static TData<FileContentResult> DownloadFile(string filePath, string fileName)
        {
            var obj = new TData<FileContentResult>();
            var absoluteFilePath = filePath;
            var fileBytes = File.ReadAllBytes(absoluteFilePath);
            var fileNamePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
            obj.Data = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = string.Format("{0}_{1}", fileNamePrefix, fileName),
            };
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region GetContentType

        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            var contentType = types[ext];
            if (string.IsNullOrEmpty(contentType)) contentType = "application/octet-stream";
            return contentType;
        }

        #endregion

        #region GetMimeTypes

        public static Dictionary<string, string> GetMimeTypes()
        {
            return new()
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        #endregion

        public static void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }

        public static void DeleteDirectory(string filePath)
        {
            try
            {
                if (Directory.Exists(filePath)) //如果存在这个文件夹删除之
                {
                    foreach (var d in Directory.GetFileSystemEntries(filePath))
                        if (File.Exists(d))
                            File.Delete(d); //直接删除其中的文件
                        else
                            DeleteDirectory(d); //递归删除子文件夹
                    Directory.Delete(filePath, true); //删除已空文件夹
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        public static string ConvertDirectoryToHttp(string directory)
        {
            directory = directory.ParseToString();
            directory = directory.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return directory;
        }

        public static string ConvertHttpToDirectory(string http)
        {
            http = http.ParseToString();
            http = http.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            return http;
        }

        public static TData CheckFileExtension(string fileExtension, string allowExtension)
        {
            var obj = new TData();
            var allowArr = TextHelper.SplitToArray<string>(allowExtension.ToLower(), '|');
            if (allowArr.Where(p => p.Trim() == fileExtension.ParseToString().ToLower()).Any())
                obj.Tag = 1;
            else
                obj.Message = "只有文件扩展名是 " + allowExtension + " 的文件才能上传";
            return obj;
        }

        public static async Task<string> GetMD5CheckSum(string fileName)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            await using var stream = File.OpenRead(fileName);
            return BitConverter.ToString(await md5.ComputeHashAsync(stream)).Replace("-", string.Empty);
        }

        #region 上传单个文件

        /// <summary>
        ///     上传单个文件
        /// </summary>
        public static TData<string> UploadFile(UploadImages uploadImages, UploadFileType uploadFileType)
        {
            var returnDta = new TData<string>();

            var year = DateTime.Now.Year.ToString("0000");
            var month = DateTime.Now.Month.ToString("00");
            var day = DateTime.Now.Day.ToString("00");

            var temp = uploadFileType.ParseToString();
            var path = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath,
                "Resource" + Path.DirectorySeparatorChar + temp + Path.DirectorySeparatorChar + year +
                Path.DirectorySeparatorChar + month + Path.DirectorySeparatorChar + day);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path); //获取当前项目所在目录

            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var datetime = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            var suffix =
                "." + (string.IsNullOrEmpty(uploadImages.ImgType) ? "jpg" : uploadImages.ImgType); //文件的后缀名根据实际情况
            var strPath = path + Path.DirectorySeparatorChar + datetime + suffix;
            var bt = Convert.FromBase64String(uploadImages.ImgPath.Split(',')[1]);

            //写入文件
            File.WriteAllBytes(strPath, bt);
            returnDta.Message = "保存成功。";
            returnDta.Tag = 1;

            //返回路径
            returnDta.Data = @"/Resource/" + temp + @"/" + year + @"/" + month + @"/" + day + @"/" + datetime + suffix;
            return returnDta;
        }

        /// <summary>
        ///     上传单个文件
        /// </summary>
        public static async Task<TData<string>> UploadFile(int fileModule, IFormFileCollection files)
        {
            var dirModule = string.Empty;
            var obj = new TData<string>();
            if (files == null || files.Count == 0)
            {
                obj.Message = "请先选择文件！";
                return obj;
            }

            if (files.Count > 1)
            {
                obj.Message = "一次只能上传一个文件！";
                return obj;
            }

            TData objCheck = null;
            var file = files[0];
            switch (fileModule)
            {
                case (int) UploadFileType.Portrait:
                    objCheck = CheckFileExtension(Path.GetExtension(file.FileName), ".jpg|.jpeg|.gif|.png");
                    if (objCheck.Tag != 1)
                    {
                        obj.Message = objCheck.Message;
                        return obj;
                    }

                    dirModule = UploadFileType.Portrait.ToString();
                    break;

                case (int) UploadFileType.TaskFile:
                    objCheck = CheckFileExtension(Path.GetExtension(file.FileName), ".pdf|.jpg|.jpeg|.gif|.png");
                    if (objCheck.Tag != 1)
                    {
                        obj.Message = objCheck.Message;
                        return obj;
                    }

                    dirModule = UploadFileType.TaskFile.ToString();
                    break;

                default:
                    obj.Message = "请指定上传到的模块";
                    return obj;
            }

            var fileExtension = TextHelper.GetCustomValue(Path.GetExtension(file.FileName), ".png");

            var newFileName = SecurityHelper.GetGuid() + fileExtension;
            var dir = "Resource" + Path.DirectorySeparatorChar + dirModule + Path.DirectorySeparatorChar +
                      DateTime.Now.ToString("yyyy-MM-dd").Replace('-', Path.DirectorySeparatorChar) +
                      Path.DirectorySeparatorChar;

            var absoluteDir = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, dir);
            var absoluteFileName = string.Empty;
            if (!Directory.Exists(absoluteDir)) Directory.CreateDirectory(absoluteDir);
            absoluteFileName = absoluteDir + newFileName;
            try
            {
                using (var fs = File.Create(absoluteFileName))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }

                obj.Data = Path.AltDirectorySeparatorChar + ConvertDirectoryToHttp(dir) + newFileName;
                obj.Message = Path.GetFileNameWithoutExtension(TextHelper.GetCustomValue(file.FileName, newFileName));
                obj.Description = (file.Length / 1024).ToString(); // KB
                obj.Tag = 1;
            }
            catch (Exception ex)
            {
                obj.Message = ex.Message;
            }

            return obj;
        }

                /// <summary>
        ///     上传单个文件
        /// </summary>
        public static async Task<TData<string>> UploadFile(int fileModule, IFormFile formFile)
        {
            var dirModule = string.Empty;
            var obj = new TData<string>();
            if (formFile == null || formFile.Length == 0)
            {
                obj.Message = "请先选择文件！";
                return obj;
            }

            TData objCheck = null;
            var file = formFile;
            switch (fileModule)
            {
                case (int) UploadFileType.Portrait:
                    objCheck = CheckFileExtension(Path.GetExtension(file.FileName), ".jpg|.jpeg|.gif|.png");
                    if (objCheck.Tag != 1)
                    {
                        obj.Message = objCheck.Message;
                        return obj;
                    }

                    dirModule = UploadFileType.Portrait.ToString();
                    break;

                case (int) UploadFileType.TaskFile:
                    objCheck = CheckFileExtension(Path.GetExtension(file.FileName), ".pdf|.jpg|.jpeg|.gif|.png");
                    if (objCheck.Tag != 1)
                    {
                        obj.Message = objCheck.Message;
                        return obj;
                    }

                    dirModule = UploadFileType.TaskFile.ToString();
                    break;

                default:
                    obj.Message = "请指定上传到的模块";
                    return obj;
            }

            var fileExtension = TextHelper.GetCustomValue(Path.GetExtension(file.FileName), ".png");

            var newFileName = SecurityHelper.GetGuid() + fileExtension;
            var dir = "Resource" + Path.DirectorySeparatorChar + dirModule + Path.DirectorySeparatorChar +
                      DateTime.Now.ToString("yyyy-MM-dd").Replace('-', Path.DirectorySeparatorChar) +
                      Path.DirectorySeparatorChar;
            var absoluteDir = string.Empty;
            if (!string.IsNullOrEmpty(GlobalContext.SystemConfig.DefaultFileStorage))
            {
                absoluteDir = Path.Combine(GlobalContext.SystemConfig.DefaultFileStorage, dir);
            }
            else
            {
                absoluteDir = Path.Combine(GlobalContext.HostingEnvironment.ContentRootPath, dir);
            }

            var absoluteFileName = string.Empty;
            if (!Directory.Exists(absoluteDir)) Directory.CreateDirectory(absoluteDir);
            absoluteFileName = absoluteDir + newFileName;
            try
            {
                using (var fs = File.Create(absoluteFileName))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }

                obj.Data = ConvertDirectoryToHttp(dir) + newFileName;
                obj.Message = Path.GetFileNameWithoutExtension(TextHelper.GetCustomValue(file.FileName, newFileName));
                obj.Description = (file.Length / 1024).ToString(); // KB
                obj.Tag = 1;
            }
            catch (Exception ex)
            {
                obj.Message = ex.Message;
            }

            return obj;
        }

        #endregion
    }
}

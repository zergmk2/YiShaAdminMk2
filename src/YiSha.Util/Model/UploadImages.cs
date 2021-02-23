namespace YiSha.Util.Model
{
    /// <summary>
    ///     图片上传实体类
    /// </summary>
    public class UploadImages
    {
        /// <summary>
        ///     图片路径
        /// </summary>
        public string ImgPath { get; set; }

        /// <summary>
        ///     图片类型
        /// </summary>
        public string ImgType { get; set; }

        /// <summary>
        ///     保存文件
        /// </summary>
        public string FolderName { get; set; }


        public int fileModule { set; get; }
    }
}

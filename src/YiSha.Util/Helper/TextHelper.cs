using System;
using System.Linq;

namespace YiSha.Util.Helper
{
    public class TextHelper
    {
        /// <summary>
        ///     获取默认值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetCustomValue(string value, string defaultValue)
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            return value;
        }

        /// <summary>
        ///     截取指定长度的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetSubString(string value, int length, bool ellipsis = false)
        {
            if (string.IsNullOrEmpty(value)) return value;
            if (value.Length > length)
            {
                value = value.Substring(0, length);
                if (ellipsis) value += "...";
            }

            return value;
        }

        /// <summary>
        ///     字符串转指定类型数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static T[] SplitToArray<T>(string value, char split)
        {
            var arr = value.Split(new[] {split.ToString()}, StringSplitOptions.RemoveEmptyEntries).CastSuper<T>()
                .ToArray();
            return arr;
        }

        /// <summary>
        ///     字符串首字母小写
        /// </summary>
        /// <param name="str">字符串|如:TheName</param>
        /// <returns>输出:theName</returns>
        public static string StrFirstCharToLower(string str)
        {
            if (str == null)
                return "";
            var iLen = str.Length;
            if (iLen == 0)
                return "";
            if (iLen == 1)
                return str.ToLower();
            return str[0].ToString().ToLower() + str.Substring(1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace YiSha.Util
{
    public static partial class Extensions
    {
        #region 获取枚举的描述

        /// <summary>
        ///     获取枚举值对应的描述
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum enumType)
        {
            var EnumInfo = enumType.GetType().GetField(enumType.ToString());
            if (EnumInfo != null)
            {
                var EnumAttributes =
                    (DescriptionAttribute[]) EnumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (EnumAttributes.Length > 0) return EnumAttributes[0].Description;
            }

            return enumType.ToString();
        }

        #endregion

        #region 根据值获取枚举的描述

        public static string GetDescriptionByEnum<T>(this object obj)
        {
            var tEnum = System.Enum.Parse(typeof(T), obj.ParseToString()) as System.Enum;
            var description = tEnum.GetDescription();
            return description;
        }

        #endregion

        #region 枚举成员转成dictionary类型

        /// <summary>
        ///     转成dictionary类型
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<int, string> EnumToDictionary(this Type enumType)
        {
            var dictionary = new Dictionary<int, string>();
            var typeDescription = typeof(DescriptionAttribute);
            var fields = enumType.GetFields();
            var sValue = 0;
            var sText = string.Empty;
            foreach (var field in fields)
                if (field.FieldType.IsEnum)
                {
                    sValue = (int) enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);
                    var arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        var da = (DescriptionAttribute) arr[0];
                        sText = da.Description;
                    }
                    else
                    {
                        sText = field.Name;
                    }

                    dictionary.Add(sValue, sText);
                }

            return dictionary;
        }

        /// <summary>
        ///     枚举成员转成键值对Json字符串
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string EnumToDictionaryString(this Type enumType)
        {
            var dictionaryList = EnumToDictionary(enumType).ToList();
            var sJson = JsonConvert.SerializeObject(dictionaryList);
            return sJson;
        }

        #endregion
    }
}

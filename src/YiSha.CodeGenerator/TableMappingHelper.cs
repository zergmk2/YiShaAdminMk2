﻿using System.Text;
using YiSha.Util;

namespace YiSha.CodeGenerator
{
    public class TableMappingHelper
    {
        /// <summary>
        ///     UserService转成userService
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstLetterLowercase(string instanceName)
        {
            instanceName = instanceName.ParseToString();
            if (!instanceName.IsEmpty())
            {
                var sb = new StringBuilder();
                sb.Append(instanceName[0].ToString().ToLower() + instanceName.Substring(1));
                return sb.ToString();
            }

            return instanceName;
        }

        /// <summary>
        ///     sys_menu_authorize变成MenuAuthorize
        /// </summary>
        public static string GetClassNamePrefix(string tableName)
        {
            var arr = tableName.Split('_');
            var sb = new StringBuilder();
            for (var i = 1; i < arr.Length; i++) sb.Append(arr[i][0].ToString().ToUpper() + arr[i].Substring(1));
            return sb.ToString();
        }

        public static string GetPropertyDatatype(string sDatatype)
        {
            var sTempDatatype = string.Empty;
            sDatatype = sDatatype.ToLower();
            switch (sDatatype)
            {
                case "int":
                case "number":
                case "integer":
                case "smallint":
                    sTempDatatype = "int?";
                    break;

                case "bigint":
                    sTempDatatype = "long?";
                    break;

                case "tinyint":
                    sTempDatatype = "byte?";
                    break;

                case "numeric":
                case "real":
                    sTempDatatype = "Single?";
                    break;

                case "float":
                    sTempDatatype = "float?";
                    break;

                case "decimal":
                case "numer(8,2)":
                    sTempDatatype = "decimal?";
                    break;

                case "bit":
                    sTempDatatype = "bool?";
                    break;

                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    sTempDatatype = "DateTime?";
                    break;

                case "money":
                case "smallmoney":
                    sTempDatatype = "double?";
                    break;

                case "char":
                case "varchar":
                case "nvarchar2":
                case "text":
                case "nchar":
                case "nvarchar":
                case "ntext":
                default:
                    sTempDatatype = "string";
                    break;
            }

            return sTempDatatype;
        }
    }
}

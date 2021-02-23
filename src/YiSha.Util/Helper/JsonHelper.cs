﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YiSha.Util.Helper
{
    #region JsonHelper

    public static class JsonHelper
    {
        public static T ToObject<T>(this string Json)
        {
            Json = Json.Replace("&nbsp;", "");
            return Json == null ? default : JsonConvert.DeserializeObject<T>(Json);
        }

        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }
    }

    #endregion

    #region JsonConverter

    /// <summary>
    ///     Json数据返回到前端js的时候，把数值很大的long类型转成字符串
    /// </summary>
    public class StringJsonConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return reader.Value.ParseToLong();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var sValue = value.ToString();
            writer.WriteValue(sValue);
        }
    }

    /// <summary>
    ///     DateTime类型序列化的时候，转成指定的格式
    /// </summary>
    public class DateTimeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return reader.Value.ParseToString().ParseToDateTime();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var dt = value as DateTime?;
            if (dt == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(dt.Value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }

    #endregion
}

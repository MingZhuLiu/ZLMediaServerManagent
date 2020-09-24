using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaServerManagent.Commons
{
    /// <summary>
    /// Long类型Json序列化重写
    /// 在js中传输会导致精度丢失，故而在序列化时转换成字符类型
    /// </summary>
    public class JsonConverterLong : JsonConverter
    {
        /// <summary>
        /// 是否可以转换
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <summary>
        /// 读json
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if ((reader.ValueType == null || reader.ValueType == typeof(long?)) && reader.Value == null)
            {
                return null;
            }
            else
            {
                long.TryParse(reader.Value != null ? reader.Value.ToString() : "", out long value);
                return value;
            }
        }

        /// <summary>
        /// 写json
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteValue(value);
            else
                writer.WriteValue(value + "");
        }
    }
}
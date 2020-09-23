using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Models.Dto
{
    public class ConfigDto
    {
        public string ConfigKey { get; set; }

        public string ConfigValue { get; set; }

        public T GetValue<T>()
        {
            T result = default(T);
            if (ConfigValue == null)
                return result;

            if (typeof(T) == typeof(string))
            {
                result = (T)(ConfigValue as Object);
            }
            else if (typeof(T) == typeof(int))
            {
                result = (T)(int.Parse(ConfigKey) as Object);
            }
            else if (typeof(T) == typeof(float))
            {
                result = (T)(float.Parse(ConfigKey) as Object);
            }
            else if (typeof(T) == typeof(double))
            {
                result = (T)(double.Parse(ConfigKey) as Object);
            }
            else if (typeof(T) == typeof(decimal))
            {
                result = (T)(decimal.Parse(ConfigKey) as Object);
            }

            else if (typeof(T) == typeof(DateTime))
            {
                result = (T)(DateTime.Parse(ConfigKey) as Object);
            }
            return result;
        }

        public void SetValue(Object data)
        {
            if (data == null)
            {
                ConfigValue = null;
            }
            else if (data.GetType() == typeof(string))
            {
                ConfigValue = data as String;
            }
            else if (data.GetType() == typeof(DateTime))
            {
                ConfigValue = ((DateTime)data).ToString("yyyy-MM-dd HH:mm:ss:ffff");
            }
            else
            {
                ConfigValue = data.ToString();
            }
        }
    }

    /// <summary>
    /// 配置表字段名称
    /// </summary>
    public static class ConfigKeys
    {
        /// <summary>
        /// ZL媒体服务器IP地址
        /// </summary>
        public static string ZLMediaServerIp = "ZLMediaServerIp";

        /// <summary>
        ///  ZL媒体服务器端口地址
        /// </summary>
        public static string ZLMediaServerPort = "ZLMediaServerPort";

        /// <summary>
        /// ZL媒体服务器密钥
        /// </summary>
        public static string ZLMediaServerSecret = "ZLMediaServerSec";

    }
}

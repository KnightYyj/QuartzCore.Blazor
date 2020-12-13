using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuartzCore.Common.Helper
{
    public class JsonHelper
    {
        /// <summary>
        /// 处理Json的时间格式为正常格式
        /// </summary>
        public static string JsonDateTimeFormat(string json)
        {
            json = Regex.Replace(json,
                @"\\/Date\((\d+)\)\\/",
                match =>
                {
                    DateTime dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                });
            return json;
        }

        /// <summary>
        /// 把对象序列化成Json字符串格式并格式化时间对象
        /// </summary>
        /// <param name="object">Json 对象</param>
        /// <returns>Json 字符串</returns>
        public static string ToJsonFormatDateTime(object @object)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string json = JsonConvert.SerializeObject(@object, Newtonsoft.Json.Formatting.Indented, timeFormat);
            return json;
        }

        /// <summary>
        /// 把对象序列化成Json字符串格式
        /// </summary>
        /// <param name="object">Json 对象</param>
        /// <returns>Json 字符串</returns>
        public static string ToJson(object @object)
        {
            string json = JsonConvert.SerializeObject(@object);
            return JsonDateTimeFormat(json);
        }

        /// <summary>
        /// 把Json字符串转换为强类型对象
        /// </summary>
        public static T FromJson<T>(string json)
        {
            json = JsonDateTimeFormat(json);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 根据制定的key查询json的值的集合
        /// </summary>
        /// <param name="jsonString">json字符串</param>
        /// <param name="key">json键</param>
        /// <returns>集合</returns>
        public static List<string> GetJsonValueList(string jsonString, string key)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(jsonString))
            {
                try
                {
                    using (JsonReader reader = new JsonTextReader(new StringReader(jsonString)))
                    {
                        while (reader.Read())
                        {
                            //result += " 【path:" + reader.Path + " Token:" + reader.TokenType + " Type: " + reader.ValueType + " Value: " + reader.Value + " 】";
                            string path = reader.Path;
                            JsonToken jt = reader.TokenType;
                            if (!string.IsNullOrEmpty(path))
                            {
                                var pathList = path.Split('.').ToList();
                                if (pathList.Contains(key) && jt == JsonToken.String)
                                {
                                    string val = reader.Value.ToString().Trim();
                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        if (!list.Contains(val))
                                        {
                                            list.Add(val);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("Json序列化异常：", ex);
                }
            }
            return list;
        }
    }
}

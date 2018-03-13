using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxes.Common
{
    class DataConvertHelper
    {
        #region Json处理
        /// <summary>
        ///  序化object为Json字符
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JsonSerialize(object obj)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            IsoDateTimeConverter idtc = new IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(idtc);
            JsonWriter jw = new JsonTextWriter(sw);
            jw.Formatting = Formatting.Indented;
            serializer.Serialize(jw, obj);
            return sb.ToString();
        }
        /// <summary>
        ///  解析Json字符为object对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T JsonDeserializeObject<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);

        }
        #endregion
    }
}

using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// AlibabaAliqinFcVoiceNumDoublecallResponse.
    /// </summary>
    public class AlibabaAliqinFcVoiceNumDoublecallResponse : TopResponse
    {
        /// <summary>
        /// 接口返回参数
        /// </summary>
        [XmlElement("result")]
        public Top.Api.Domain.BizResult Result { get; set; }

    }
}

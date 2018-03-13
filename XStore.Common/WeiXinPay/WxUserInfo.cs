using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStore.Common.WeiXinPay
{

    //用户信息类
    public class WxUserInfo
    {
        public int subscribe { get; set; } = 0;

        public string openid { get; set; } = "";
        public int sex { get; set; } = 0;
        public string language { get; set; } = "";
        public string city { get; set; } = "";
        public string province { get; set; } = "";
        public string country { get; set; } = "";
        public string headimgurl { get; set; } = "";

        public string subscribe_time { get; set; } = "";
        /// <summary>
        /// 消息接收方微信号，一般为公众平台账号微信号
        /// </summary>
        public string toUserName { get; set; } = "";
        public string nickname { get; set; } = "";

    }

}

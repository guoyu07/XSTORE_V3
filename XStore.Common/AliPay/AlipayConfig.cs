

namespace XStore.Common.AiLiPay
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.3
    /// 日期：2012-07-05
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// 
    /// 如何获取安全校验码和合作身份者ID
    /// 1.用您的签约支付宝账号登录支付宝网站(www.alipay.com)
    /// 2.点击“商家服务”(https://b.alipay.com/order/myOrder.htm)
    /// 3.点击“查询合作者身份(PID)”、“查询安全校验码(Key)”
    /// </summary>
    public class Config
    {
        #region 字段
        private static string partner = "";
        private static string key = "";
        private static string email = "";
        private static string type = "1";
        private static string return_url = "";
        private static string notify_url = "";
        private static string input_charset = "";
        private static string sign_type = "";
        private static string private_key = "";
        private static string public_key = "";
        private static string gatewayUrl = "";
        private static string app_id = "";
        #endregion

        static Config()
        {
            //Log.WriteLog("zfb config", "", "");

            //合作身份者ID，以2088开头由16位纯数字组成的字符串
            partner = "2088721227281596";

            app_id = "2017061207475665";
            //交易安全检验码，由数字和字母组成的32位字符串
            key = "yzqd7wudjubg6o1b1s4fhaoula19vr2m";
            //签约支付宝账号或卖家支付宝帐户
            email = "499456529@qq.com ";
            //接口类型1即时到帐2担保交易
            type = "1";
            //商户的私钥
            private_key = "MIICXgIBAAKBgQDodWFjuFNVtk/8A7ZHrthI2dSbViu+BnwkjmTstPa9iyEPZ/3UotaPq+rG4sNo4aHlvG+eRV1wuEZdKmYUPhVqFTmQozIca8R7KzvW2ByZKWBCol9aElzGc5Ff49epTpIC2Au+VSbPs+V6kFNB3tCoKeoGie5vGxizXGZv38bouwIDAQABAoGBAMYFWBUurC7Tw4cXUmv2EeDdTzOUUGbr90zc0DSkY5xLrLoHCD/fB5AUD0elXHk33EZsI1lcFaE0GRy8RYDw8iNwPkwSwocpZzBYi9COpmJpI29WgE677rkZ3eXVLZS4agw74CeHdX+JpqGWjCM1oiKB9pewEh8PuI59ZanDRg+BAkEA/laYqH48Jz3y6nxZcmlc2WpkEg3RT5E3ZUzlfn1jCFGOLXwSjUXLYVH6KweuRLQoHi9UaxoJjuokgb9Y/FYScQJBAOn6MDFeTTOREiZQ/gTNzvDd5Oa2D7PI4Eo7dIfbUgp2XCu5YjG56o5OaRiLTqC1U1PTq4qw4/PaVuuU6pHbK+sCQQCCrugdm085MqGAToh/OxgUNpBYnnTwF0OJb2t0BOU/vvf48wltQXFw/fg25+lpL9B1Qgh0R5qlrjU33aPRdEBhAkEAxdcWHvhlAQBev2VmlLtNix+lKGuzdUqaVEpXq3SIt24DW7liTTeuHGwys10/u+X2sn/doeUWqp/pNUPy4CfZxwJAF1aK/7FTS48CLt0ep3tv84CEEnj2bt47P/+Y9OV0d1n/+dctoLPsKGr7kSK7w7fEFZE9WeBuk2+hxbEBGcV/5A==";
          
            //支付宝的公钥，无需修改该值
            public_key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDDI6d306Q8fIfCOaTXyiUeJHkrIvYISRcc73s3vF1ZT7XN8RNPwJxo8pWaJMmvyTn9N4HQ632qJBVHf8sxHi/fEsraprwCtzvzQETrNRwVxLO5jVmRGi60j8Ue1efIlzPXV9je9mkjzOmdssymZkh2QhUrCmZYI/FCEa3/cNMW0QIDAQAB";

            //页面跳转同步返回页面文件路径 要用 http://格式的完整路径，不允许加?id=123这类自定义参数
           // return_url = "http://" + HttpContext.Current.Request.Url.Authority.ToLower() + "/NotifyPay/alipaypc/return_url.aspx";
            return_url = "http://wx2.x-store.com.cn/WebSite/Order/AliReturn.aspx";
                 //服务器通知的页面文件路径 要用 http://格式的完整路径，不允许加?id=123这类自定义参数
                 // notify_url = "http://" + HttpContext.Current.Request.Url.Authority.ToLower() + "/ NotifyPay / alipaypc / notify_url.aspx";
            notify_url = "http://wx2.x-store.com.cn/WebSite/Order/AliNotify.aspx";

            //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

            //字符编码格式 目前支持 gbk 或 utf-8
            input_charset = "utf-8";

            //签名方式，选择项：RSA、DSA、MD5
            sign_type = "RSA";
            //支付宝网关地址
             gatewayUrl = "https://openapi.alipay.com/gateway.do";
        /*
         * NSString *partner = @"2088421326493185";	//商户号
NSString *seller  = @"xjx1919@goujiuwang.com"; 	//支付账户
NSString *privateKey = @"MIICeAIBADANBgkqhkiG9w0BAQEFAASCAmIwggJeAgEAAoGBAL3IHkIRogedUjuiSHKmnqMAviRrIKPTuXmZPwUNit1b/1uCcWaR3EeovXsABjN291oeTRXDlL3NwljxbmiIawVhZpCyCBrMLyp6JW+N57//Oc6yE7GLztBFSmYC62M9I+2QZBzXd1hXL7ZVp81u7dSheyGT/G1wJwkVHAASHuwfAgMBAAECgYEAjDhd0ucAVqvwZEtFSCC/uSQFWRcl6KW4tpV5sJwO6/rbM5uved9vaCrOxSCBdGkD3TviLKBzN8HdRKYts2KH3wvWBgtsxlvjoD04GGqtS+JHNaeoxAkrdq6YRGZx1Pe28FpQts2+3vPw00TBG6RByoHLJlACyd4ZsbLOkO5Zc6ECQQD4HGtIfgRYZCU/uhpiz+Mqq7B+X5cyda6WJNJHUZNpLSmgkgiWILm0DldnItokxSTUnlO6MA8ABS+03pnDQsQpAkEAw9DoncpS+QLhcTtJpkSQtmHw/qnhODkwbyZSkul3d0Z67eaxIoc+lNX8BwmrXuWfze6pm8nM1ViXG512kMj3BwJBAMhm9ATGvK3Eng2ePUfI0J0btmAsx8xbH5xou2wdqOqQLwpiSgsw/a5A0zob/YUrs7cE923w+Xyzs7ftANelqxkCQQCTGUylvGe4SEGzHialkLxlRg4UQOm/oraPyOofEuN75qZSMQrANXj9tgIuB0WFdFooCTHApkgJvRNG23NGMLHhAkBKPAXIrVOzrbcPkki6LRlKoZ7FcpQxekWgX5mDdH4BvcIn7I96WHb83bDG+gcmC7EyQT4TLNwu07Di3svUYkq8"; 	//私钥

         * **/
    }

        #region 属性
        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public static string Partner
        {
            get { return partner; }
            set { partner = value; }
        }
        /// <summary>
        /// 应用ID
        /// </summary>
        public static string AppId
        {
            get { return app_id; }
            set { app_id = value; }
        }

        /// <summary>
        /// 获取或设置交易安全检验码
        /// </summary>
        public static string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// 接口类型1即时到帐2担保交易
        /// </summary>
        public static string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// 获取或设置签约支付宝账号或卖家支付宝帐户
        /// </summary>
        public static string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// 获取或设置商户的私钥
        /// </summary>
        public static string Private_key
        {
            get { return private_key; }
            set { private_key = value; }
        }
      
        /// <summary>
        /// 获取或设置支付宝的公钥
        /// </summary>
        public static string Public_key
        {
            get { return public_key; }
            set { public_key = value; }
        }
     
        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public static string Input_charset
        {
            get { return input_charset; }
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public static string Sign_type
        {
            get { return sign_type; }
        }

        /// <summary>
        /// 获取页面跳转同步通知页面路径
        /// </summary>
        public static string Return_url
        {
            get { return return_url; }
        }

        /// <summary>
        /// 获取服务器异步通知页面路径
        /// </summary>
        public static string Notify_url
        {
            get { return notify_url; }
        }
        /// <summary>
        /// 支付宝网管地址
        /// </summary>
        public static string GateWayUrl
        {
            get { return gatewayUrl; }
        }
        #endregion



    }
}

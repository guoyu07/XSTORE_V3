using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace XStore.Common.WeiXinPay
{
    public class WxPayConfig
    {

        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        */
        //public static string APPID = ConfigurationManager.AppSettings["APPID"].ToString();
        //public static string APPSECRET = ConfigurationManager.AppSettings["APPSecret"].ToString();
        //public static string MCHID = ConfigurationManager.AppSettings["MCHID"].ToString();
        //public static string KEY = ConfigurationManager.AppSettings["KEY"].ToString();

        //public static string APPID = "wx0320e1e7c034a43b";
        //public static string APPSECRET = "953f10b10959fc35ad69c61a17552d1e";//"436f9e04b0fbd2e11f4a5d70b6d1d990";
        //public static string MCHID = "1486235672";//"1296165301";
        //public static string KEY = "gP1eBSfnrhzpn3P3gfeAucyy4jjpE0gz";

        public static string APPID = "wx4b52212c5d5983ad";
        public static string APPSECRET = "58954dc71e9ac0d51e142ecacb44b0ba";//"436f9e04b0fbd2e11f4a5d70b6d1d990";
        public static string MCHID = "1433628402";//"1296165301";
        public static string KEY = "gP1eBSfnrhzpn3P3gfeAucyy4jjpE0gz";


        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SSLCERT_PATH = "cert/apiclient_cert.p12";
        //证书密码
        public const string SSLCERT_PASSWORD = "1433628402";

        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public const string NOTIFY_URL = "http://wx2.x-store.com.cn/WebSite/Order/WeiXinNotify.aspx";

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public const string IP = "119.29.94.189";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string PROXY_URL = "";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 1;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LOG_LEVENL = 3;
    }
}

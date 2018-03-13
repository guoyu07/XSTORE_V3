using System;
using System.Web.Security;
using System.Xml;
using Newtonsoft.Json;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace XStore.Common.WeiXinPay
{
    public class WeiXinOath
    {
        private string Token = "xstore888";
        public string devlopID = ConfigurationManager.AppSettings["APPID"].ToString();  //"wx4b52212c5d5983ad";// 
        public string devlogPsw = ConfigurationManager.AppSettings["APPSecret"].ToString();  //"58954dc71e9ac0d51e142ecacb44b0ba";// 
        public string AccessToken = "";

        #region"验证及初始化"
        public WeiXinOath() { }
        public WeiXinOath(string _devlopID, string _devlogPsw)
        {
            this.devlopID = _devlopID;
            this.devlogPsw = _devlogPsw;
        }
        public void Auth()
        {
            string echoStr = System.Web.HttpContext.Current.Request.QueryString["echoStr"];
            if (CheckSignature())
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    System.Web.HttpContext.Current.Response.Write(echoStr);
                    System.Web.HttpContext.Current.Response.End();
                }
            }
        }

   
        public string GetAccessToken()
        {
            string url_token = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + devlopID + "&secret=" + devlogPsw;
            string result = Utils.HttpGet(url_token);
            //return result;
            accessToken deserializedProduct = (accessToken)JsonConvert.DeserializeObject(result, typeof(accessToken));
            //return deserializedProduct.expires_in.ToString();
            this.AccessToken = deserializedProduct.access_token;
            return this.AccessToken;
        }
        public string GetAccessTokenByAppId(string _devlopID, string _devlogPsw)
        {
            string url_token = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + _devlopID + "&secret=" + _devlogPsw;
            string result = Utils.HttpGet(url_token);
            //return result;
            accessToken deserializedProduct = (accessToken)JsonConvert.DeserializeObject(result, typeof(accessToken));
            //return deserializedProduct.expires_in.ToString();
            this.AccessToken = deserializedProduct.access_token;
            return this.AccessToken;
        }
        public string GetOpenID(string _code)
        {
            string result = "";
            string _url2 = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + this.devlopID + "&secret=" + this.devlogPsw + "&code=" + _code + "&grant_type=authorization_code";
            result = Utils.HttpGet(_url2);
            access_tokenUser deserializedProduct = (access_tokenUser)JsonConvert.DeserializeObject(result, typeof(access_tokenUser));
            return  deserializedProduct.openID; 
        } 

        public string GetMenu()
        {
            string url_Menu_Get = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + this.AccessToken;
            string output = Utils.HttpGet(url_Menu_Get);
            //wxErr deserializedProduct = (wxErr)JsonConvert.DeserializeObject(output, typeof(wxErr));
            //return deserializedProduct.errmsg;
            return output;
        }

        #region 获取网页授权Token
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code">用户同意授权，获取code</param>
        /// <returns></returns>
        public OauthToken GetOauthToken(string Code)
        {
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + devlopID + "&secret=" + devlogPsw + "&code=" + Code + "&grant_type=authorization_code";
            return JsonConvert.DeserializeObject<OauthToken>(Utils.HttpGet(url));
        }
        #endregion

        #region 获取网页授权用户基本信息
        /// <summary>
        /// 获取网页授权用户基本信息
        /// </summary>
        /// <param name="OAuthToken">网页授权口令</param>
        /// <param name="_openid">网页用户OpenId</param>
        /// <returns></returns>
        public WxUserInfo GetWebUserInfo(string OAuthToken, string _openid)
        {
            string _result = string.Empty;
            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + OAuthToken + "&openid=" + _openid + "&lang=zh_CN";
            LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "用户信息请求地址：" + url);
            var result = Utils.HttpGet(url);
            LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "用户信息返回结果：" + result);
            return JsonConvert.DeserializeObject<WxUserInfo>(result);
        }
        #endregion
        
       
        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        private bool CheckSignature()
        {
            string signature = System.Web.HttpContext.Current.Request.QueryString["signature"];
            string timestamp = System.Web.HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = System.Web.HttpContext.Current.Request.QueryString["nonce"];
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region "在后台回复微信消息"
        /// <summary>
        /// 返回回复信息的URl
        /// </summary>
        /// <param name="accToke"></param>
        /// <returns></returns>
        private string ReMessage(string accToke)
        {
            return "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + accToke;
        }
        #endregion
        #region 获取GetCode
        /// <summary>
        ///  获取GetCode
        /// </summary>
        public void GetCode(string redirect_uri)
        {
            string _url = "";

            _url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + devlopID + "&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_base&state=1#wechat_redirect";

            HttpContext.Current.Response.Redirect(_url);
        }
        #endregion


    }
  

    //微信请求类
    public class RequestXML
    {
        private string msgId = "";
        public string msgID
        {
            get { return msgId; }
            set { msgId = value; }
        }
        private string toUserName = "";
        /// <summary>
        /// 消息接收方微信号，一般为公众平台账号微信号
        /// </summary>
        public string ToUserName
        {
            get { return toUserName; }
            set { toUserName = value; }
        }

        private string fromUserName = "";
        /// <summary>
        /// 消息发送方微信号
        /// </summary>
        public string FromUserName
        {
            get { return fromUserName; }
            set { fromUserName = value; }
        }

        private string createTime = "";
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        private string msgType = "";
        /// <summary>
        /// 信息类型 地理位置:location,文本消息:text,消息类型:image
        /// </summary>
        public string MsgType
        {
            get { return msgType; }
            set { msgType = value; }
        }

        private string content = "";
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string location_X = "";
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public string Location_X
        {
            get { return location_X; }
            set { location_X = value; }
        }

        private string location_Y = "";
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y
        {
            get { return location_Y; }
            set { location_Y = value; }
        }

        private string scale = "";
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        private string label = "";
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        private string picUrl = "";
        /// <summary>
        /// 图片链接，开发者可以用HTTP GET获取
        /// </summary>
        public string PicUrl
        {
            get { return picUrl; }
            set { picUrl = value; }
        }

        private string MediaId = "";
        public string mediaId
        {
            set { MediaId = value; }
            get { return MediaId; }
        }

        private string Format = "";
        public string format
        {
            set { Format = value; }
            get { return Format; }
        }

        private string ThumbMediaId = "";
        public string thumbMediaId
        {
            set { ThumbMediaId = value; }
            get { return ThumbMediaId; }
        }

        private string title = "";
        public string Title
        {
            set { title = value; }
            get { return title; }
        }
        private string description = "";
        public string Description
        {
            set { description = value; }
            get { return description; }
        }

        private string url = "";
        public string Url
        {
            set { url = value; }
            get { return url; }
        }

        private string Event = "";
        public string evEnt
        {
            get { return Event; }
            set { Event = value; }
        }

        private string EventKey = "";
        public string eventKey
        {
            get { return EventKey; }
            set { EventKey = value; }

        }

        private string Ticket = "";
        public string ticket
        {
            get { return Ticket; }
            set { Ticket = value; }

        }

        private string Latitude = "";
        public string latitude
        {
            get { return Latitude; }
            set { Latitude = value; }
        }

        private string Longitude = "";
        public string longitude
        {
            get { return Longitude; }
            set { Longitude = value; }
        }

        private string Precision = "";
        public string precision
        {
            get { return Precision; }
            set { Precision = value; }
        }
    }

    public class accessToken
    {
        private string _access_token;
        public string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }
        private int _expires_in;
        public int expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }
    }
    public class access_tokenUser
    {
        private string _access_token;
        public string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }
        private int _expires_in;
        public int expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }
        private string _refresh_token;
        public string refresh_token
        {
            get { return _refresh_token; }
            set { _refresh_token = value; }
        }
        private string _openID;
        public string openID
        {
            get { return _openID; }
            set { _openID = value; }
        }
        private string _scope;
        public string scope
        {
            get { return _scope; }
            set { _scope = value; }
        }
    }
}

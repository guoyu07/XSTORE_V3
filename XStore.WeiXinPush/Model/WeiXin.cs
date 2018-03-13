using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;

namespace WeiXinPush.Model
{ 
    class WeiXin
    {
        string Token = "Se1f_99";

        public static string appid = ConfigurationManager.AppSettings["APPID"];

        public static string secret = ConfigurationManager.AppSettings["APPSecret"];

        public void RequestPlate(string postData)
        {

            var access_token = WeiXin.GetAccess_token().access_token;

            var url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + access_token;

            System.Net.HttpWebRequest request = default(System.Net.HttpWebRequest);

            System.IO.Stream requestStream = default(System.IO.Stream);

            byte[] postBytes = Encoding.UTF8.GetBytes(postData.ToString());
            //byte[] postBytes = null;

            request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);

            request.ContentType = "application/x-www-form-urlencoded;charset=utf8";

            request.ContentLength = postBytes.Length;

            request.Timeout = 10000;

            request.Method = "POST";

            request.AllowAutoRedirect = false;

            requestStream = request.GetRequestStream();

            // postBytes = Encoding.ASCII.GetBytes(postData.ToString());

            //string finalPost = System.Text.Encoding.UTF8.GetString(postBytes);

            requestStream.Write(postBytes, 0, postBytes.Length);

            requestStream.Close();

        }

        /// <summary>
        /// 动态获取access_token值，由于access_token是有时效性的
        /// </summary>
        /// <returns></returns>
        public static Access_token GetAccess_token()
        {

            string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;

            Access_token mode = new Access_token();

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);

            req.Method = "GET";

            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

                string content = reader.ReadToEnd();
                //在这里对Access_token 赋值 
                Access_token token = new Access_token();

                token = JsonHelper.ParseFromJson<Access_token>(content);

                mode.access_token = token.access_token;

                mode.expires_in = token.expires_in;
            }
            return mode;
        }

    }
}

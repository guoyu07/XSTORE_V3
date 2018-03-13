namespace boxes.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web;
 public class HttpHelper
 {
     #region URL请求数据
     /// <summary>
     /// HTTP POST方式请求数据
     /// </summary>
     /// <param name="url">URL.</param>
     /// <param name="param">POST的数据</param>
     /// <returns></returns>
     public static string HttpPost(string url, string param,string contenttype="application/x-www-form-urlencoded")
     {
         HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
         request.Method = "POST";
         request.ContentType = contenttype;
         request.Accept = "*/*";
         request.Timeout = 15000;
         request.AllowAutoRedirect = false;

         StreamWriter requestStream = null;
         WebResponse response = null;
         string responseStr = null;

         try
         {
             requestStream = new StreamWriter(request.GetRequestStream());
             requestStream.Write(param);
             requestStream.Close();

             response = request.GetResponse();
             if (response != null)
             {
                 StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                 responseStr = reader.ReadToEnd();
                 reader.Close();
             }
         }
         catch (Exception)
         {
             throw;
         }
         finally
         {
             request = null;
             requestStream = null;
             response = null;
         }

         return responseStr;
     }

     /// <summary>
     /// HTTP GET方式请求数据.
     /// </summary>
     /// <param name="url">URL.</param>
     /// <returns></returns>
     public static string HttpGet(string url)
     {
         HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
         request.Method = "GET";
         //request.ContentType = "application/x-www-form-urlencoded";
         request.Accept = "*/*";
         request.Timeout = 15000;
         request.AllowAutoRedirect = false;

         WebResponse response = null;
         string responseStr = null;

         try
         {
             response = request.GetResponse();

             if (response != null)
             {
                 StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                 responseStr = reader.ReadToEnd();
                 reader.Close();
             }
         }
         catch (Exception)
         {
             throw;
         }
         finally
         {
             request = null;
             response = null;
         }

         return responseStr;
     }

     /// <summary>
     /// //将键值对转换成&拼接字符串，例如id=1&type=a
     /// </summary>
     /// <param name="_param"></param>
     /// <returns></returns>
     public static string GetUrlParam(Dictionary<object, object> _param)
     {
         StringBuilder sbparam = new StringBuilder();
         if (_param != null && _param.Count > 0)
         {
             foreach (KeyValuePair<object, object> item in _param)
             {
                 sbparam.Append(item.Key + "=" + item.Value + "&");
             }
         }
         return sbparam.ToString().TrimEnd('&');

     }
     #endregion
 }
}

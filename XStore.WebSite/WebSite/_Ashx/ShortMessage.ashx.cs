using Chloe.MySql;
using Common.AliDayu;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XStore.Entity;
using XStore.Entity.Model;
using XStore.WebSite.DBFactory;

namespace XStore.WebSite.WebSite._Ashx
{
    /// <summary>
    /// ShortMessage 的摘要说明
    /// </summary>
    public class ShortMessage : IHttpHandler
    {
        public static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                var mysqlContext = new MySqlContext(new MySqlConnectionFactory(connString));
                string phone = context.Request["phone"].ObjToStr();
                Random ran = new Random();
                string code = ran.Next(1000, 10000).ToString();
                var messageInfo = mysqlContext.Query<MessageLog>().OrderByDesc(o=>o.createTime).FirstOrDefault(o => o.phone.Equals(phone));

                if (messageInfo != null && messageInfo.createTime.AddMinutes(1) > DateTime.Now)
                {
                    var addtime = messageInfo.createTime.AddMinutes(1);
                    var isbig = addtime > DateTime.Now;
                    context.Response.Write(JsonConvert.SerializeObject(new AjaxResponse{ success = false, message = "验证已发送的您的手机，请稍后再试!" }));
                    return;
                }
                else
                {
                    var job = new JObject();
                    job["code"] = code;
                    job["product"] = string.Empty;
                    if (AliSendMessage.Send(phone, JsonConvert.SerializeObject(job)))
                    {
                        mysqlContext.Insert(new MessageLog
                        {
                            code = code,
                            createTime = DateTime.Now,
                            phone = phone
                        });
                        context.Response.Write(JsonConvert.SerializeObject(new AjaxResponse { success = true, message = "验证码已发送到您的手机", code = code }));
                        return;
                    }
                    else
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new AjaxResponse { success = false, message = "验证码发送失败", code = code }));
                        return;
                    };
                }
            }
            catch (Exception e)
            {
                context.Response.Write(JsonConvert.SerializeObject(new AjaxResponse { success = false, message = "数据异常"}));
                return;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
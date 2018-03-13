using Chloe.MySql;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using XStore.Entity;
using XStore.Entity.Model;
using XStore.WebSite.DBFactory;

namespace XStore.WebSite.WebSite._Ashx
{
    /// <summary>
    /// UnBindAccount 的摘要说明
    /// </summary>
    public class UnBindAccount : IHttpHandler, IRequiresSessionState
    {
        public static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                var mysqlContext = new MySqlContext(new MySqlConnectionFactory(connString));
                var userInfo = (User)context.Session[Constant.CurrentUser];
                userInfo.weichat = string.Empty;
                userInfo.phone = string.Empty;
                var returnState = mysqlContext.Update(userInfo);
                if (returnState != 0)
                {
                    context.Session.Clear();
                    context.Response.Write(JsonConvert.SerializeObject(new AjaxResponse { success = true, message = "注销成功" }));
                    return;
                }
                else
                {
                    context.Response.Write(JsonConvert.SerializeObject(new AjaxResponse { success = false, message = "账号已被注销" }));
                    return;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(JsonConvert.SerializeObject(new AjaxResponse { success = false, message = "注销失败" }));
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
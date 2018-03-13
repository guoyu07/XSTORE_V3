using Chloe.MySql;
using Newtonsoft.Json;
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
    /// ChangePassword 的摘要说明
    /// </summary>
    public class ChangePassword : IHttpHandler
    {
        public static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/plain";
                var mysqlContext = new MySqlContext(new MySqlConnectionFactory(connString));
                string phone = context.Request["phone"].ObjToStr();
                string name = context.Request["name"].ObjToStr();
                string account = context.Request["account"].ObjToStr();
                string password = context.Request["password"].ObjToStr();
                string username = context.Request["username"].ObjToStr();
                var userInfo = mysqlContext.Query<User>().FirstOrDefault(o => o.username.Equals(username));
                if (userInfo == null)
                {
                    context.Response.Write(JsonConvert.SerializeObject(new AjaxResponse { success = false, message = "用户不存在" }));
                    return;
                }
                var isExist = mysqlContext.Query<User>().FirstOrDefault(o => o.username.Equals(account) && !o.username.Equals(username));
                if (isExist != null)
                {
                    context.Response.Write(JsonConvert.SerializeObject(new AjaxResponse { success = false, message = "用户名已被使用" }));
                    return;
                }
                userInfo.password = password;
                userInfo.phone = phone;
                userInfo.realname = name;
                userInfo.username = account;
                mysqlContext.Update(userInfo);
                context.Response.Write(JsonConvert.SerializeObject(new AjaxResponse { success = true, message = "密码修改成功" }));
                return;
            }
            catch (Exception)
            {
                context.Response.Write(JsonConvert.SerializeObject(new AjaxResponse { success = false, message = "密码修改失败" }));
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
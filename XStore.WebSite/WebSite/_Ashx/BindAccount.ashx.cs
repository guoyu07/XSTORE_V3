using Chloe.MySql;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using XStore.Common;
using XStore.Entity;
using XStore.Entity.Model;
using XStore.WebSite.DBFactory;
using static XStore.Entity.Enum;

namespace XStore.WebSite.WebSite._Ashx
{
    /// <summary>
    /// BindAccount 的摘要说明
    /// </summary>
    public class BindAccount : IHttpHandler, IRequiresSessionState
    {
        public static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                var mysqlContext = new MySqlContext(new MySqlConnectionFactory(connString));
                string phone = context.Request["phone"].ObjToStr();
                string realname = context.Request["realname"].ObjToStr();
                string username = context.Request["username"].ObjToStr();
                string password = context.Request["password"].ObjToStr();
                string openid = context.Session[Constant.OpenId].ObjToStr();
                var userInfo = mysqlContext.Query<User>().FirstOrDefault(o => o.username.Equals(username));
                if (userInfo == null)
                {
                    context.Response.Write(JsonConvert.SerializeObject(new BindResponse { success = false, message = "用户不存在" }));
                    return;
                }
                else
                {
                    mysqlContext.Session.BeginTransaction();
                    //迁移数据到历史记录表
                    var userHistory = TinyMapper.Map<UserHistory>(userInfo);
                    mysqlContext.Insert(userHistory);
                    //更新用户信息
                    userInfo.password = password;
                    userInfo.phone = phone;
                    userInfo.realname = realname;
                    userInfo.weichat = openid;
                    mysqlContext.Update(userInfo);
                    //添加登录日志
                    mysqlContext.Insert<LoginRecord>(new LoginRecord
                    {
                        date = DateTime.Now,
                        ip = Utils.GetUserIp(),
                        state = 1,
                        username = userInfo.username
                    });
                    mysqlContext.Session.CommitTransaction();

                    var userRole = mysqlContext.Query<UserRole>().FirstOrDefault(o => o.username.Equals(userInfo.username));
                    if (userRole == null)
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new BindResponse { success = false, message = "权限异常" }));
                        return;
                    }
                    
                   
                    switch ((UserRoleEnum)userRole.role_id)
                    {
                        case UserRoleEnum.经理:
                            context.Response.Write(JsonConvert.SerializeObject(new BindResponse { success = true, message = "修改成功", url = Constant.JsCenterDic+ "ManageCenter.aspx" }));
                            return;
                        case UserRoleEnum.财务:
                            context.Response.Write(JsonConvert.SerializeObject(new BindResponse { success = true, message = "修改成功", url = Constant.JsCenterDic + "FinanceCenter.aspx" }));
                            return;
                        case UserRoleEnum.前台:
                            context.Response.Write(JsonConvert.SerializeObject(new BindResponse { success = true, message = "修改成功", url = Constant.JsCenterDic + "EmployeeCenter.aspx" }));
                            return;
                        case UserRoleEnum.测试员:
                            context.Response.Write(JsonConvert.SerializeObject(new BindResponse { success = true, message = "修改成功", url = Constant.JsCenterDic + "TestCenter.aspx" }));
                            return;
                        case UserRoleEnum.配水员:
                            context.Response.Write(JsonConvert.SerializeObject(new BindResponse { success = true, message = "修改成功", url = Constant.JsCenterDic + "PromotionCenter.aspx" }));
                            return;
                        case UserRoleEnum.区域经理:
                            context.Response.Write(JsonConvert.SerializeObject(new BindResponse { success = true, message = "修改成功", url = Constant.JsCenterDic + "ManageCenter.aspx" }));
                            return;
                        default:
                            context.Response.Write(JsonConvert.SerializeObject(new BindResponse { success = true, message = "修改成功", url = Constant.JsCenterDic + "" }));
                            return;
                    }
                }
                
            }
            catch (Exception ex)
            {
                context.Response.Write(JsonConvert.SerializeObject(new BindResponse { success = false, message = "数据异常"}));
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
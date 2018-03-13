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
using static XStore.Entity.Enum;

namespace XStore.WebSite.WebSite._Ashx
{
    /// <summary>
    /// CheckOrderState 的摘要说明
    /// </summary>
    public class CheckOrderState : IHttpHandler, IRequiresSessionState
    {
        public static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                var mysqlContext = new MySqlContext(new MySqlConnectionFactory(connString));
                var orderNo = context.Session[Constant.OrderNo].ObjToStr();
                var orderInfo = mysqlContext.Query<OrderInfo>().FirstOrDefault(o => o.code.Equals(orderNo));
                if (orderInfo == null)
                {
                    context.Response.Write(JsonConvert.SerializeObject(new PayAjaxResponse
                    {
                        success = false,
                        code = "01",
                        message = "订单不存在"
                    }));
                    return;
                }
                else
                {
                    var result = JsonConvert.SerializeObject(new PayAjaxResponse
                    {
                        success = true,
                        code = "00",
                        message = "查询成功",
                        pay = orderInfo.paid,
                        deliver = orderInfo.delivered
                    });
                    context.Response.Write(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(JsonConvert.SerializeObject(new PayAjaxResponse
                {
                    success = true,
                    code = "00",
                    message = ex.Message
                }));
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
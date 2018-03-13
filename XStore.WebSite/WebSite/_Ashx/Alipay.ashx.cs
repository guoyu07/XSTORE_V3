using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using XStore.Common.AiLiPay;
using XStore.Common.WeiXinPay;
using XStore.Entity;

namespace XStore.WebSite.WebSite._Ashx
{
    /// <summary>
    /// Alipay 的摘要说明
    /// </summary>
    public class Alipay : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                Log.WriteLog("Alipay", "进入支付宝", "-----");
                //subject = "商品描述";
                string orderNo = context.Request["orderNo"].ObjToStr();
                Log.WriteLog("Alipay", "orderNo", orderNo);
                decimal amount = context.Request["amount"].ObjToDecimal(0);
                string subject = "幸事多"; //context.Request["subject"].ObjToStr();

                string app_id = Config.AppId;//Config.Partner;//合作伙伴id
                string merchant_private_key = Config.Private_key;
                string alipay_public_key = Config.Public_key;

                string timeout_express = "30m";//订单有效时间（分钟）
                string postUrl = Config.GateWayUrl;
                string sign_type = Config.Sign_type;//加签方式 有两种RSA和RSA2 我这里使用的RSA2（支付宝推荐的）
                string version = "1.0";//固定值 不用改
                string format = "json";//固定值
                string Amount = amount.ObjToStr();//订单金额

                string method = "alipay.trade.wap.pay";//调用接口 固定值 不用改

                IAopClient client = new DefaultAopClient(postUrl, app_id, merchant_private_key, format, version, sign_type, alipay_public_key, Config.Input_charset, false);
                AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();

                string notify_url = Config.Notify_url;
                string return_url = Config.Return_url;
                Log.WriteLog("Alipay", "notify_url", notify_url);
                Log.WriteLog("Alipay", "return_url", return_url);
                request.SetNotifyUrl(notify_url);
                request.SetReturnUrl(return_url);

                var bizcontent = "{" +
                "    \"body\":\"" + subject + "\"," +
                "    \"subject\":\"" + subject + "\"," +
                "    \"out_trade_no\":\"" + orderNo + "\"," +
                "    \"timeout_express\":\"" + timeout_express + "\"," +
                "    \"total_amount\":\"" + Amount + "\"," +
                "    \"product_code\":\"" + method + "\"" +
                "  }";
                Log.WriteLog("Alipay", "bizcontent", bizcontent);
                request.BizContent = bizcontent;
                AlipayTradeWapPayResponse response = client.pageExecute(request);
                string form = response.Body;
                context.Response.Write(form);
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using XStore.Common;
using XStore.Common.Helper;
using XStore.Common.WeiXinPay;
using XStore.Entity;
using XStore.Entity.Model;

namespace XStore.WebSite.WebSite.Order
{
    public class WxPayNotify:Notify
    {
        BasePage basePage = new BasePage();
        public WxPayNotify(Page page) : base(page)
        {

        }
        public override void ProcessNotify()
        {
            try
            {
                WxPayData notifyData = GetNotifyData();
                //检查支付结果中transaction_id是否存在
                if (!notifyData.IsSet("transaction_id"))
                {
                    //若transaction_id不存在，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "支付结果中微信订单号不存在");
                    page.Response.Write(res.ToXml());
                    page.Response.End();
                }
                string transaction_id = notifyData.GetValue("transaction_id").ToString();
                //查询订单，判断订单真实性
                if (!QueryOrder(transaction_id))
                {
                    //若订单查询失败，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "订单查询失败");
                    page.Response.Write(res.ToXml());
                    page.Response.End();
                }
                //查询订单成功
                else
                {
                    var context = basePage.context;
                    string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
                    out_trade_no = out_trade_no.Split('_')[0].ToString();
                    var orderInfo = context.Query<OrderInfo>().FirstOrDefault(o => o.code.Equals(out_trade_no));
                    if (orderInfo == null)
                    {
                        WxPayData res = new WxPayData();
                        res.SetValue("return_code", "FAIL");
                        res.SetValue("return_msg", "订单失效");
                        page.Response.Write(res.ToXml());
                        page.Response.End();
                    }

                    if (orderInfo.price1 == notifyData.GetValue("total_fee").ObjToInt(0))
                    {
                        try
                        {
                            var requestUrl = string.Format("{2}test/pay?orderId={0}&payId={1}", out_trade_no, transaction_id,Constant.YunApi);
                            var response = JsonConvert.DeserializeObject<OrderResponse>(Utils.HttpGet(requestUrl));
                            if (response.operationStatus.Equals("SUCCESS"))
                            {
                                var rbh = new RemoteBoxHelper();
                                //执行开箱成功
                                rbh.OpenRemoteBox(orderInfo.cabinet_mac, out_trade_no, orderInfo.pos.ObjToStr());
                            };
                            WxPayData res = new WxPayData();
                            res.SetValue("return_code", "SUCCESS");
                            res.SetValue("return_msg", "订单成功");
                            page.Response.Write(res.ToXml());
                            page.Response.End();
                        }
                        catch (Exception e)
                        {
                            WxPayData res = new WxPayData();
                            res.SetValue("return_code", "FAIL");
                            res.SetValue("return_msg", "订单异常");
                            page.Response.Write(res.ToXml());
                            page.Response.End();
                        }
                    }
                    else
                    {
                        WxPayData res = new WxPayData();
                        res.SetValue("return_code", "FAIL");
                        res.SetValue("return_msg", "订单金额不对");
                        page.Response.Write(res.ToXml());
                        page.Response.End();

                    }
                }
            }
            catch (Exception ex)
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "回调异常");
                page.Response.Write(res.ToXml());
                page.Response.End();
            }

        }


        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
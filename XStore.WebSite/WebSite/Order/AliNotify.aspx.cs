using Chloe.MySql;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using XStore.Common;
using XStore.Common.AiLiPay;
using XStore.Common.Helper;
using XStore.Common.WeiXinPay;
using XStore.Entity;
using XStore.Entity.Model;
using XStore.WebSite.DBFactory;

namespace XStore.WebSite.WebSite.Order
{
    public partial class AliNotify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Log.WriteLog("notify_url", "支付宝回调", "-----");
                string connString = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
                var  context = new MySqlContext(new MySqlConnectionFactory(connString));
                //Log.WriteLog("not11ify_url", "page_load", "");
                SortedDictionary<string, string> sPara = GetRequestPost();
                bool verifyResult = false;
                //Log.WriteLog("notify_url", "page_load", "");
                if (sPara.Count > 0)//判断是否有带返回参数
                {
                    Log.WriteLog("notify_url", "验证成功", sPara.ObjToStr());
                    Common.AiLiPay.Notify aliNotify = new Common.AiLiPay.Notify();
                    //Log.WriteLog("notify_url", "验证成功", "1");
                    try
                    {
                        Log.WriteLog("notify_url", "验证成功", "1_try");
                        verifyResult = aliNotify.Verify(sPara, DTRequest.GetString("notify_id"), DTRequest.GetString("sign"));
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog("", "", ex.ToString());
                        Response.Write("fail");
                        return;
                        //Log.WriteLog("notify_url", "验证成功", "1_catch");
                    }
                    //Log.WriteLog("notify_url", "验证成功", "2");
                    if (verifyResult)//验证成功
                    {
                        //Log.WriteLog("notify_url", "验证成功", "2_if");
                        //Log.WriteLog("zfb_________________________________________________________________________________", "验证成功", "");
                        string trade_no = DTRequest.GetString("trade_no").Trim();
                        Log.WriteLog("验证成功2_if", "trade_no", trade_no.ObjToStr());//支付宝交易号
                        string out_trade_no = DTRequest.GetString("out_trade_no").Trim();
                        Log.WriteLog("验证成功2_if", "order_no", out_trade_no.ObjToStr());//获取订单号
                        string total_fee = DTRequest.GetString("total_amount");
                       
                        Log.WriteLog("验证成功2_if", "total_fee", total_fee.ObjToStr());//获取总金额
                        string trade_status = DTRequest.GetString("trade_status");           //交易状态

                        var orderInfo = context.Query<OrderInfo>().FirstOrDefault(o => o.code.Equals(out_trade_no) && o.paid == false);
                        //如果改订单已经支付或者不存在，则不继续往下走
                        if (orderInfo == null)
                        {
                            Response.Write("fail");
                            return;
                        }
                     
                        if (Config.Type == "1") //即时到帐接口处理方法
                        {

                            if (trade_status == "TRADE_FINISHED" || trade_status == "TRADE_SUCCESS")
                            {
                                var requestUrl = string.Format("{2}test/pay?orderId={0}&payId={1}&payType={3}", out_trade_no, trade_no, Constant.YunApi,1);
                                Log.WriteLog("支付接口回调", "requestUrl", requestUrl);//获取总金额
                                var response = JsonConvert.DeserializeObject<OrderResponse>(Utils.HttpGet(requestUrl));
                                Log.WriteLog("支付接口回调", "response", JsonConvert.SerializeObject(response));//获取总金额
                                if (response.operationStatus.Equals("SUCCESS"))
                                {
                                    var rbh = new RemoteBoxHelper();
                                    Response.Write("success");
                                    //执行开箱成功
                                    rbh.OpenRemoteBox(orderInfo.cabinet_mac, out_trade_no, orderInfo.pos.ObjToStr());
                                    Log.WriteLog("支付接口回调", "成功", "成功");
                                    return;
                                }
                                else
                                {
                                    Response.Write("fail");
                                    return;
                                }
                                ;
                              
                            }
                            else
                            {
                                Response.Write("fail");
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Response.Write("fail");
                return;
               
            }

        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            coll = Request.Form;

            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            return sArray;
        }
    }
}
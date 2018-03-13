using System;
using System.Web;
using XStore.Common;
using XStore.Common.WeiXinPay;
using XStore.Entity;

namespace XStore.WebSite.WebSite.Order
{
    public partial class PayCenter : OrderPage
    {
        public static string wxJsApiParam { get; set; }



        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-支付中心";
            PageInit();
        }
       
        protected void PageInit()
        {
            try
            {
                if (orderInfo == null)
                {
                    MessageBox.Show(this, "system_alert", "订单信息不存在");
                    return;
                }
                var openId = Session[Constant.OpenId].ObjToStr();
                //绑定订单商品
                BindGoods();
                if (debug)
                {
                    return;
                }
                    //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
 
                JsApiPay jsApiPay = new JsApiPay(this);
                jsApiPay.openid = openId;
                jsApiPay.total_fee = orderInfo.price1;
                //JSAPI支付预处理
                try
                {
                    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(orderInfo.code.ObjToStr());
                    wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数   
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Log.Info(ex.Message + ":" + ex.StackTrace);
                MessageBox.Show(this, "system_alert", "生成预订单失败");
                return;
            }
        }


        protected void BindGoods()
        {
            var productId = orderInfo.product.ObjToInt(0);
            var productList = context.Query<Product>().Where(o => o.id == productId).ToList();
            if (productList.Count==0)
            {
                MessageBox.Show(this, "system_alert", "商品不存在或者已下架");
                return;
            };
            car_rp.DataSource = productList;
            car_rp.DataBind();

        }
    }
}
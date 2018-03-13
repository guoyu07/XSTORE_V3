using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XStore.Entity;

namespace XStore.WebSite
{
    public class OrderPage:BasePage
    {
        #region 订单信息
        private OrderInfo _orderInfo;
        public OrderInfo orderInfo
        {
            get
            {
                if (_orderInfo == null)
                {
                    var orderNo = Session[Constant.OrderNo].ObjToStr();
                    if (string.IsNullOrEmpty(orderNo))
                    {
                        orderNo = Request.QueryString[Constant.OrderNo].ObjToStr();
                    }
                    _orderInfo = context.Query<OrderInfo>().FirstOrDefault(o => o.code.Equals(orderNo));
                }
                return _orderInfo;
            }
        }
        #endregion

        #region 购买的商品
        private Product _productInfo;
        protected Product productInfo
        {
            get
            {
                if (_productInfo == null)
                {
                    var productId = orderInfo.product.ObjToInt(0);
                    _productInfo = context.Query<Product>().FirstOrDefault(o => o.id == productId);
                }
                return _productInfo;
            }
        }

        #endregion

    }
}
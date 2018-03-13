using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Entity;

namespace XStore.WebSite.WebSite.Order
{
    public partial class PayWaiting : OrderPage
    {
      
       
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-支付成功";
            if (orderInfo == null)
            {
                MessageBox.Show(this, "system_alert", "订单不存在");
                return;
            }
            title_img.ImageUrl = "~/Content/Images/yh.PNG";
            joker_img.ImageUrl = "~/Content/Images/joker.jpg";
            tel_img.ImageUrl = "~/Content/Images/tel.png";
        }
    }
}
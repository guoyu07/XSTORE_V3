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
    public partial class PayFail : OrderPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-开箱失败";
            if (orderInfo == null)
            {
                MessageBox.Show(this, "system_alert", "订单不存在");
                return;
            }
            title_img.ImageUrl = "~/Content/Images/fail-emoij.png";
            lc1.ImageUrl = "~/Content/Images/lc1.jpg";
            lc2.ImageUrl = "~/Content/Images/lc2.jpg";
            lc2.ImageUrl = "~/Content/Images/lc3.jpg";
        }

       
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Entity;

namespace XStore.WebSite.WebSite.Order
{
    public partial class PaySuccess : OrderPage
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-开箱成功";
            title_img.ImageUrl = "~/Content/Images/paymentSuccessed.png";
        }
       
    }
}
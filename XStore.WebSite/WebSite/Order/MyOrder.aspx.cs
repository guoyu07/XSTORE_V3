using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Entity;

namespace XStore.WebSite.WebSite.Order
{
    public partial class MyOrder : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-我的订单";
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        public void PageInit() {
            var orderList = context.Query<OrderInfo>().LeftJoin<Product>((a, b) => a.product == b.id).Where((o, p) => o.delivered == false && o.paid == true && o.open_id.Equals(OpenId) && o.date.AddMinutes(30) < DateTime.Now).Select((a, b) => new
            {
                b.id,
                b.code,
                b.price1,
                b.image,
                b.name
            }).ToList();

            myOrderRepeater.DataSource = orderList;
            myOrderRepeater.DataBind();
            if (orderList.Count == 0)
            {
                empty.Visible = false;
                has.Visible = true;
                title.Visible = true;
            }
            else
            {
                empty.Visible = true;
                has.Visible = false;
                title.Visible = false;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Entity;

namespace XStore.WebSite.WebSite.Operation
{
    public partial class BackLog : CenterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-投放记录";
            if (!IsPostBack)
            {
                PageInit();
            }
            
        }
        private void PageInit() {
            var backList = context.Query<BackOrderInfo>().InnerJoin<Product>((a, b) => a.product == b.id).InnerJoin<Cabinet>((a, b, c) => a.cabinet_mac.Equals(c.mac))
                .Where((a, b, c) => a.operator_username.Equals(userInfo.username) && a.date.AddMonths(1) > DateTime.Now && a.product != 0)
                .Select((a, b, c) => new
                {
                    id = b.id,
                    image = b.image,
                    room = c.room,
                    name = b.name,
                    code = b.code,
                    date = a.date,
                    pos = a.pos
                }).OrderByDesc(o => o.date).ToList();
            backLogRepeater.DataSource = backList;
            backLogRepeater.DataBind();
        }
    }
}
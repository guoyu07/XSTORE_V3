using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Entity;

namespace XStore.WebSite.WebSite.Information
{
    public partial class GoodsInfo : CenterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-基础信息(商品)";
            if (!IsPostBack)
            {
                PageInit();
            }
        }

        protected void PageInit() {
            var layout = context.Query<CabinetLayout>().FirstOrDefault(o => o.hotel_id == hotelInfo.id);
            if (layout == null)
            {
                MessageBox.Show(this,"system_alert","酒店未设置商品");
                return;
            }
            else
            {
                var products = layout.products.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(o=>o.ObjToInt(0)).ToList();
                var query= context.Query<Product>()
                    .LeftJoin<OrderInfo>((a, b) => a.id == b.product)
                    .LeftJoin<Cabinet>((a,b,c)=>c.mac==b.cabinet_mac)
                    .Where((a,b,c)=>(c.hotel == hotelInfo.id || c.hotel == null)&& products.Contains(a.id))
                    .Select((a, b,c) => new {
                        id = a.id,
                        image = a.image,
                        price1 = a.price1,
                        code = a.code,
                        name = a.name,
                        salePrice = b.price1
                    }).GroupBy(o => o.id)
                    .Select(o => new {
                        id = o.id,
                        name = o.name,
                        code = o.code,
                        image = o.image,
                        price1 = o.price1,
                        salesAmount = AggregateFunctions.Sum(o.salePrice)

                    });
                var list = query.ToList();
                goods_rp.DataSource = list;
                goods_rp.DataBind();
                goods_count.InnerText = (list.Count).ObjToStr();
            }
            
        }
    }
}
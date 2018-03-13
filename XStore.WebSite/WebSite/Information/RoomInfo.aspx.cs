using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Entity;
using XStore.Entity.Model;

namespace XStore.WebSite.WebSite.Information
{
    public partial class RoomInfo : CenterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-基础信息(房间)";
            if (!IsPostBack)
            {
                SortImgBtn.ImageUrl = "/Content/Images/sort37.png";
                PageInit();
            }
        }
        protected void PageInit(string sort="down")
        {
            var list = context.Query<Cabinet>().Where(o => o.hotel == hotelInfo.id).LeftJoin<OrderInfo>((a, b) => a.mac.Equals(b.cabinet_mac))
                .Select((a, b) => new CabinetQuery()
                {
                    mac = a.mac,
                    room = a.room,
                    last_offline_date = a.last_offline_date,
                    online = a.online,
                    salesAmount = b.price1

                })
                .GroupBy(o => o.mac)
                .Select(o => new {
                    mac = AggregateFunctions.Max(o.mac),
                    room = AggregateFunctions.Max(o.room),
                    offline = AggregateFunctions.Max(o.last_offline_date),
                    online = AggregateFunctions.Max(o.online),
                    salesAmount = AggregateFunctions.Sum(o.salesAmount)
                }).ToList()
                .Select(
                    o => new {
                        mac = o.mac,
                        room = o.room,
                        online = o.online,
                        offline = (o.online.HasValue && o.online.Value) ? "--" : o.offline.HasValue ? (DateTime.Now-o.offline.Value).Hours + " H" : "999 H",
                        salesAmount = o.salesAmount
                    }
                ).ToList();
            room_count.InnerHtml = list.Count(o => o.salesAmount > 0).ObjToStr();
            if (sort.Equals("up"))
            {
                psy_rp.DataSource = list.OrderBy(o=>o.salesAmount);
                psy_rp.DataBind();
            }
            else if (sort.Equals("down"))
            {
                psy_rp.DataSource = list.OrderByDescending(o => o.salesAmount);
                psy_rp.DataBind();
            }
           

        }
        protected void SortImgBtn_OnClick(object sender, ImageClickEventArgs e)
        {
            var sort = SortImgBtn.Attributes["Sort"].ObjToStr();

            if (sort.Equals("down"))
            {
                SortImgBtn.Attributes["Sort"] = "up";
                SortImgBtn.ImageUrl = "/Content/Images/sort33.png";
                PageInit("up");
            }
            else
            {
                SortImgBtn.Attributes["Sort"] = "down";
                SortImgBtn.ImageUrl = "/Content/Images/sort37.png";
                PageInit("down");
            }

        }
    }
}
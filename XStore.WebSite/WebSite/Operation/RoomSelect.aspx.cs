using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Entity;

namespace XStore.WebSite.WebSite.Operation
{
    public partial class RoomSelect : CenterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-常规补货";
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        protected void PageInit() {

            try
            {
                if (userInfo == null)
                {
                    MessageBox.Show(this,"system_alert","用户未绑定");
                    return;
                }
                else
                {
                    if (hotelInfo == null)
                    {
                        MessageBox.Show(this, "system_alert", "酒店信息异常");
                        return;
                    }
                    else
                    {
                        var roomList = context.Query<Cabinet>().Where(o => o.hotel == hotelInfo.id).LeftJoin<Cell>((a, b) => a.mac.Equals(b.mac))
                            .Where((a, b) => b.part == 0 && b.product_id == null)
                            .Select((a, b) => new
                            {
                                a.mac,
                                a.online,
                                a.room

                            }).GroupBy(o => o.mac)
                        .Select(o => new { mac = AggregateFunctions.Max(o.mac), online = AggregateFunctions.Max(o.online), room = AggregateFunctions.Max(o.room) }).ToList();
                        rooms_rp.DataSource = roomList;
                        rooms_rp.DataBind();
                        if (roomList.Count() > 0)
                        {
                            roomSelectDiv.Visible = true;
                            empty_div.Visible = false;
                        }
                        else
                        {
                            roomSelectDiv.Visible = false;
                            empty_div.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "system_alert", "数据异常");
            }
        }
    }
}
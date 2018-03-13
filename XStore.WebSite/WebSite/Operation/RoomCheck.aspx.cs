using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Entity;
using XStore.Entity.Model;

namespace XStore.WebSite.WebSite.Operation
{
    public partial class RoomCheck : CenterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-开箱检查";
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        protected void PageInit()
        {
            var roomList = context.Query<Cabinet>().Where(o => o.hotel == hotelInfo.id).ToList();
            roomStateRepaeater.DataSource = roomList;
            roomStateRepaeater.DataBind();
        }
        #region 获取房间状态
        public RoomState GetRoomStatus(string boxmac)
        {
            string href = string.Format(Constant.JsOperationDic + "RoomGoods.aspx?boxmac={0}", boxmac);
            var cabinet = context.Query<Cabinet>().FirstOrDefault(o => o.mac.Equals(boxmac));
            if (cabinet!=null)
            {
                var salesList = context.Query<Cell>().Where(o => o.mac.Equals(boxmac) && o.part == 0&& o.product_id == null).ToList();
                //在线
                if (cabinet.online.HasValue&&cabinet.online.Value)
                {
                    if (salesList.Count > 0)
                    {
                        return new RoomState() { css = "nep", href = href, icon = "<p class=\"label noEquipment\">配</p>" };
                    }
                    else
                    {
                        return new RoomState() { href = href };
                    }
                }
                else
                {
                    if (salesList.Count > 0)
                    {
                        return new RoomState() { css = "ofl", href = href, icon = "<p class=\"label offLine\">配</p>" };
                    }
                    else
                    {
                        return new RoomState() { css = "ofl", href = href, icon = "<p class=\"label offLine\">离</p>" };
                    }
                }
            }
            else
            {
                return new RoomState();
            }
        }
        #endregion
    }

}
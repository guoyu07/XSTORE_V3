using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Entity;
using static XStore.Entity.Enum;

namespace XStore.WebSite.WebSite.Operation
{
    public partial class HotelSelect : CenterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-酒店列表";
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        private void PageInit()
        {
            Session[Constant.HotelId] = null;
            if (userRole.role_id != (int)UserRoleEnum.区域经理)
            {
                MessageBox.Show(this,"system_alert","您没有权限访问");
                return;
            }
            var list = context.Query<UserHotel>().LeftJoin<Hotel>((a, b) => a.hotels_id == b.id)
                .Where((a, b) => a.user_username.Equals(userInfo.username))
                .Select((a,b) => new
                {
                    id = a.hotels_id,
                    simple_name = b.simple_name
                }).ToList();
            hotel_rp.DataSource = list;
            hotel_rp.DataBind();
        }
    }
}
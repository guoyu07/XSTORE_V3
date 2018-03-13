using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Entity;
using static XStore.Entity.Enum;

namespace XStore.WebSite.WebSite.Information
{
    public partial class Employee : CenterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-人员管理";
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        private void PageInit() {
            var list = context.Query<UserHotel>().LeftJoin<User>((a, b) => a.user_username.Equals(b.username))
                .LeftJoin<UserRole>((a,b,c)=>b.username.Equals(c.username))
                .Where((a, b,c) => a.hotels_id == hotelInfo.id&&c.role_id==(int)UserRoleEnum.前台)
                .Select((a, b,c) => new
                {
                    username = b.username,
                    phone = b.phone,
                    realname = b.realname,

                }).ToList();
            people_repeater.DataSource = list;
            people_repeater.DataBind();
        }
    }
}
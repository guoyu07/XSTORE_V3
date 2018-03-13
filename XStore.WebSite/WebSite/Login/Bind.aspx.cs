using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Entity;

namespace XStore.WebSite.WebSite.Login
{
    public partial class Bind : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-用户绑定";
            if (!IsPostBack)
            {
                PageInit();
            }
        }

        protected void PageInit()
        {
            User model = (User)Session[Constant.CurrentUser];
            Session[Constant.CurrentUser] = null;
            name_input.Value = model.realname;
            phone_input.Value = model.phone;
            account_input.Value = model.username;
        }
    }
}
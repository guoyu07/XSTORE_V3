using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Entity;

namespace XStore.WebSite.WebSite.Center
{
    public partial class EmployeeCenter : CenterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-酒店前台";
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        private void PageInit() {

            if (userInfo == null)
            {
                MessageBox.Show(this, "system_alert", "您未绑定账号");
                return;
            }
        }
    }
}
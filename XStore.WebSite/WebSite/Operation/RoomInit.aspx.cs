using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Entity;

namespace XStore.WebSite.WebSite.Operation
{
    public partial class RoomInit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void markSure_Click(object sender, EventArgs e)
        {
            var url = Constant.OperationDic + "RoomFixed?boxmac=" + mac.Value;
            Response.Redirect(url);
        }
    }
}
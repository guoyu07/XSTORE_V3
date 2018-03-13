using Chloe.MySql;
using Newtonsoft.Json;
using System;
using System.Text;
using XStore.Common;
using XStore.Common.Helper;
using XStore.Common.WeiXinPay;
using XStore.Entity;
using XStore.Entity.Model;

namespace XStore.WebSite.WebSite.Order
{
    public partial class WeiXinNotify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WxPayNotify resultNotify = new WxPayNotify(this);
            resultNotify.ProcessNotify();
        }


    }
}
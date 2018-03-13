using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Common.WeiXinPay;

namespace XStore.WebSite.Content.Oath
{
    public partial class WxOath : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WeiXinOath wxOath = new WeiXinOath();
                string postStr = "";
                if (Request.HttpMethod.ToLower() == "post")
                {
                    Stream s = System.Web.HttpContext.Current.Request.InputStream;
                    byte[] b = new byte[s.Length];
                    s.Read(b, 0, (int)s.Length);
                    postStr = Encoding.UTF8.GetString(b);

                }
                else
                {
                    wxOath.Auth();
                }

            }
        }
    }
}
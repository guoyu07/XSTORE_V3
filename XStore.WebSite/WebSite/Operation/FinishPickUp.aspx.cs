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
    public partial class FinishPickUp : CenterPage
    {
        #region 房间
        private List<Cabinet> _cabinets;
        protected List<Cabinet> cabinets
        {
            get
            {
                if (_cabinets == null)
                {
                    var macsStr = Session[Constant.Cabinets].ObjToStr();
                    if (!string.IsNullOrEmpty(macsStr))
                    {
                        _cabinets = context.Query<Cabinet>().Where(o => macsStr.Contains(o.mac)).ToList();

                    }
                    else
                    {
                        _cabinets = new List<Cabinet>();
                    }
                }
                return _cabinets;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-常规补货";
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        private void PageInit()
        {
            if (cabinets.Count == 0)
            {
                MessageBox.Show(this, "system_alert", "未选择补货房间");
                return;
            }
            rooms_rp.DataSource = cabinets;
            rooms_rp.DataBind();

            if (cabinets.Count > 0)
            {
                list_div.Visible = true;
                empty_div.Visible = false;
            }
            else
            {
                list_div.Visible = false;
                empty_div.Visible = true;
            }

        }
    }
}
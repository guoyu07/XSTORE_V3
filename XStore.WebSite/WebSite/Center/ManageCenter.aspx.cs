using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Entity;
using XStore.Entity.Model;
using static XStore.Entity.Enum;

namespace XStore.WebSite.WebSite.Center
{
    public partial class ManageCenter : CenterPage
    {
        protected ManageQuery manageQuery = new ManageQuery();
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-酒店经理";
            if (!IsPostBack)
            {
            
                PageInit();
            }

        }
        private void PageInit() {
            var sumCabinet = context.Query<Cabinet>().Where(o => o.hotel == hotelInfo.id).ToList();
            manageQuery.trustAssets = sumCabinet.Count * 1600;
            var sumSales = context.Query<OrderInfo>().LeftJoin<Cabinet>((a, b) => a.cabinet_mac.Equals(b.mac)).Where((a, b) => a.paid == true && b.hotel == hotelInfo.id && a.date.Date == DateTime.Now.AddDays(-1).Date).Select((a, b) => new {
                a.price1
            }).ToList().Sum(o => o.price1);

            if (userRole.role_id == (int)UserRoleEnum.区域经理)
            {
                changeHotel.Visible = true;
            }
            else
            {
                changeHotel.Visible = false;
            }
           
            manageQuery.salesLight = sumCabinet.Count == 0 ? 0 : Math.Round((double)(sumSales/sumCabinet.Count), 2);
            manageQuery.lightColor = manageQuery.salesLight > 5 ? "normal" : manageQuery.salesLight >= 2 ? "warning" : "error";
        }
        #region 补货完成
        protected void finish_button_Click(object sender, EventArgs e)
        {
            var backIdList = ((List<int>)ViewState["BackNo"]).Select(o => o.ObjToStr()).ToList();
            var requestUrl = string.Format(Constant.YunApi + "test/back/open?backOrderId={0}", backIdList.Aggregate((x, y) => x + "," + y));
            LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "finishBackNoRequestUrl：" + requestUrl);
            var response = JsonConvert.DeserializeObject<BuyResponse>(Utils.HttpGet(requestUrl));
            LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "finishBackNoResponse：" + JsonConvert.SerializeObject(response));
            if (response.operationStatus.Equals("SUCCESS"))
            {
                MessageBox.Show(this, "system_alert", "补货成功");
                return;
            }
            else
            {
                MessageBox.Show(this, "system_alert", response.operationMessage);
                return;
            }
        }
        #endregion
    }
}
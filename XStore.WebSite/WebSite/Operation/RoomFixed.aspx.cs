using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Common.Helper;
using XStore.Entity;
using XStore.Entity.Model;
using static XStore.Entity.Enum;

namespace XStore.WebSite.WebSite.Operation
{
    public partial class RoomFixed : MacPage
    {
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
            if (userInfo == null)
            {
                MessageBox.Show(this, "system_alert", "用户不存在");
                return;
            }
            if (cabinet == null)
            {
                MessageBox.Show(this, "system_alert", "房间不存在");
                return;
            }

            #region 绑定房间商品
            var proidList = BindGoods(this,box_rp, cabinet);
            //var proidList = context.Query<Cell>().Where(o => o.part == 0 && o.mac.Equals(cabinet.mac)).Select(o => o.product_id.HasValue ? o.product_id.Value : 0).ToList();
            //LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "proidList：" + JsonConvert.SerializeObject(proidList));
            //if (proidList.Count == 0)
            //{
            //    proidList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //}
            //var layout = context.Query<CabinetLayout>().FirstOrDefault(o => o.hotel_id == cabinet.hotel);
            //if (layout == null)
            //{
            //    MessageBox.Show(this, "system_alert", "酒店未设置默认商品");
            //    return;
            //}
            //var layoutProList = layout.products.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "layoutProList：" + JsonConvert.SerializeObject(layoutProList));
            //if (proidList.Count() != layoutProList.Count())
            //{
            //    MessageBox.Show(this, "system_alert", "房间设置商品不全");
            //    return;
            //}
            //List<ProductQuery> list = new List<ProductQuery>();
            //List<Product> productList = context.Query<Product>().Where(o => o.state == 1).ToList();

            //for (int i = 0; i < proidList.Count(); i++)
            //{
            //    var proid = proidList[i].ObjToInt(0);
            //    var sell_out = false;
            //    //如果实际商品是0，则用默认商品补全
            //    if (proid == 0)
            //    {
            //        proid = layoutProList[i].ObjToInt(0);
            //        sell_out = true;
            //    }
            //    var pro = productList.FirstOrDefault(o => o.id == proid);
            //    if (pro == null)
            //    {
            //        continue;
            //    }
            //    var proQuery = TinyMapper.Map<ProductQuery>(pro);
            //    proQuery.sell_out = sell_out;
            //    list.Add(proQuery);
            //}
            //LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "bindList：" + JsonConvert.SerializeObject(list));
            //box_rp.DataSource = list;
            //box_rp.DataBind();
            #endregion

            #region 打开需要补货的格子
            try
            {

                var requestUrl = string.Format(Constant.YunApi + "test/back/start?mac={0}&username={1}", cabinet.mac, userInfo.username);
                LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "backNoRequestUrl：" + requestUrl);
                var response = JsonConvert.DeserializeObject<BackNoResponse>(Utils.HttpGet(requestUrl));
                LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "backNoResponse：" + JsonConvert.SerializeObject(response));
                if (!response.operationStatus.Equals("SUCCESS"))
                {
                    MessageBox.Show(this, "system_alert", "补货单生成失败");
                    return;
                }        
                ViewState["BackNo"] = response.operationMessage;
                var position =string.Empty;
                for (int i = 0; i < proidList.Count; i++)
                {
                    if (proidList[i] == 0)
                    {
                        position += i + ",";
                    }
                }
                if (!string.IsNullOrEmpty(position))
                {
                    position = position.TrimEnd(',');
                    var rbh = new RemoteBoxHelper();
                    rbh.OpenRemoteBox(cabinet.mac, string.Empty, position, 0x02);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "system_alert", "数据异常：" + ex.Message + ";内部异常：" + ex.InnerException.Message);
                return;
            }
            #endregion
        }
        protected void open_again_Click(object sender, EventArgs e)
        {
            var position = string.Empty;
            var proidList = context.Query<Cell>().Where(o => o.part == 0 && o.mac.Equals(cabinet.mac)).Select(o => o.product_id.HasValue ? o.product_id.Value : 0).ToList();
            LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "needOpenAgain：" + JsonConvert.SerializeObject(proidList));
            if (proidList.Count == 0)
            {
                proidList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }
            for (int i = 0; i < proidList.Count; i++)
            {
                if (proidList[i] == 0)
                {
                    position += i + ",";
                }
            }
            if (!string.IsNullOrEmpty(position))
            {
                position = position.TrimEnd(',');
                var rbh = new RemoteBoxHelper();
                rbh.OpenRemoteBox(cabinet.mac,string.Empty, position, 0x02);
            }
        }

        #region 补货完成
        protected void finish_button_Click(object sender, EventArgs e)
        {
            var redirctUrl = string.Empty;
          
            if (ViewState["BackNo"] == null || ((List<int>)ViewState["BackNo"]).Count == 0)
            {
                MessageBox.Show(this, "system_alert", "无需补货");
                PageInit();
                return;
            }
           
            var backIdList = ((List<int>)ViewState["BackNo"]).Select(o=>o.ObjToStr()).ToList();
            var requestUrl = string.Format(Constant.YunApi + "test/back/open?backOrderId={0}", backIdList.Aggregate((x, y) => x + "," + y));
            LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "finishBackNoRequestUrl：" + requestUrl);
            var response = JsonConvert.DeserializeObject<BuyResponse>(Utils.HttpGet(requestUrl));
            LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "finishBackNoResponse：" + JsonConvert.SerializeObject(response));
            if (response.operationStatus.Equals("SUCCESS"))
            {
                MessageBox.Show(this, "system_alert", "补货成功");
                PageInit();
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
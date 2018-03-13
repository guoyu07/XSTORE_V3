using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Common.Helper;
using XStore.Entity;
using XStore.Entity.Model;
using static XStore.Entity.Enum;

namespace XStore.WebSite.WebSite.Operation
{
    public partial class RoomGoods : CenterPage
    {
        #region 房间
        private Cabinet _cabinet;
        protected Cabinet cabinet
        {
            get
            {
                if (_cabinet == null)
                {
                    var boxMac = Request.QueryString[Constant.IMEI].ObjToStr();
                    _cabinet = context.Query<Cabinet>().FirstOrDefault(o => o.mac.Equals(boxMac));
                }
                return _cabinet;
            }
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-开箱检查";
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        protected void PageInit()
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
            BindGoods(this,box_rp,cabinet);
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
           
        }
        protected void SingleOpenBoxClick(object sender, EventArgs e)
        {
            //if (userRole == null)
            //{
            //    MessageBox.Show(this, "system_alert", "配置的商品不存在");
            //    return;
            //}
            if (userRole ==null || (userRole.role_id != (int)UserRoleEnum.经理 && userRole.role_id != (int)UserRoleEnum.区域经理))
            {
                MessageBox.Show(this, "system_alert", "您没有权限");
                return;
            }
           
            var position = ((HtmlAnchor)sender).Attributes["position"].ObjToInt(0);

            OpenSingleBox(position);
        }
        protected void makeSure_ServerClick(object sender, EventArgs e)
        {
            var rbh = new RemoteBoxHelper();

            var postion_list = "0,1,2,3,4,5,6,7,8,9,10,11";
            var boxmac = Request.QueryString["boxmac"].ObjToStr();
            try
            {
                rbh.OpenRemoteBox(boxmac, string.Empty, postion_list);

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "system_alert", "开箱异常");
                return;
            }
        }
        protected void OpenSingleBox(int position)
        {

            var rbh = new RemoteBoxHelper();
            try
            {
                rbh.OpenRemoteBox(cabinet.mac,string.Empty, position.ObjToStr());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "system_alert", "开箱异常");
                return;
            }
        }
        protected void finishCheck_Click(object sender, EventArgs e)
        {
            //返回房间列表页面
            Response.Redirect(Constant.OperationDic + "RoomCheck.aspx", false);
            return;
        }
    }
}
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

namespace XStore.WebSite.WebSite.Operation
{
    public partial class WaterFillUp : MacPage
    {
        #region 酒店信息
        private Hotel _hotelInfo;
        public Hotel hotelInfo
        {
            get
            {
                if (_hotelInfo == null)
                {
                    if (Session[Constant.HotelId].ObjToInt(0) == 0)
                    {
                        var hotelId = Request.QueryString[Constant.HotelId].ObjToInt(0);
                        if (hotelId != 0)
                        {
                            _hotelInfo = context.Query<Hotel>().FirstOrDefault(o => o.id == hotelId);
                            Session[Constant.HotelId] = _hotelInfo.id;
                        }
                        else
                        {
                            _hotelInfo = context.Query<Hotel>().LeftJoin<UserHotel>((a, b) => a.id == b.hotels_id).Where((a, b) => b.user_username.Equals(userInfo.username)).Select((a, b) => new Hotel
                            {
                                id = a.id,
                                hotel_name = a.hotel_name,
                                simple_name = a.simple_name,
                                address = a.address
                            }).FirstOrDefault();
                            Session[Constant.HotelId] = _hotelInfo.id;
                        }
                    }
                    else
                    {
                        var hotelId = Session[Constant.HotelId].ObjToInt(0);
                        _hotelInfo = context.Query<Hotel>().FirstOrDefault(o => o.id == hotelId);
                    }

                }
                return _hotelInfo;
            }
        }
        #endregion
        public List<int> position_list;
        public int kwid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PageInit();
                OpenBox();
            }
        }
        protected void PageInit()
        {

           
        }
        protected void OpenBox()
        {
            var proidList = context.Query<Cell>()
                .LeftJoin<Product>((a,b)=>a.product_id==b.id)
                .Where((a,b) => a.part == 0 && a.mac.Equals(cabinet.mac)&& !a.product_id.HasValue && b.price1<=100)
                .Select((a,b) => a.pos)
                .ToList();
            if (proidList.Count == 0)
            {
                proidList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }
            if (proidList.Count>0)
            {
                var requestUrl = string.Format(Constant.YunApi + "test/back/startWater?mac={0}", cabinet.mac);
                LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "backNoRequestUrl：" + requestUrl);
                var response = JsonConvert.DeserializeObject<BackNoResponse>(Utils.HttpGet(requestUrl));
                LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "backNoResponse：" + JsonConvert.SerializeObject(response));
                if (!response.operationStatus.Equals("SUCCESS"))
                {
                    MessageBox.Show(this, "system_alert", "补货单生成失败");
                    return;
                }
                if (response.operationMessage.Count==0)
                {
                    MessageBox.Show(this, "system_alert", "暂无促销品补货任务");
                    return;
                }
                ViewState["BackNo"] = response.operationMessage;

                var rbh = new RemoteBoxHelper();
                rbh.OpenRemoteBox(cabinet.mac,string.Empty, "0", 0x02);
            }
            else
            {
                MessageBox.Show(this, "system_alert", "暂无促销品补货任务");
                return;
            }
        }

        protected void water_fillup_ServerClick(object sender, EventArgs e)
        {
            var backIdList = ((List<int>)ViewState["BackNo"]).Select(o => o.ObjToStr()).ToList();
            var requestUrl = string.Format(Constant.YunApi + "test/back/open?backOrderId={0}", backIdList.Aggregate((x, y) => x + "," + y));
            LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "finishBackNoRequestUrl：" + requestUrl);
            var response = JsonConvert.DeserializeObject<BuyResponse>(Utils.HttpGet(requestUrl));
            LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "finishBackNoResponse：" + JsonConvert.SerializeObject(response));
            if (response.operationStatus.Equals("SUCCESS"))
            {
                MessageBox.Show(this, "system_alert", "补货成功");
                water_fillup.Visible = false;
                return;
            }
            else
            {
                MessageBox.Show(this, "system_alert", response.operationMessage);
                return;
            }
          
        }
    }
}
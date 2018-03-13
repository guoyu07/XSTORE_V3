using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Entity;
using static XStore.Entity.Enum;

namespace XStore.WebSite.WebSite.Login
{
    public partial class Welcome : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间";
            LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "Welcome：");
            PageInit();
        }
        private void PageInit()
        {
            if (Request.QueryString[Constant.IMEI] != null)
            {
                Session[Constant.IMEI] = string.Empty;
                Session[Constant.IMEI] = Request.QueryString[Constant.IMEI].ObjToStr();
            }
            else
            {
                MessageBox.Show(this, "system_alert", "箱子无效");
                return;
            }

            var userInfo = context.Query<User>().FirstOrDefault((o) => o.weichat.Equals(OpenId));
            if (userInfo != null)
            {
                var boxMac = Session[Constant.IMEI].ObjToStr();
                Session[Constant.CurrentUser] = userInfo;
                //0：正常；-1：删除
                var roomInfo = context.Query<Cabinet>().FirstOrDefault(o => o.mac.Equals(boxMac) && o.state != -1);
                if (roomInfo == null)
                {
                    Response.Redirect(string.Format("http://x.x-store.com.cn/shop/pages/enter.aspx?boxmac={0}",boxMac), false);
                    //MessageBox.Show(this, "system_alert", "箱子未绑定房间");
                    return;
                }
                var roleInfo = context.Query<UserRole>().FirstOrDefault(o => o.username == userInfo.username);
                var userHotelInfo = context.Query<UserHotel>().FirstOrDefault(o => o.user_username == userInfo.username && o.hotels_id == roomInfo.hotel);
                if (userHotelInfo == null)
                {
                    Response.Redirect(Constant.LoginDic + "NoAuth.aspx");
                    return;
                }
                
                switch ((UserRoleEnum)roleInfo.role_id.ObjToInt(0))
                {
                    case UserRoleEnum.前台:
                    case UserRoleEnum.经理:
                    case UserRoleEnum.区域经理:
                        Response.Redirect(string.Format(Constant.OperationDic+ "RoomFixed.aspx?boxmac={0}", boxMac));
                        break;
                    case UserRoleEnum.测试员:
                        Response.Redirect(string.Format(Constant.OperationDic + "BoxCheck.aspx?boxmac={0}", boxMac));
                        break;
                    case UserRoleEnum.配水员:
                        Response.Redirect(string.Format(Constant.OperationDic + "WaterFillUp.aspx?boxmac={0}", boxMac));
                        break;
                    case UserRoleEnum.财务:
                    case UserRoleEnum.集团经理:
                    case UserRoleEnum.集团财务:
                    case UserRoleEnum.后台财务:
                    case UserRoleEnum.后台管理员:
                    default:
                        Response.Redirect(string.Format(Constant.GoodsDic + "GoodsList.aspx?boxmac={0}", boxMac));
                        break;
                }
            }
        }

        protected void submit_button_ServerClick(object sender, EventArgs e)
        {
            var boxMac = Session[Constant.IMEI].ObjToStr();
            var roomInfo = context.Query<Cabinet>().FirstOrDefault(o => o.mac.Equals(boxMac) && o.state != -1);
            if (roomInfo == null)
            {
                MessageBox.Show(this, "system_alert", "箱子未绑定房间");
                return;
            }
            if (Session[Constant.CurrentUser] == null)
            {
                Response.Redirect(string.Format(Constant.GoodsDic + "GoodsList.aspx?boxmac={0}", boxMac));
            }
        }
    }
}
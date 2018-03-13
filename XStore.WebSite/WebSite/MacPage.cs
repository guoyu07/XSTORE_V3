using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Entity;
using XStore.Entity.Model;

namespace XStore.WebSite.WebSite
{
    public class MacPage:BasePage
    {
        #region 房间
        private Cabinet _cabinet;
        protected Cabinet cabinet
        {
            get
            {
                if (_cabinet == null)
                {
                    var boxMac = string.Empty;
                    if (Session[Constant.IMEI] == null || string.IsNullOrEmpty(Session[Constant.IMEI].ObjToStr()))
                    {
                        boxMac = Request.QueryString[Constant.IMEI].ObjToStr();
                        Session[Constant.IMEI] = boxMac;
                    }
                    else
                    {
                        boxMac = Session[Constant.IMEI].ObjToStr();
                    }

                    _cabinet = context.Query<Cabinet>().FirstOrDefault(o => o.mac.Equals(boxMac));
                }
                return _cabinet;
            }
        }
        #endregion
      
        #region 用户信息
        private User _userinfo;
        public User userInfo
        {
            get
            {
                if (_userinfo == null)
                {
                    _userinfo = context.Query<User>().FirstOrDefault(o => o.weichat.Equals(OpenId));
                    if (_userinfo != null)
                    {
                        Session[Constant.CurrentUser] = _userinfo;
                    }
                }
                return _userinfo;
            }
        }
        #endregion

      
    }
}
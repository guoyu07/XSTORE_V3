using Chloe.MySql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Common.WeiXinPay;
using XStore.Entity;
using XStore.WebSite.DBFactory;
using static XStore.Entity.Enum;

namespace XStore.WebSite.WebSite._Ascx
{
    public partial class CenterFooter : System.Web.UI.UserControl
    {
        public static string connString = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        MySqlContext context = new MySqlContext(new MySqlConnectionFactory(connString));
        private string _openid;
        #region 用户信息
        private User _userinfo;
        public User userInfo
        {
            get
            {
                if (_userinfo == null)
                {
                    var openId = Session[Constant.OpenId].ObjToStr();

                    _userinfo = context.Query<User>().FirstOrDefault(o => o.weichat.Equals(openId));
                    if (_userinfo != null)
                    {
                        Session[Constant.CurrentUser] = _userinfo;
                    }
                }
                return _userinfo;
            }
        }
        #endregion

        #region 用户权限
        private UserRole _userRole;

        public UserRole userRole
        {
            get
            {
                if (_userRole == null)
                {
                    _userRole = context.Query<UserRole>().FirstOrDefault(o => o.username.Equals(userInfo.username));
                }
                return _userRole;
            }
        }
        #endregion
        public string CenterPage = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            switch ((UserRoleEnum)userRole.role_id)
            {
                case UserRoleEnum.前台:
                    CenterPage = "EmployeeCenter.aspx";
                    break;
                case UserRoleEnum.区域经理:
                case UserRoleEnum.经理:
                    CenterPage = "ManageCenter.aspx";
                    break;
                default:break;
            }

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Entity;

namespace XStore.WebSite.WebSite.Login
{
    public partial class ChangePassword : CenterPage
    {
        private User _changeUserInfo;

        protected User changeUserInfo
        {
            get
            {
                if (_changeUserInfo == null)
                {
                    var username = Request.QueryString["username"].ObjToStr();
                    if (!string.IsNullOrEmpty(username.ObjToStr()))
                    {
                        _changeUserInfo = context.Query<User>().FirstOrDefault(o => o.username.Equals(username));
                    }
                    else
                    {
                        _changeUserInfo = context.Query<User>().FirstOrDefault(o => o.username.Equals(userInfo.username));
                    }
                }
                return _changeUserInfo;
            }


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-修改密码";
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        private void PageInit()
        {
            if (changeUserInfo == null)
            {
                MessageBox.Show(this, "system_alert", "该用户不存在");
                return;
            }
        }
    }
}
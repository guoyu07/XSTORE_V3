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
    public partial class Oath : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PageInit();
            }
        }
        protected void PageInit()
        {
            try
            {
                UserQuery userModel = context.Query<User>().Where(o => o.weichat.Equals(OpenId)).LeftJoin<UserRole>((a, b) => a.username.Equals(b.username)).Select((a, b) => new UserQuery()
                {
                    username = a.username,
                    password = a.password,
                    role_id = b.role_id,
                    weichat = a.weichat
                }).FirstOrDefault();
                LogHelper.WriteLogs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "SessionOpenId："+Session[Constant.OpenId]);
                if (userModel == null)
                {
                    Response.Redirect(Constant.LoginDic + "Login.aspx");
                    return;
                }
                else
                {
                    var url = string.Empty;
                    switch ((UserRoleEnum)userModel.role_id)
                    {
                        case UserRoleEnum.经理://酒店经理
                            url = Constant.CenterDic + "ManageCenter.aspx";
                          
                            break;
                        case UserRoleEnum.财务://财务
                            url = Constant.CenterDic + "FinanceCenter.aspx";
                            //Response.Redirect(Constant.CenterDic + "FinanceCenter.aspx");
                            break;
                        case UserRoleEnum.前台://配货员
                            url = Constant.CenterDic + "EmployeeCenter.aspx";
                            //Response.Redirect(Constant.CenterDic + "EmployeeCenter.aspx");
                            break;
                        case UserRoleEnum.区域经理://区域经理
                            url = Constant.CenterDic + "ManageCenter.aspx";
                            //Response.Redirect(Constant.CenterDic + "ManageCenter.aspx");
                            break;
                        case UserRoleEnum.测试员:
                            url = Constant.CenterDic + "TestCenter.aspx";
                            //Response.Redirect(Constant.CenterDic + "TestCenter.aspx");
                            break;
                        case UserRoleEnum.配水员:
                            url = Constant.CenterDic + "PromotionCenter.aspx";
                            //Response.Redirect(Constant.CenterDic + "PromotionCenter.aspx");
                            break;
                    }
                    Response.Redirect(url);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "system_alert", "网络异常");
                return;
            }

        }
    }
}
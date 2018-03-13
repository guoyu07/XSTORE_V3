using Chloe.MySql;
using log4net;
using System;
using System.Linq;
using System.Web;
using XStore.Common;
using XStore.Entity;
using XStore.WebSite.DBFactory;
using static XStore.Entity.Enum;

namespace XStore.WebSite.WebSite.Login
{
    public partial class Login : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Log.Info(DateTime.Now+" "+"进入登陆页面");
            Title = "幸事多私享空间";
        }
        protected void loginBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = username.Value;
                string password = pswd.Value;
                UserQuery userModel = context.Query<User>().Where(o => o.username.Equals(userName) && o.password.Equals(password) && o.state == ((int)UserStateEnum.启用)).LeftJoin<UserRole>((a, b) => a.username.Equals(b.username)).Select((a, b) => new UserQuery()
                {
                    username = a.username,
                    password = a.password,
                    role_id = b.role_id,
                    weichat = a.weichat
                }).FirstOrDefault();
                if (userModel != null)
                {
                    bool is_same = string.IsNullOrEmpty(userModel.weichat.ObjToStr()) ? false : userModel.weichat.Equals(OpenId);
                    if (!string.IsNullOrEmpty(userModel.weichat)&&!is_same)
                    {
                        MessageBox.Show(this,"system_alert", "用户已绑定");
                    }
                    else
                    {
                       
                        Session[Constant.CurrentUser] = userModel;
                        #region 如果登陆成功，则记录登陆日志
                        context.Insert<LoginRecord>(new LoginRecord
                        {
                            date = DateTime.Now,
                            ip = Utils.GetUserIp(),
                            state = 1,
                            username = userModel.username
                        });
                        #endregion
                        #region 登陆完成跳转
                        var redirectUrl = string.Empty;
                        switch ((UserRoleEnum)userModel.role_id)
                        {
                            case UserRoleEnum.经理://酒店经理
                                MessageBox.Show(this, "system_alert", is_same ? "登陆成功" : "登陆成功，请修改初始密码！");
                                Response.Redirect(is_same ? Constant.CenterDic + "ManageCenter.aspx" : Constant.LoginDic + "Bind.aspx");
                                break;
                            case UserRoleEnum.财务://财务
                                MessageBox.Show(this, "system_alert", is_same ? "登陆成功" : "登陆成功，请修改初始密码！");
                                Response.Redirect(is_same ? Constant.CenterDic + "FinanceCenter.aspx" : Constant.LoginDic + "Bind.aspx");
                                break;
                            case UserRoleEnum.前台://配货员
                                MessageBox.Show(this, "system_alert", is_same ? "登陆成功" : "登陆成功，请修改初始密码！"); ;
                                Response.Redirect(is_same ? Constant.CenterDic + "EmployeeCenter.aspx" : Constant.LoginDic + "Bind.aspx");
                                break;
                            case UserRoleEnum.区域经理://区域经理
                                MessageBox.Show(this, "system_alert", is_same ? "登陆成功" : "登陆成功，请修改初始密码！");
                                Response.Redirect(is_same ? Constant.CenterDic + "ManageCenter.aspx" : Constant.LoginDic + "Bind.aspx");
                               
                                break;
                            case UserRoleEnum.测试员:
                                MessageBox.Show(this, "system_alert", is_same ? "登陆成功" : "登陆成功，请修改初始密码！");
                                Response.Redirect(is_same ? Constant.CenterDic + "TestCenter.aspx" : Constant.LoginDic + "Bind.aspx");
                                break;
                            case UserRoleEnum.配水员:
                                MessageBox.Show(this, "system_alert", is_same ?"登陆成功":"登陆成功，请修改初始密码！");
                                Response.Redirect(is_same ? Constant.CenterDic + "PromotionCenter.aspx" : Constant.LoginDic + "Bind.aspx");
                                break;
                        }
                        #endregion

                    }
                }
                else
                {
                    MessageBox.Show(this, "system_alert", "登陆失败");
                    context.Insert<LoginRecord>(new LoginRecord
                    {
                        date = DateTime.Now,
                        ip = Utils.GetUserIp(),
                        state = 0,
                        username = userModel.username
                    });
                }
             
            }
            catch (Exception ex)
            {
                Log.Info( "异常信息：" + ex.Message);
                MessageBox.Show(this, "system_alert", "系统异常");
            }

        }
       
    }
}
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Employee.aspx.cs" Inherits="XStore.WebSite.WebSite.Information.Employee" %>
<%@ Register Src="~/WebSite/_Ascx/CenterFooter.ascx" TagPrefix="uc" TagName="Footer" %>
<%@ Import Namespace="XStore.Entity" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" />
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/employee/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
</head>
<body>
    <form id="form1" runat="server">
        <div class="interval"></div>
        <div class="main" style="-webkit-overflow-scrolling: touch;">
            <dl class="manager">
                <dt>人员信息管理</dt>
                <asp:Repeater runat="server" ID="people_repeater">
                    <ItemTemplate>
                        <dd>
                            <div class="link_a">
                                <h3>账号：<strong><%#Eval("username") %></strong></h3>
                                <p>姓名： <span><%#Eval("realname") %></span>;&nbsp;&nbsp;&nbsp;&nbsp;电话：<span><%#Eval("phone") %></span></p>
                            </div>
                            <div class="modify_a"><a href='<%#Constant.JsLoginDic+"ChangePassword.aspx?username="+Eval("username") %>'>修改</a></div>
                        </dd>
                    </ItemTemplate>
                </asp:Repeater>
            </dl>
        </div>
         <div class="footer_bar openwebview">
            <uc:Footer ID="UserFooter" runat="server" EnableViewState="False"></uc:Footer>
        </div>
    </form>
</body>
</html>

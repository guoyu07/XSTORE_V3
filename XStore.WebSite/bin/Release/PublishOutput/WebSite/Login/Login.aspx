<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="XStore.WebSite.WebSite.Login.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />
    <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />
    <title><%=Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/login")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
</head>
<body>
    <form id="form1" runat="server">
        <div class="imgWrap">
            <img src="/Content/Icon/logo.png" alt="" />
        </div>
        <div class="account">
            <input id="username" runat="server" type="text" placeholder="请输入用户名" />
        </div>

        <div class="password">
            <span><input id="pswd" runat="server" type="password" placeholder="请输入密码" /></span>
        </div>
        <div class="btnWrap">
            <asp:Button ID="loginBtn" runat="server" OnClick="loginBtn_Click" Text="登录" CssClass="loginBtn" Style="border: none;" />
        </div>
    </form>
</body>
</html>



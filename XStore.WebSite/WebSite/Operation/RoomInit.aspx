<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoomInit.aspx.cs" Inherits="XStore.WebSite.WebSite.Operation.RoomInit" %>

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
        <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/login")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
</head>
<body>
    <form id="form1" runat="server">
        <div class="imgWrap">
        </div>
        <div class="account">
            <input id="mac" runat="server" type="text" placeholder="请输入IMEI" />
        </div>
        <div class="btnWrap">
            <asp:Button ID="markSure" runat="server" OnClick="markSure_Click" Text="确定" CssClass="loginBtn" Style="border: none;" />
        </div>
    </form>
</body>
</html>

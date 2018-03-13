<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaySuccess.aspx.cs" Inherits="XStore.WebSite.WebSite.Order.PaySuccess" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%:Title %></title>
    <meta name="viewport" charset="UTF-8" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/paysuccess/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/weui/js","~/bundles/paysuccess/js")%>
</head>
<body runat="server">
    <form id="form1" runat="server">
        <div class="banner clearfix">
            <div class="l">
                <span class="suc">开箱成功</span>
            </div>
            <asp:Image ID="title_img" runat="server" CssClass="r"/>
        </div>
        <div class="total_money">
            合计：<i>¥ <span>  <%= orderInfo.price1.CentToRMB(0) %></span></i>
        </div>
       <div class="remind"><p>请付款后15分钟内开箱取走商品，否则将视作放弃！</p></div>
    </form>
</body>
</html>

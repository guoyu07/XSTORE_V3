<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayFail.aspx.cs" Inherits="XStore.WebSite.WebSite.Order.PayFail" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%:Title %></title>
    <meta name="viewport" charset="UTF-8" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/payfail/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
</head>
<body>
    <form id="form1" runat="server">
        <div class="top clearfix">
            <div class="tips">
                <p>啊哦，信号好像迷路了。</p>
            </div>
            <div class="tips-logo">
                 <asp:Image ID="title_img" runat="server" />
            </div>
        </div>
        <div class="tips-product">
            <p>商品名:<%=productInfo.name %> &nbsp&nbsp&nbsp售价:<%=productInfo.price1.CentToRMB(0) %> </p>
        </div>
        <div class="middle">
            <h3 >别着急，您可以<span>拔插电源</span>来帮助信号回家</h3>
              <asp:Image ID="lc1" runat="server" />
              <asp:Image ID="lc2" runat="server" />
              <asp:Image ID="lc3" runat="server" />
        </div>
        <div class="bottom tips-details">
            <h3>重新插拔之后还没开箱？</h3>
            <p>点击<a href="tel:400-880-2482">此处</a>拨打客服电话</p>
        </div>
    </form>
</body>
</html>

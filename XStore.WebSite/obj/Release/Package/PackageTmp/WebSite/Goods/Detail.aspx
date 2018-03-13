<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="XStore.WebSite.WebSite.Goods.Detail" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="UTF-8" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />
    <title><%=Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/swiper/css","~/bundles/weui/css","~/bundles/detail/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/swiper/js","~/bundles/weui/js")%>

</head>
<body runat="server">
    <form id="form1" runat="server" style="height: 100%;">
        <div class="main" style="-webkit-overflow-scrolling: touch; overflow: auto!important;">
            <div class="banner swiper-container">
                <div class="imgWrap">
                    <img src="<%=GetProductImg(product.id.ObjToInt(0),product.image.ObjToStr()) %>" alt=""/>
                </div>
                <div class="swiper-pagination"></div>
            </div>
            <div class="clearfix pd3">
                <div class="l goodsName"><%=product.name %></div>
                <div class="r goodsPrice">¥ <span><%=product.price1.ObjToInt(0).CentToRMB(0) %></span></div>
            </div>
   
            <div class="goodsInfo pd3">
                <ul class="topic clearfix">
                    <li class="clickOn">商品详情</li>

                </ul>
                <dl class="goodsContent">
                    <dd class="showTime" id="product_detail">
                       <%-- <iframe id="iframeWin" src="<%=GetProductHtml(product.html.ObjToStr())%>" style="width:100%;height:auto; "></iframe>--%>
                    </dd>
                </dl>
            </div>
        </div>
        <div class="settlement clearfix">
            <div class="priceInfo l">
                价格: ¥ <span><%=product.price1.ObjToInt(0).CentToRMB(0) %></span>
            </div>
            <input type="button" runat="server" id="buy" class="buyBtn r" style="border: none;" onserverclick="buy_ServerClick" value="立即购买" />
        </div>
    </form>
</body>
   
</html>
<script type="text/javascript">
    $(function () {
        $('#product_detail').load("<%=GetProductHtml(product.html.ObjToStr())%>");
    });
    
</script>
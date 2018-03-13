<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoodsList.aspx.cs" Inherits="XStore.WebSite.WebSite.Goods.GoodsList" %>
<%@ Register Src="~/WebSite/_Ascx/Footer.ascx" TagPrefix="uc" TagName="Footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" />
    <title><%=Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/goodslist/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/goodslist/js")%>
</head>
<body>
    <form id="form1" runat="server">
        <div id='view' style="-webkit-overflow-scrolling: touch; overflow: auto!important;">
            <div id="mySpace">
                <ul class="clearfix">
                    <asp:Repeater ID="goods_list" runat="server">
                        <ItemTemplate>
                            <li class="<%#(bool)Eval("sell_out")?"kong":"" %>">
                                <a href="<%#link_detail((bool)Eval("sell_out"),Container.ItemIndex,(int)Eval("id")) %>">
                                    <div class="pic">
                                        <img src="<%# GetProductImg(Eval("id").ObjToInt(0),Eval("image").ObjToStr())%>" alt="" />
                                        <p class="goodsName over"><%#Eval("name")%></p>

                                    </div>
                                    <div class="price">¥ <span><%#Eval("price1").ObjToInt(0).CentToRMB(0)%></span></div>
                                </a>
                                <div class="model">
                                    <p>暂无商品</p>
                                </div>
                            </li>

                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div class="info">
                    <div class="infoWrapper">
                        <p class="topic">私密的事情私密地做</p>
                        <p>1.购买过程中，不需填写任何个人私密信息，就可以从平台上轻松购买，获取商品。</p>
                        <p>2.在酒店特定环境里，您可以放心使用这些私密产品。</p>
                        <p>3.使用后的产品，您可以方便地随身带走。</p>
                        <p>4.多种商品，供您选择。如想一次购买多个商品，可添加到购物车内，一次支付。当然，您也可以单独购买。</p>

                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <br />
        <div class="footer_bar openwebview">
            <uc:Footer ID="UserFooter" runat="server" EnableViewState="False"></uc:Footer>
        </div>
    </form>

</body>
</html>

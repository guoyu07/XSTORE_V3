<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyOrder.aspx.cs" Inherits="XStore.WebSite.WebSite.Order.MyOrder" %>

<%@ Register Src="~/WebSite/_Ascx/Footer.ascx" TagPrefix="uc" TagName="Footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" />
    <title><%=Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/goodslist/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/myorder/js")%>
</head>
<body>
    <form id="form1" runat="server">
        <div id='view' style="-webkit-overflow-scrolling: touch; overflow: auto!important;">
            <div id="myself">
                <div class="hasOrder" id="has" runat="server">
                    <ul class="goodsList">
                        <asp:Repeater ID="myOrderRepeater" runat="server">
                            <ItemTemplate>
                                <li class="clearfix">
                                    <img class="l" src="<%#GetProductImg(Eval("id").ObjToInt(0),Eval("image").ObjToStr()) %>" alt="" />
                                    <div class="r">
                                        <p class="goodsName">
                                            <%#Eval("name").ToString()%>
                                        </p>
                                        <p class="price">
                                            ¥ <span><%#Eval("price1").ObjToInt(0).CentToRMB(0)%></span>
                                        </p>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>

                <div class="noOrder" id="empty" runat="server">
                    <div class="imgWrap">
                        <img src="/Content/Images/null.png" alt="" />
                    </div>
                    <p>无待开箱订单</p>
                </div>
                <div class="remind" id="title" runat="server">
                    <table>
                        <tr>
                            <td colspan="2" style="padding: 10px;">
                                <p style="text-align: left;">开箱失败自主解决：</p>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>1.</p>
                            </td>
                            <td style="padding: 10px;">
                                <p style="text-align: left;">请对售货机断电10秒钟后上电，等LED灯带闪烁停止后，然后点击“立即开箱”，即可实现取货。</p>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p>2.</p>
                            </td>
                            <td style="padding: 10px;">
                                <p style="text-align: left;">如果还有问题，请拨打前台电话，由前台送货。</p>
                            </td>
                        </tr>
                    </table>
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

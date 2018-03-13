<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoomGoods.aspx.cs" Inherits="XStore.WebSite.WebSite.Operation.RoomGoods" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" />
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/roomgoods/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
</head>
<body>
    <form id="form1" runat="server">
        <div class="clearfix topTitle">
            <p class="roomNumber">房间名：<%=cabinet.room %></p>
        </div>
        <div class="interval"></div>
         <div id='view' style="-webkit-overflow-scrolling: touch; overflow: auto!important;">
            <div id="mySpace" class="roomGoods">
                <ul class="clearfix" style="margin-bottom:40px;">
                    <asp:Repeater ID="box_rp" runat="server">
                        <ItemTemplate>
                            <li class="<%#(bool)Eval("sell_out")?"kong":"" %>" >
                                <a href="#" position='<%#Container.ItemIndex %>' runat="server" onserverclick="SingleOpenBoxClick">
                                    <div class="pic">
                                        <img src="<%#GetProductImg(Eval("id").ObjToInt(0),Eval("image").ObjToStr())%>" alt="" />
                                        <p class="goodsName over"><span style="font-weight: bolder"><%#Eval("code")%>&nbsp;&nbsp;<%#Eval("name")%></p>

                                    </div>
                                    <div class="price">¥ <span><%#Eval("price1").ObjToInt(0).CentToRMB(0)%></span></div>
                               
                                <div class="model">
                                    <p>暂无商品</p>
                                </div>
                                     </a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <table style=" width:100%;">
                    <tr>
                        <td style=" width:45%;">
                            <div class="btnWrap">
                                <asp:LinkButton runat="server" ID="markSure" CssClass="makeSure" OnClick="makeSure_ServerClick">全部开箱</asp:LinkButton>
                            </div>
                        </td>
                        <td style=" width:45%;">
                            <div class="btnWrap">
                                <asp:LinkButton runat="server" ID="finishCheck" CssClass="finishSure" OnClick="finishCheck_Click">完成检查</asp:LinkButton>

                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>

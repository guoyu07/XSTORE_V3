<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoomFixed.aspx.cs" Inherits="XStore.WebSite.WebSite.Operation.RoomFixed" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" />
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
        <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/roomfixed/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
    <script type="text/javascript">
        function confirm_all_fixed() {
            layer.open({
                content: '确定已全部配货完成？',
                btn: ['确定', '取消'],
                yes: function (index) {

                    $("#finishButton").click();
                }
            })
        }
    </script>
</head>
<body>
    <form id="form2" runat="server">
        <div class="clearfix topTitle">
            <p class="roomNumber">房间名：<span><%=cabinet.room %></span></p>
            <p class="distributer">箱子码：<span><%=cabinet.mac %></span></p>
        </div>
        <div class="interval"></div>
        <div id='view' style="-webkit-overflow-scrolling: touch; overflow: auto!important;">
            <div id="mySpace">
                <ul class="clearfix" style="margin-bottom: 40px;">
                    <asp:Repeater ID="box_rp" runat="server">
                        <ItemTemplate>
                            <li class="<%#(bool)Eval("sell_out")?"kong":"" %>">
                                <a href="#">
                                    <div class="pic">
                                        <img src="<%#GetProductImg(Eval("id").ObjToInt(0),Eval("image").ObjToStr())%>" alt="" />
                                        <p class="goodsName over"><span style="font-weight: bolder"><%#Eval("code")%>&nbsp;&nbsp;<%#Eval("name")%></p>

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
                <br />
                  <br />
                <table id="pickUp" style="width: 100%;">
                    <tr>
                        <td style="width: 50%;">
                            <div class="btnWrap">
                                <asp:LinkButton runat="server" ID="openAgain" CssClass="makeSure" OnClick="open_again_Click">点击开箱</asp:LinkButton>
                            </div>
                        </td>
                        <td style="width: 50%;">
                            <div class="btnWrap">
                               <a class="makeSure"  onclick ="return confirm_all_fixed();">补货完成</a>
                                <asp:Button runat="server" id="finishButton" style="display:none;" OnClick="finish_button_Click"/>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>

</body>
</html>

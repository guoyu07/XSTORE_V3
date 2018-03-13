<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoomInfo.aspx.cs" Inherits="XStore.WebSite.WebSite.Information.RoomInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/roominfo/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
    <script>
        function sort_amount_click() {
            $("#SortImgBtn").click();

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input hidden value="0" id="hiddenButton" />
        <div class="roomInfoHead">
            <h1><%=hotelInfo.simple_name%></h1>
        </div>
        <div class="main" style="-webkit-overflow-scrolling: touch;">
            <div class="room item">
                <ul>
                    <li class="clearfix">
                        <a href="">
                            <div class="l" style="width: 25%;">
                                <p class="roomNumber"><span>房间号</span></p>
                            </div>

                            <div class="r status" style="width: 25%;">
                                <p class="roomNumber"><span>状态</span></p>
                            </div>
                            <div class="l" style=" width: 25%;">
                                <p class="roomNumber"><span>离线时长</span></p>
                            </div>
                            <div class="l" style=" width: 25%;">
                                <p class="roomNumber">
                                    <span onclick="sort_amount_click()">销售金额</span>
                                    <asp:ImageButton runat="server" Width="12" Height="12" ID="SortImgBtn" Sort="down"
                                        OnClick="SortImgBtn_OnClick" />
                                </p>
                            </div>
                        </a>
                    </li>
                    <asp:Repeater ID="psy_rp" runat="server">

                        <ItemTemplate>
                            <li class="clearfix">
                                <a href="#">
                                    <div class="l" style="width: 25%;">
                                        <p class="roomNumber"><span><%#Eval("room").ObjToStr() %></span></p>


                                    </div>
                                    <div class="l" style=" width: 25%;">
                                        <p class="roomNumber"><%#Eval("offline").ObjToStr() %></p>
                                    </div>

                                    <div class="r status" style="width: 25%;">
                                        <%#(bool)Eval("online")?"<span class=\"online\">在线</span>":"<span class=\"offline\">离线</span>" %>
                                    </div>
                                    <div class="l" style="width: 25%;">
                                        <p class="roomNumber">¥<span><%#Eval("salesAmount").ObjToInt(0).CentToRMB(0) %></span></p>
                                    </div>
                                </a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div class="clearfix fixBottom">
                    <p class="r">活跃房间数量合计： <span runat="server" id="room_count"></span></p>
                </div>
            </div>
    </form>
</body>
</html>

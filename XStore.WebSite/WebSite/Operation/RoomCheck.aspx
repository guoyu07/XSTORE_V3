<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoomCheck.aspx.cs" Inherits="XStore.WebSite.WebSite.Operation.RoomCheck" %>
<%@ Register Src="~/WebSite/_Ascx/CenterFooter.ascx" TagPrefix="uc" TagName="Footer" %>
<%@ Import Namespace="XStore.Entity" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no"/>
     <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/roomcheck/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/finishpickup/js")%>
</head>
<body>
    <form id="form1" runat="server">
        <div id='view' style="-webkit-overflow-scrolling: touch; overflow: auto!important;">
            <div id="roomStatus">
                <div class="clearfix topTitle">
                    <img src="/Content/Images/delivery.png" alt="" class="l" />
                    <div class="disInfo l">
                        <h3 class="over">
                            <asp:Label runat="server"><%=hotelInfo.simple_name %></asp:Label></h3>
                        <p>
                            <asp:Label runat="server"><%=hotelInfo.address %></asp:Label></p>
                    </div>
                </div>
                <div class="interval"></div>
                <ul class="clearfix">
                    <asp:Repeater runat="server" ID="roomStateRepaeater">
                        <ItemTemplate>
                            <li class="<%# GetRoomStatus(Eval("mac").ObjToStr()).css%>">
                                <a href="<%# GetRoomStatus(Eval("mac").ObjToStr()).href%>">
                                    <img src="/Content/Images/room.png" />
                                    <p class="roomNumber"><%#Eval("room") %></p>
                                    <p class="label offLine">离</p>
                                    <%# GetRoomStatus(Eval("mac").ObjToStr()).icon%>

                                </a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
       
        <div class="footer_bar openwebview">
            <uc:Footer ID="UserFooter" runat="server" EnableViewState="False"></uc:Footer>
        </div>

    </form>
</body>
</html>

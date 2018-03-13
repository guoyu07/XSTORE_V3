<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseInfo.aspx.cs" Inherits="XStore.WebSite.WebSite.Information.BaseInfo" %>
<%@ Register Src="~/WebSite/_Ascx/CenterFooter.ascx" TagPrefix="uc" TagName="Footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no"/>
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/baseinfo/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
</head>
<body>
    <form id="form2" runat="server">
        <div class="main" style="-webkit-overflow-scrolling: touch;">
            <div class="room item">
                <ul>
                    <li class="clearfix">
                        <a href='RoomInfo.aspx'>
                            <div class="l">
                                <p class="roomNumber"><span>房间</span></p>
                            </div>
                            <div class="r iconfont icon-gengduo"></div>
                        </a>
                    </li>
                    <li class="clearfix">
                        <a href='GoodsInfo.aspx'>
                            <div class="l">
                                <p class="roomNumber"><span>商品</span></p>
                            </div>
                             <div class="r iconfont icon-gengduo"></div>
                        </a>
                    </li>
<%--                    <li class="clearfix" style="display:none;">
                        <a href='UserInfoList.aspx?hotel_id=<%#hotelInfo.id %>'>
                            <div class="l">
                                <p class="roomNumber"><span>注册用户</span></p>
                            </div>
                             <div class="r iconfont icon-gengduo"></div>
                        </a>
                    </li>--%>
                </ul>
            </div>
        </div>
         <div class="footer_bar openwebview">
               <uc:Footer ID="UserFooter" runat="server" EnableViewState="False"></uc:Footer>
        </div>
    </form>
</body>
</html>

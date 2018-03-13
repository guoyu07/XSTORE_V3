<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WaterFillUp.aspx.cs" Inherits="XStore.WebSite.WebSite.Operation.WaterFillUp" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="UTF-8" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/paycenter/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
</head>
<body>
    <form id="form" runat="server">
        <div class="total_money pd3 clearfix" style="margin-top:10px;">
            <div class="l"  style=" font-size:xx-large;">酒店名称：
                <br />
                <label style=" font-size:xx-large;"><%=hotelInfo.simple_name.ObjToStr() %></label>
            </div>
        </div>
        <div class="total_money pd3 clearfix" style="margin-top:40px; font-size:xx-large;">
            <div class="l"  style=" font-size:xx-large;">房间号：<br /> 
                 <label style=" font-size:xx-large;"><%=cabinet.room.ObjToStr() %></label>
            </div>
        </div>
        <div class="submit pd3" style="margin-top:100px;">
            <a class="submitBtn" id="water_fillup" style="height: 84px; font-weight:600; font-size: 50px;line-height: 72px;" runat="server" onserverclick="water_fillup_ServerClick" >完成配货</a>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoPower.aspx.cs" Inherits="XStore.WebSite.WebSite.Login.NoPower" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />
    <meta name="format-detection" content="telephone=yes" />
    <title><%=Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/nopower")%>
</head>
<body>
    <form id="form2" runat="server">
        <div class="imgWrap" style="margin:100px 0px 15px;">
            
            <img style="width: 55%; height: 55%; display:none;" src="/Content/Images/no-power.png" alt="" />
        </div>
        <div class="des" style="margin:15px;">
             <h1 style="font-weight:800;margin-bottom:45px;">通讯正在努力连接中...</h1>
            <h3 style="display:none;">您可以使用“即买即用”模式</h3>
            <p >请检查：</p>
            <p>1.售货机电源是否连接良好</p>
            <p>2.房卡是否插上</p>
            <p style="margin-bottom:10px;">3.请尝试拔下插头，再上一次电</p>
            <p style="font-weight:800; text-align:left;">如刚上电，请等售货机提示灯停止</p>
            <p style="font-weight:800; text-align:left;">闪烁一分钟之后，再次扫码</p>
            <h3 style="display:none;">您也可以使用“前台送货”模式</h3>
        </div>
    </form>
</body>

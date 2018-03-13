<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinishPickUp.aspx.cs" Inherits="XStore.WebSite.WebSite.Operation.FinishPickUp" %>

<%@ Register Src="~/WebSite/_Ascx/CenterFooter.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no"/>
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
        <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/finishpickup/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/finishpickup/js")%>
</head>
<body>
    <form id="form1" runat="server">
       
        <div runat="server" id="list_div" class="finishPickUp">
            <ul class="clearfix">
                <asp:Repeater ID="rooms_rp" runat="server">
                    <ItemTemplate>
                        <li class="offLine">
                            <a href="#">
                                <img class="roomclass" src="/Content/Images/<%#(bool)Eval("online")?"room.png":"room_off.png" %>" />
                                 <p class="roomNumber"><%#Eval("room") %></p>
                            </a>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
         <div  class="alarm_div">
             然后到如下任何一个房间启动补货。
            补货时，售货机需先上线【上电后，LED灯闪烁停止】，采用微信对售货机顶部二维码扫码，点击“立即进入ENTER”，需要补货的格子门将自动打开。
为方便后续补货操作，您需要点击“确认”后，点击手机左上角的“X”，退到微信主页面。
        </div>
        <div class="noRoom" runat="server" style="text-align:center;padding-top:40%;" id="empty_div">
            <p>暂无配送任务</p>
        </div>
        <div class="footer_bar openwebview">
               <uc:Footer ID="UserFooter" runat="server" EnableViewState="False"></uc:Footer>
        </div>
        <script type="text/javascript">
            $(function () {
                $("#foot li").removeClass("clickOn");
                $("#foot li").eq(0).addClass("clickOn");
            })
        </script>
    </form>
</body>
</html>

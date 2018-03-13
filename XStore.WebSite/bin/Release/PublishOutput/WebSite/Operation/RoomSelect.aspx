<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoomSelect.aspx.cs" Inherits="XStore.WebSite.WebSite.Operation.RoomSelect" %>
<%@ Register Src="~/WebSite/_Ascx/CenterFooter.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" />
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
        <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/roomselect/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/roomselect/js")%>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#foot li").removeClass("clickOn");
            $("#foot li").eq(0).addClass("clickOn");
        })
        function select_click(sender) {
            var obj = $(sender);
            var cls = obj.find(".label").find("img");
            if (obj.attr("class") == "active") {
                obj.removeClass("active");
                cls.attr("src", "/Content/Images/unchecked.png");
            }
            else {
                obj.addClass("active");
                cls.attr("src", "/Content/Images/checked.png");
            }
        }
        function makeSureClick() {
            var totalId="";
            $(".active").each(function () {
                var self = $(this);
                totalId += self.attr("data-id") + ",";
            });
            if (totalId == "") {
                system_alert("请先选择房间");
                return;
            }
            window.location.href = "PickUp.aspx?totalId=" + totalId;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id='view' style="-webkit-overflow-scrolling: touch; overflow: auto!important;">
        <div id="roomSelectDiv" runat="server">
        <section class="clearfix process">
            <dl class="publishProgress clearfix">
               
                <dd class="step01 l pass">
                    <h3 class="icon iconfont icon-fangjian"></h3>
                    <h4>房间选择</h4>
                </dd>
                <dt class="progressBar01 l">
                    <div class="progressCenter"></div>
                </dt>
                <dd class="step02 l">
                    <h3 class="icon iconfont icon-peisongzhong"></h3>
                    <h4>仓库取货</h4>
                </dd>
            </dl>
        </section>
        <div class="roomSelectUl">
            <ul class="clearfix">
                <asp:Repeater ID="rooms_rp" runat="server">
                    <ItemTemplate>
                        <li>
                            <a href="#" data-id='<%#Eval("mac")%>' onclick="select_click(this);">
                                <img class="roomclass" src="/Content/Images/<%#(bool)Eval("online")?"room.png":"room_off.png" %>" />
                                <p class="roomNumber"><%#Eval("room") %></p>
                                <p class="label" ><img  class="" src="/Content/Images/unchecked.png"/></p>
                            </a>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div class="alarm_div">
           请从如上需要补货的房间中，选择可补货的房间。确认之后，您将看到需要从仓库取走的商品。
        </div>
        <div class="btnWrap" style="margin-top: 10px;">
		    <a class="makeSure"href="#" onclick="makeSureClick();">确认</a>
	    </div>
        </div>
        <div class="noRoom" runat="server" style="text-align:center;padding-top:40%;" id="empty_div">
            <p>暂无配送房间</p>
        </div>
            </div>
         <div class="footer_bar openwebview">
               <uc:Footer ID="UserFooter" runat="server" EnableViewState="False"></uc:Footer>
        </div>
    </form>
</body>
</html>

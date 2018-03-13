<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="XStore.WebSite.WebSite.Login.Welcome" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="UTF-8"  content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />
    <title><%=Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/welcome")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
</head>
<body>
    <form runat="server">
    <div class="top">
         <asp:Image runat="server" ImageUrl="~/Content/Images/top.png"/>
    </div>
    <div class="main">
        <div class="btnWrap mb10">
            <input class="old_check" type="checkbox" />
            <label for="">确认已满18周岁</label>
        </div>
        <div class="btnWrap">
            <div class="btn">立即开启 ENTER</div>
            <input type="button" id="submit_button" class="submit_button" style="display:none;" runat="server" onserverclick="submit_button_ServerClick"/>
        </div>
    </div>
    <div class="bottom">
        <asp:Image runat="server" ImageUrl="~/Content/Images/bottom.png"/>
    </div>
    <script>

        $('.okBtn').on('click', function () {
            $('.tips').hide();
        });
        $('.tips').on('click', function () {
            $('.tips').hide();
        });
        $('.enterFail').on('click', function (event) {
            event.stopPropagation();
        });
        $('.btn').on('click', function () {
            if ($('.old_check').prop('checked')) {
                $(".submit_button").click();
            } else {
                system_alert("请确认您已满18周岁");
            }
        });
       
    </script>
        </form>
</body>

</html>

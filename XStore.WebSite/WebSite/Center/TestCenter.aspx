<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestCenter.aspx.cs" Inherits="XStore.WebSite.WebSite.Center.TestCenter" %>
<%@ Import Namespace="XStore.Entity" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no"/>
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/employeecenter/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>

    <style>
        #form1 {
            width: 100%;
            height: 100%;
            font-family: 'Microsoft YaHei';
        }

        .get_div {
            text-align: center;
            margin: 40px;
            border: 1px solid #999;
            border-radius: 3px;
            color: #999;
            padding: 10px;
            background-color: #eee;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id='view' style="-webkit-overflow-scrolling: touch; overflow: auto!important;">
            <div class="get_div">
                测试员，您好！
您可以对所有未配置酒店、房间的售货机执行开箱操作。
请点击本页面顶部的“X”号，关闭此登录界面。然后，微信扫描，智能售货机上二维码。
            </div>
            <div class="roomDetail">
                <div class="btnWrap " style="background-color: #ffffff;">
                    <a href="#" class="makeSure cancellation" >注销账号</a>

                </div>
            </div>
        </div>

    </form>
    <script type="text/javascript">
        $(function () {
            $('.cancellation').on('click', function () {
                layer.open({
                    content: '是否注销？',
                    btn: ['确定', '取消'],
                    yes: function (index) {
                        $.ajax({
                            url: '<%=Constant.ApiDic%>' + 'UnBindAccount.ashx',
                                type: 'GET',
                                dataType: 'JSON',
                                success: function (response) {
                                    if (response.success) {
                                        system_alert(response.message)
                                        window.location.href = '<%=Constant.JsLoginDic%>' + "Login.aspx";
                                    } else {
                                        system_alert(response.message);
                                    }
                                }
                            })
                        }
                    })
                })
        })
        </script>

</body>
</html>

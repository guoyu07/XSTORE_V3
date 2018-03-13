<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayWaiting.aspx.cs" Inherits="XStore.WebSite.WebSite.Order.PayWaiting" %>
<%@ Import Namespace="XStore.Entity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%:Title %></title>
    <meta name="viewport" charset="UTF-8" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/paysuccess/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/weui/js")%>
    <script type="text/javascript" src="/Scripts/jquery.myProgress.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#progress_bar").myProgress({ speed: 30000, percent: 100 });
            setTimeout('delayer()', 3000);
            setTimeout('fail_check()', 30000);
        });
        function delayer() {
            $.ajax({
                url: '<%=Constant.ApiDic%>'+'CheckOrderState.ashx',
                type: 'GET',
                success: function (response) {
                    var jsonData = $.parseJSON(response);
                    if (jsonData.success) {
                        if (jsonData.pay && jsonData.deliver) {
                            window.location.href = "PaySuccess.aspx";
                        }
                    }
                    else {
                        system_alert(jsonData.message);
                    }
                }
            });
        }
        function fail_check() {
            $.ajax({
                url: '<%=Constant.ApiDic%>' + 'CheckOrderState.ashx',
                  type: 'GET',
                  success: function (response) {
                      var jsonData = $.parseJSON(response);
                      if (jsonData.success) {
                          if (jsonData.pay && jsonData.deliver) {
                              window.location.href = "PaySuccess.aspx";
                          }
                          else {
                              window.location.href = "PayFail.aspx";
                          }
                      }
                      else {
                          system_alert(jsonData.message);
                      }
                  }
             });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="banner clearfix">
            <div class="l">
                <span class="suc">正在为您开箱</span>

            </div>
            <asp:Image ID="title_img" runat="server" CssClass="r"/>
        </div>

        <div class="total_money">
          
            合计：<i>¥ <span>  <%=orderInfo.price1.CentToRMB(0) %></span></i>
        </div>
        <div class="middle">
                <div class="jdt">
                    <h4 style="text-align: center; font-family: '微软雅黑'; color: #ff6600; margin: 20px;">开心一刻</h4>
                    <div style="margin: 15px; width: 90%; overflow-y: auto; height: 300px;">
                        <asp:Image ID="joker_img" runat="server" />
                    </div>

                    <div class="progress-out" id="progress_bar">
                        <div class="percent-show"><span>0</span>%</div>
                        <div class="progress-in"></div>
                    </div>
                    <p>努力开箱中...</p>
                </div>
                <div class="restart">
                    <a class="tel" href="tel:400-880-2482">
                        <div class="telWrap">
                            <div class="telContent clearfix">
                                <span class="l">如开箱失败，请拨打客服电话</span>
                                <br />
                                 <asp:Image ID="tel_img" runat="server"/>
                                <span class="l"><em>400-880-2482</em></span>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayCenter.aspx.cs" Inherits="XStore.WebSite.WebSite.Order.PayCenter" %>
<%@ Import Namespace="XStore.Entity" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="UTF-8" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />
    <title><%=Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/paycenter/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/weui/js","~/bundles/paycenter/js")%>
    <script type="text/javascript" >
        $(function () {
            window.addEventListener("popstate", function (e) {
                window.location.href = '<%= Constant.GoodsDic+"GoodsList.aspx"%>';
            }, false);
       
            //setInterval('checkOrder()',2000);
        });
        function checkOrder() {
            var requesturl = '<%= Constant.ApiDic+"CheckOrderState.ashx"%>';
            $.ajax({
                type: "GET",
                url: requesturl,
                success: function (response) {
                    var jsonData = $.parseJSON(response);
                    if (jsonData.success) {
                        if (jsonData.pay) {
                            window.location.href = 'PayWaiting.aspx';
                        }
                    } 
                    else {
                        console.log("返回不正确");
                        system_alert(jsonData.message);
                    }
                }
            });
        }
        //调用微信JS api支付
        function jsApiCall() {
            WeixinJSBridge.invoke(
            'getBrandWCPayRequest',
            <%=wxJsApiParam%>,//josn串
            function (res) {
               
            });
        }
        function callpay() {
            if ($("#zfbPay").is(":checked")) {
                window.location.href = '/WebSite/Order/Alipay.html?order=' + '<%=orderInfo.code %>' + '&money=' + '<%=orderInfo.price1.ObjToInt(0).CentToRMB(0) %>';
            }
            else {
                if (typeof WeixinJSBridge == "undefined") {
                    if (document.addEventListener) {
                        document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
                    }
                    else if (document.attachEvent) {
                        document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                        document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
                    }
                }
                else {
                    jsApiCall();
                }
            }
           
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- 订单金额 -->
        <ul class="clearfix">
            <asp:Repeater ID="car_rp" runat="server">
                <ItemTemplate>
                    <li>
                        <div class="pic">
                            <img src="<%#GetProductImg(Eval("id").ObjToInt(0),Eval("image").ObjToStr()) %>" alt="" />
                            <p class="goodsName over"><%#Eval("name") %></p>
                        </div>
                        <div class="price">¥ <span><%#Eval("price1").ObjToInt(0).CentToRMB(0) %></span></div>

                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
        <div class="interval"></div>
        <div class="total_money pd3 clearfix">
            <div class="l">合计：</div>
            <div class="r">¥ <%=orderInfo.price1.CentToRMB(0) %></div>
        </div>
        <div class="interval"></div>
        <div class="payWay clearfix">
            <div class="wxPay pd3 clearfix">
                <input type="radio" name="payWay" checked="checked" class="l" id="wxPay" />
                <p class="l">微信支付</p>
            </div>
            <div class="zfbPay pd3 clearfix">
                <input type="radio" name="payWay" class="l" id="zfbPay" />
                <p class="l">支付宝支付</p>
            </div>
        </div>
        <div class="interval"></div>
        <div class="submit pd3">
            <a class="submitBtn" id="wx_submit_order" onclick="callpay()">确认支付</a>
        </div>
        <div class="remind">
            <p>可以选择微信或支付宝支付。选择好支付方式后，点击确认支付，付款。付完款后售货箱会自动打开，您在规定时间内取货即可</p>
        </div>
    </form>

</body>
</html>

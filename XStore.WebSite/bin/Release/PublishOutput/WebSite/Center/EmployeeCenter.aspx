<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeCenter.aspx.cs" Inherits="XStore.WebSite.WebSite.Center.EmployeeCenter" %>
<%@ Register Src="~/WebSite/_Ascx/CenterFooter.ascx" TagPrefix="uc" TagName="Footer" %>
<%@ Import Namespace="XStore.Entity" %>
<html>
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no">
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/employeecenter/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/employeecenter/js")%>
</head>
<body>
    <form runat="server" id="form1">
        <div id='view' style="-webkit-overflow-scrolling: touch; overflow: auto!important;">
            <div id="disMyself">
                <div class="clearfix personalInfo">
                     <img class="headPortrait l" src="<%=wxUserInfo.headpic.ObjToStr() %>""/>
                    <div class="positinInfo r clearfix">
                        <div class="market l">
                            <p class="name"><%=userInfo.realname %></p>
                            <p class="job">酒店服务员</p>
                            <p class="account"><%=userInfo.phone %></p>
                        </div>
                        <p class="area l over2"><%=hotelInfo.hotel_name %></p>
                    </div>
                </div>
                <div class="interval"></div>
                <ul>
                    <li class="deliveryTask">
                        <a href='<%=Constant.JsOperationDic+"RoomFillUp.aspx"%>' class="clearfix">
                            <div class="l">
                                <i class="iconfont icon-renwu"></i>
                                <span>常规补货</span>
                            </div>
                            <div class="r iconfont icon-gengduo"></div>
                        </a>
                    </li>
                    <li class="deliveryNote">
                        <a href='<%=Constant.JsOperationDic+"BackLog.aspx"%>' class="clearfix">
                            <div class="l">
                                <i class="iconfont icon-jilu"></i>
                                <span>投放记录</span>
                            </div>
                            <div class="r iconfont icon-gengduo"></div>
                        </a>
                    </li>
                    <div class="interval"></div>
                    <li class="changePsd">
                        <a href='<%=Constant.JsLoginDic+"ChangePassword.aspx"%>' class="clearfix">
                            <div class="l">
                                <i class="iconfont icon-xiugaimima"></i>
                                <span>修改密码</span>
                            </div>
                            <div class="r iconfont icon-gengduo"></div>
                        </a>
                    </li>
                    <li class="cancellation">
                        <a class="clearfix">
                            <div class="l">
                                <i class="iconfont icon-zhuxiao"></i>
                                <span>用户注销</span>
                            </div>
                            <div class="r iconfont icon-gengduo"></div>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
 
        <div class="footer_bar openwebview">
               <uc:Footer ID="UserFooter" runat="server" EnableViewState="False"></uc:Footer>
        </div>
        <script type="text/javascript">
            $(function () {
                $("#foot li").removeClass("clickOn");
                $("#foot li").eq(1).addClass("clickOn");
                $('.cancellation').on('click', function () {
                    layer.open({
                        content: '是否注销？',
                        btn: ['确定', '取消'],
                        yes: function (index) {
                            $.ajax({
                                url: '<%=Constant.ApiDic%>'+'UnBindAccount.ashx',
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
    </form>
</body>
</html>

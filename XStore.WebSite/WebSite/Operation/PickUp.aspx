<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PickUp.aspx.cs" Inherits="XStore.WebSite.WebSite.Operation.PickUp" %>

<%@ Register Src="~/WebSite/_Ascx/CenterFooter.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" />
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
        <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/pickup/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/pickup/js")%>
</head>
<body>
    <form id="form1" runat="server">
        <div id='view' style="-webkit-overflow-scrolling: touch; overflow: auto!important;">
            <div id="pickUp" runat="server">
                <section class="clearfix process">
                    <dl class="publishProgress clearfix">
                        <dd class="step01 l pass">
                            <h3 class="icon iconfont icon-fangjian"></h3>
                            <h4>房间选择</h4>
                        </dd>
                        <dt class="progressBar01 l pass">
                            <div class="progressCenter"></div>
                        </dt>
                        <dd class="step02 l pass">
                            <h3 class="icon iconfont icon-peisongzhong"></h3>
                            <h4>仓库取货</h4>
                        </dd>
                    </dl>
                </section>
                <div class="interval"></div>
                <ul>
                    <asp:Repeater ID="Rp_pickup" runat="server">
                        <ItemTemplate>
                            <li class="clearfix">
                                <div class="imgWrap l">
                                    <img src="<%#GetProductImg(Eval("id").ObjToInt(0),Eval("image").ObjToStr()) %>" alt="" />
                                </div>
                                <div class="info l">
                                    <h3><span><%#Eval("code") %></span> &nbsp;<span><%#Eval("name") %></span></h3>
                                </div>
                                <div class="num r iconfont icon-cha"><span><%#Eval("count") %></span></div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div class="alarm_div">
                    请到仓库取货。
                </div>
                <div class="btnWrap">
                    <a class="makeSure" href="#" runat="server" id="markSure" onserverclick="markSure_OnServerClick">确认</a>
                </div>
            </div>
            <div class="noTask" runat="server" style="text-align: center; padding-top: 40%;" id="noTask">
                暂无配送任务
            </div>
        </div>

        <div style="display: block;" runat="server" id="foot_div" class="footer_bar openwebview">
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

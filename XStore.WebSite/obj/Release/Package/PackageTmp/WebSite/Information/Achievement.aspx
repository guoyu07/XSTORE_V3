<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Achievement.aspx.cs" Inherits="XStore.WebSite.WebSite.Information.Achievement" %>

<%@ Register Src="~/WebSite/_Ascx/CenterFooter.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="UTF-8" content="width=device-width,initial-scale=1,minimum-scale=1,maximum-scale=1,user-scalable=no" />
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/achievement/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/achievement/js")%>
    <script>
        function sort_amount_click() {
            $("#SortImgBtn").click();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="AchievementBox">
            <div id="sliderAchievementControl" class="mui-slider-indicator mui-segmented-control mui-segmented-control-inverted ">
                <a class="mui-control-item mui-active" id="yestoday" href="#content1">昨天</a>
                <a class="mui-control-item" id="serverDays" href="#content2">7天</a>
                <a class="mui-control-item" id="lastMonth" href="#content3">上月</a>
                <a class="mui-control-item" id="thisMonth" href="#content4">本月</a>
                <a class="mui-control-item" id="thisYear" href="#content5">本年</a>
                <a class="mui-control-item" id="All" href="#content6">全部</a>
            </div>
        </div>
        <div id="sliderAchievementControlContents" class="sliderAchievementControlContents">
            <div id="content1" class="mui-control-content mui-active">
                 <div class="bootomBar">
                     <p class="daysale">日均房：<span>¥<%=yestodyDaySales %></span></p>
                    <p class="total">总计：<span>¥<%=yestodyTotal %></span></p>
                </div>
                <table border="" cellspacing="" cellpadding="" class="Am_table">
                    <tr>
                        <th>商品</th>
                        <th>编码</th>
                        <th>数量</th>
                        <th>金额</th>
                    </tr>
                    <asp:Repeater runat="server" ID="Yestoday">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("name") %></td>
                                <td><%#Eval("code") %></td>
                                <td>× <span><%#Eval("salesCount") %></span></td>
                                <td>¥ <span><%#Eval("totalAmount").ObjToInt(0).CentToRMB(0) %></span></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </table>
               
            </div>
            <div id="content2" class="mui-control-content">
                 <div class="bootomBar">
                    <p class="daysale">日均房：<span>¥<%=serverDaySales %></span></p>
                    <p class="total">总计：<span>¥<%=serverTotal %></span></p>
                </div>
                <table border="" cellspacing="" cellpadding="" class="Am_table">
                    <tr>
                        <th>商品</th>
                           <th>编码</th>
                        <th>数量</th>
                        <th>金额</th>
                    </tr>
                    <asp:Repeater ID="ServerDays" runat="server">
                        <ItemTemplate>
                           <tr>
                                <td><%#Eval("name") %></td>
                                <td><%#Eval("code") %></td>
                                <td>× <span><%#Eval("salesCount") %></span></td>
                                <td>¥ <span><%#Eval("totalAmount").ObjToInt(0).CentToRMB(0) %></span></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </table>
               
            </div>
            <div id="content3" class="mui-control-content">
                 <div class="bootomBar">
                      <p class="daysale">日均房：<span>¥<%=lastMonthDaySales %></span></p>
                    
                    <p class="total">总计：<span>¥<%=lastMonthTotal %></span></p>
                </div>
                <table border="" cellspacing="" cellpadding="" class="Am_table">
                    <tr>
                        <th>商品</th>
                           <th>编码</th>
                        <th>数量</th>
                        <th>金额</th>
                    </tr>
                    <asp:Repeater ID="LastMonth" runat="server">
                        <ItemTemplate>
                             <tr>
                                <td><%#Eval("name") %></td>
                                <td><%#Eval("code") %></td>
                                <td>× <span><%#Eval("salesCount") %></span></td>
                                <td>¥ <span><%#Eval("totalAmount").ObjToInt(0).CentToRMB(0) %></span></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </table>
               
            </div>
            <div id="content4" class="mui-control-content">
                  <div class="bootomBar">
                      <p class="daysale">日均房：<span>¥<%=thisMonthDaySales %></span></p>
                    <p class="total">总计：<span>¥<%=thisMonthTotal %></span></p>
                </div>
                <table border="" cellspacing="" cellpadding="" class="Am_table">
                    <tr>
                        <th>商品</th>
                              <th>编码</th>
                        <th>数量</th>
                        <th>金额</th>
                    </tr>
                    <asp:Repeater runat="server" ID="ThisMonth">
                        <ItemTemplate>
                             <tr>
                                <td><%#Eval("name") %></td>
                                <td><%#Eval("code") %></td>
                                <td>× <span><%#Eval("salesCount") %></span></td>
                                <td>¥ <span><%#Eval("totalAmount").ObjToInt(0).CentToRMB(0) %></span></td>
                            </tr>

                        </ItemTemplate>
                    </asp:Repeater>

                </table>
              
            </div>
            <div id="content5" class="mui-control-content">
                <div class="bootomBar">
                    <p class="daysale">日均房：<span>¥<%=thisYearDaySales %></span></p>
                    <p class="total">总计：<span>¥<%=thisYearTotal %></span></p>
                </div>
                <table border="" cellspacing="" cellpadding="" class="Am_table">
                    <tr>
                        <th>商品</th>
                              <th>编码</th>

                        <th>数量</th>
                        <th>金额</th>
                    </tr>
                    <asp:Repeater ID="ThisYear" runat="server">
                        <ItemTemplate>
                             <tr>
                                <td><%#Eval("name") %></td>
                                <td><%#Eval("code") %></td>
                                <td>× <span><%#Eval("salesCount") %></span></td>
                                <td>¥ <span><%#Eval("totalAmount").ObjToInt(0).CentToRMB(0) %></span></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </table>
                
            </div>
            <div id="content6" class="mui-control-content">
                <div class="bootomBar" style="bottom:30px;">
                     <p class="daysale">日均房：<span>¥<%=allDaySales %></span></p>
                    <p class="total">总计：<span>¥<%=allTotal %></span></p>
                </div>
                <div>
                    <table border="" cellspacing="" cellpadding="" class="Am_table" >
                        <tr>
                            <th>商品</th>
                                  <th>编码</th>

                            <th>数量</th>
                            <th>金额</th>
                        </tr>
                        <asp:Repeater runat="server" ID="AllGoods">
                            <ItemTemplate>
                                <tr>
                                <td><%#Eval("name") %></td>
                                <td><%#Eval("code") %></td>
                                <td>× <span><%#Eval("salesCount") %></span></td>
                                <td>¥ <span><%#Eval("totalAmount").ObjToInt(0).CentToRMB(0) %></span></td>
                            </tr>

                            </ItemTemplate>
                        </asp:Repeater>

                    </table>
                </div>
                <br />
                <br />
                <br />
                
            </div>
        </div>
        <div class="footer_bar openwebview">
            <uc:Footer ID="UserFooter" runat="server" EnableViewState="False"></uc:Footer>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Settlement.aspx.cs" Inherits="XStore.WebSite.WebSite.Information.Settlement" %>
<%@ Register Src="~/WebSite/_Ascx/CenterFooter.ascx" TagPrefix="uc" TagName="Footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no">
       <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/settlement/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/settlement/js")%>
    <link href="/Content/mui.picker.min.css" rel="stylesheet" />
    <script src="/Scripts/mui.picker.min.js"></script>
</head>
<body>
    <form id="form1" runat="server" style="height:100%;">
        <div style="text-align: center; margin: auto; margin-top: 5px; ">  <h3 ><%=hotelInfo.simple_name%></h3></div>
      
        <table style="width:100%; margin-top:15px; border-bottom: 1px solid #ccc; border-top: 1px solid #cccccc;">
            <tr>
                <td align="center">
                    <div class="startDate " style="padding: 5px;">
                        <label runat="server" id="start_date_label">-开始时间-</label>
                    </div>
                </td>
                <td align="center">
                    <div class="endDate " style="padding: 5px;">
                        <label  runat="server" id="end_date_label">-结束时间-</label>
                    </div>
                </td>
            </tr>
        </table>
        <div class="main" style="-webkit-overflow-scrolling: touch;">
   
            <div class="box">
                <div class="interval"></div>
                <dl class="total_money clearfix">
                    <dt class="l">合计</dt>
                    
                    <dd class="r">金额: ¥ <span><%=totalAmount %></span></dd>
                    <dd class="r">数量: × <span><%=totalSum %></span></dd>
                </dl>
                <table border="1">
                    <tr class="topic">
                        <th>商品</th>
                        <th>编码</th>
    
                        <th>数量</th>
                        <th>金额</th>
                    </tr>
                    <asp:Repeater ID="notlement" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("name") %></td>
                                <td><%#Eval("code") %></td>
                                <td>× <span><%#Eval("salesCount") %></span></td>
                                <td>¥ <%#Eval("totalAmount").ObjToInt(0).CentToRMB(0) %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
        <div>
            <input type="hidden" id="data_type_input" class="data_type_input" runat="server"/>
        </div>
        <div class="footer_bar openwebview">
            <uc:Footer ID="UserFooter" runat="server" EnableViewState="False"></uc:Footer>
        </div>
    </form>

        <script type="text/javascript">
            $(function () {
                $('.topic li').on('click', function () {
                    $(this).addClass('clickOn').siblings().removeClass('clickOn');
                    var index = $(this).index();
                    $('.box').eq(index).show().siblings().hide();
                });
                var data_type = $(".data_type_input").val();

                $('.topic li').eq(data_type).click();

                $(".startDate").on('click', function () {
                    var dtpicker = new mui.DtPicker({
                        type: "date",//设置日历初始视图模式 
                        beginDate: new Date(1990, 01, 01),//设置开始日期 
                        endDate: new Date(),//设置结束日期 
                        labels: ['Year', 'Mon', 'Day'],//设置默认标签区域提示语 
                    });
                    dtpicker.show(function (e) {
                        $('.startDate label').text(e.value);
                        window.location.href = "settlement.aspx?start_time=" + e.value + "&end_time=" + $('.endDate label').text() + "&data_type=" + $(".clickOn").data("type");
                    });
                });

                $(".endDate").on('click', function () {
                    var dtpicker = new mui.DtPicker({
                        type: "date",//设置日历初始视图模式 
                        beginDate: new Date(1990, 01, 01),//设置开始日期 
                        endDate: new Date(2100, 01, 01),//设置结束日期 
                        labels: ['Year', 'Mon', 'Day'],//设置默认标签区域提示语 
                    });
                    dtpicker.show(function (e) {
                        $('.endDate label').text(e.value);
                        window.location.href = "settlement.aspx?start_time=" + $('.startDate label').text() + "&end_time=" + e.value + "&data_type=" + $(".clickOn").data("type");
                    });
                });

               
            })
		</script>

</body>
</html>

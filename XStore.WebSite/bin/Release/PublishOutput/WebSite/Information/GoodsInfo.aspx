<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoodsInfo.aspx.cs" Inherits="XStore.WebSite.WebSite.Information.GoodsInfo" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
   <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/roominfo/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
</head>
<body>
    <form id="form1" runat="server">
        <input hidden value="0" id="hiddenButton" />
        <div class="roomInfoHead">
            <h1><%=hotelInfo.simple_name %></h1>
        </div>
        <div class="main" style="-webkit-overflow-scrolling: touch;">
            <div class="goods">
                <ul>
                    <li class="clearfix">
                      
                            <div class="l" style="width: 20%;text-align: center;">
                                <p class="roomNumber"><span>图片</span></p>
                            </div>
                            <div class="l" style="margin-left: 1px;text-align: center; width: 25%;">
                                <p class="roomNumber"><span>名称</span></p>
                            </div>
                            <div class="l" style="margin-left: 1px;text-align: center; width: 15%;">
                                <p class="roomNumber"><span>编码</span></p>

                            </div>
                            <div class="l" style="margin-left: 1px;text-align: center; width: 15%;">
                                <p class="roomNumber"><span>单价</span></p>
                            </div>
                            <div class="l" style="margin-left: 1px;text-align: center; width: 20%;">
                                <p class="roomNumber">
                                    <span onclick="sort_amount_click()">累计销量</span>
                                </p>
                            </div>
                  
                    </li>
                    <asp:Repeater ID="goods_rp" runat="server">
                        <ItemTemplate>
                            <li class="clearfix">
                         
                                    <div class="l" style="width: 20%;">
                                        <p class="roomNumber"><span>
                                            <img src='<%#GetProductImg(Eval("id").ObjToInt(0),Eval("image").ObjToStr()) %>' alt="" class="l" /></span></p>
                                    </div>
                                    <div class="l lh" style="margin-left: 1px;  width: 25%;">
                                        <p class="roomNumber"><span><%#Eval("name") %></span></p>
                                    </div>
                                    <div class="l lh" style="margin-left: 1px; text-align: center; width: 15%;">
                                        <p class="roomNumber"><span><%#Eval("code") %></span></p>

                                    </div>
                                    <div class="l lh" style="margin-left: 1px; text-align: center;width: 15%;">
                                        <p class="roomNumber"><span>¥<%#Eval("price1").ObjToInt(0).CentToRMB(0) %></span></p>
                                    </div>
                                    <div class="l lh" style="margin-left: 1px;text-align: center; width: 20%;">
                                        <p class="roomNumber">
                                            <span >¥<%#Eval("salesAmount").ObjToInt(0).CentToRMB(0) %></span>
                                        </p>
                                    </div>
                             
                            </li>

                        </ItemTemplate>
                    </asp:Repeater>

                </ul>
                <div class="clearfix fixBottom">
                    <p class="r">在售商品数量合计： <span runat="server" id="goods_count"></span></p>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BackLog.aspx.cs" Inherits="XStore.WebSite.WebSite.Operation.BackLog" %>
<%@ Register Src="~/WebSite/_Ascx/CenterFooter.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" />
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/backlog/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
</head>
<body>
    <form id="form1" runat="server">
        <header class="mui-bar mui-bar-nav mui-clearfix dealContent1Title">
            <span class="mui-pull-left">投放记录</span>
        </header>
        <div id="backlog" class="mui-content">
            <ul>
                <asp:Repeater ID="backLogRepeater" runat="server">
                    <ItemTemplate>
                        <li class="mui-table-view-cell">
                            <div class="mui-table">
                                 <div class="imgWrap l">
                                        <img  style="margin-right:15px; height:60px;" src="<%#GetProductImg(Eval("id").ObjToInt(0),Eval("image").ObjToStr()) %>" alt="暂无图片" />
                                </div>
                                <div class="mui-table-cell l">
                                    <p class="mui-ellipsis AM_title"><span><%#Eval("name").ObjToStr() %></span> &nbsp;商品编码：<span><%#Eval("code").ObjToStr() %></span></p>
                                    <h5 class="numText">房间：<span><%#Eval("room").ObjToStr() %></span> &nbsp;格号：<span class="danjia"><%#Eval("pos").ObjToInt(0)+1 %></span></h5>
                                    <p class="mui-h6 mui-ellipsis totle"><%#((DateTime)Eval("date")).ToString("yyyy-MM-dd HH:mm") %></span> </p>
                                </div>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div class="footer_bar openwebview">
               <uc:Footer ID="UserFooter" runat="server" EnableViewState="False"></uc:Footer>
        </div>
    </form>
</body>
</html>

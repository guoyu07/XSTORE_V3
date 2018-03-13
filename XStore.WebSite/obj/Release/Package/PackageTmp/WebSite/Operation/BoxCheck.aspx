<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BoxCheck.aspx.cs" Inherits="XStore.WebSite.WebSite.Operation.BoxCheck" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no"/>
  <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/roomgoods/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
</head>
<body>
    <form id="form2" runat="server">
        <div class="clearfix topTitle" style="font-size:18px; font-weight:900;">
            <p class="distributer">MAC：<span><%=cabinet.mac %></span></p>
            <p class="distributer">版本号：<span><%=cabinet.version.ObjToStr() %></span></p>
        </div>
        <div class="interval"></div>
         <div id='view' style="-webkit-overflow-scrolling: touch; overflow: auto!important;">
            <div id="mySpace" class="roomDetail">
                <ul class="clearfix" style="margin-bottom:5px;">
                    <asp:Repeater ID="box_rp" runat="server">
                        <ItemTemplate>
                            <li>
                                <a href="#" runat="server" onserverclick="position_click" num='<%#Eval("num") %>'> <%#Eval("num") %></a>
                               <asp:LinkButton runat="server" ></asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                 <div style="text-align:left; background-color:#ffffff; padding:15px; padding-top:30px; font-size:14px; ">
                               点击如下“全部开箱”，
                             <br />
                            您可打开从1到12个格子的全部格子门。
                            <br />
                                1）开门顺序是否正确？
                             <br />
                                2）全部格子门是否打开？
                             <br />
                                3）您可以多次点击“全部开箱”，反复验证

                           </div>
                  <div class="btnWrap " style="background-color:#ffffff;">
                        

                    <asp:LinkButton runat="server" ID="openAll" CssClass="makeSure" OnClick="openAll_Click">全部开箱</asp:LinkButton>
	            </div> 
            </div>
        </div>
    </form>
</body>

</html>

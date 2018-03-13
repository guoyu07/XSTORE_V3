<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HotelSelect.aspx.cs" Inherits="XStore.WebSite.WebSite.Operation.HotelSelect" %>

<%@ Import Namespace="XStore.Entity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no"/>
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/comprehensive/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
</head>
<body>
    <form id="form2" runat="server">
        <div class="main"  style="-webkit-overflow-scrolling:touch;">
		    <div class="room item">
		        <ul>
                     <asp:Repeater ID="hotel_rp" runat="server">
                        <ItemTemplate>
                             <li class="clearfix">
		    		          <a href='<%# Constant.JsCenterDic+"ManageCenter.aspx?hotelId="+Eval("id").ObjToInt(0) %>'>
			    		            <div class="l">
			    			            <p class="roomNumber"><span><%#Eval("simple_name").ObjToStr() %></span></p>
			    		            </div>
			    		             <div class="r iconfont icon-gengduo"></div>
		    		            </a>
		    	            </li>
                        </ItemTemplate>
                    </asp:Repeater>
		        </ul>
	       </div>
        </div>
    </form>
</body>
</html>

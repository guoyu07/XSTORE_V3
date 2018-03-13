<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bridge.aspx.cs" Inherits="XStore.WebSite.WebSite.Login.Bridge" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta charset="UTF-8" />
    <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />
    <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />
    <title><%=Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
    <script type="text/javascript">
        $(function () {

            $("div").load("http://www.baidu.com");
        })
      
    </script>
  
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>

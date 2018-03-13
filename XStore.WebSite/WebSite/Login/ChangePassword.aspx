<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="XStore.WebSite.WebSite.Login.ChangePassword" %>
<%@ Import Namespace="XStore.Entity" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="UTF-8" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0" />
 <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
    <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/changepassword/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs")%>
</head>

<body>
    <form id="form1" runat="server">
        <div class="name">
			<div class="wrap">
				<label>姓名：</label><input type="text" id="name_input" placeholder="请输入姓名" value='<%=changeUserInfo.realname.ObjToStr() %>'/>
			</div>
		</div>
		<div class="tel">
			<div class="wrap">
				<label>手机号：</label><input type="text" id="phone_input" placeholder="请输入手机号" value="<%=changeUserInfo.phone.ObjToStr() %>"/>
			</div>
		</div>
		<div class="account">
			<div class="wrap"><label for="">账号：</label><input type="text" id="account_input"  placeholder="请输入账号" value="<%=changeUserInfo.username.ObjToStr() %>"/></div>
		</div>
		<div class="newPsd">
			<div class="wrap"><label for="">新密码：</label><input type="password"  placeholder="请输入新密码"/></div>
		</div>
		<div class="newPsdRepeat">
			<div class="wrap"><label for="">新密码：</label><input type="password"  placeholder="请再次输入新密码"/></div>
		</div>
		<div class="btnWrap">
		    <input type="button" class="changePsdBtn" value="修改信息"/>
		</div>
		<script>
		    $(function () {
                var telreg = /^(((13[0-9]{1})|(14[0-9]{1})|(15[0-9]{1})|(17[0-9]{1})|(18[0-9]{1}))+\d{8})$/;
		        var unreg = /^[a-zA-Z0-9_]{6,16}$/;
		        var yzm;
		      
		        $('.newPsd input').on('blur', function () {
		            if ($(this).val().trim().length < 6) {
		                layer.open({
		                    content: '密码至少6位',
		                    skin: 'msg',
		                    time: 2
		                })
		            }
		        });
		        $('.newPsdRepeat input').on('blur', function () {
		            if ($(this).val().trim().length < 6) {
		                layer.open({
		                    content: '密码至少6位',
		                    skin: 'msg',
		                    time: 2
		                });

		            } else if ($(this).val().trim() != $('.newPsd input').val().trim()) {
		                layer.open({
		                    content: '两次密码不一致',
		                    skin: 'msg',
		                    time: 2
		                });
		            } else {
		            }
		        });
		        $('.changePsdBtn').on('click', function () {
		            if ($('.name input').val().trim() == '') {
		                layer.open({
		                    content: '姓名不能为空',
		                    skin: 'msg',
		                    time: 2
		                });
		            } else if ($('.tel input').val().trim() == '') {
		                layer.open({
		                    content: '手机号不能为空',
		                    skin: 'msg',
		                    time: 2
		                });
		            } else if (!telreg.test($('.tel input').val().trim())) {
		                layer.open({
		                    content: '手机号不正确',
		                    skin: 'msg',
		                    time: 2
		                });
		          
		            } else if ($('.account input').val().trim() == "") {
		                layer.open({
		                    content: '请输入账号',
		                    skin: 'msg',
		                    time: 2
		                });
		            } else if ($('.newPsd input').val().trim() == '') {
		                layer.open({
		                    content: '请输入密码',
		                    skin: 'msg',
		                    time: 2
		                });
		            } else if ($('.newPsd input').val().trim().length < 6) {
		                layer.open({
		                    content: '密码至少6位',
		                    skin: 'msg',
		                    time: 2
		                });
		            } else if ($('.newPsdRepeat input').val().trim() != $('.newPsd input').val().trim()) {
		                layer.open({
		                    content: '两次密码不相同',
		                    skin: 'msg',
		                    time: 2
		                });
		            } else {
                        var username = "<%=changeUserInfo.username.ObjToStr()%>";
		                var name = $('.name input').val();
		                var phone = $('.tel input').val();
		                var account = $('.account input').val();
		                var password = $('.newPsd input').val();
		                var url = "<%=Constant.ApiDic%>"+"ChangePassword.ashx";
                        $.post(url, { username: username, name: name, phone: phone, account: account, password: password }, function (response) {
                            if (response.success) {
		                        layer.open({
		                            content: '修改成功',
		                            btn: 'ok',
		                            yes: function (index) {
                                        layer.close(index);
                                        if (username === '<%=userInfo.username%>') {
                                            if ('<%=(userRole.role_id==(int)XStore.Entity.Enum.UserRoleEnum.经理)||(userRole.role_id==(int)XStore.Entity.Enum.UserRoleEnum.区域经理)%>') {
                                                window.location.href = "<%=Constant.JsCenterDic+"ManageCenter.aspx"%>";
                                            }
                                            else if ('<%=userRole.role_id==(int)XStore.Entity.Enum.UserRoleEnum.前台%>') {
                                                window.location.href = "<%=Constant.JsCenterDic+"EmployeeCenter.aspx"%>";
                                            }
                                           
                                        }
                                        else {
                                            window.location.href = "<%=Constant.JsInformationDic+"Employee.aspx"%>";
                                        }
                                       
		                            }
		                        });
		                    }
                            else {
                                system_alert(response.message);
		                    }

		                }, "json");

		            }
		        });
		    })
		</script>
    </form>
</body>
</html>


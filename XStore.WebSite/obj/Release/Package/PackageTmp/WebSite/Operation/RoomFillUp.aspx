<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoomFillUp.aspx.cs" Inherits="XStore.WebSite.WebSite.Operation.RoomFillUp" %>
<%@ Register Src="~/WebSite/_Ascx/CenterFooter.ascx" TagPrefix="uc" TagName="Footer" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" charset="utf-8" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" />
    <title><%:Title %></title>
    <link rel="icon" href="/Content/Icon/logo.png" type="image/x-icon" />
        <link href="/Content/fonts/iconfont.css" rel="stylesheet" />
    <%: System.Web.Optimization.Styles.Render("~/bundles/CommonStyle","~/bundles/roomfillup/css")%>
    <%: System.Web.Optimization.Scripts.Render("~/bundles/CommonJs","~/bundles/roomselect/js")%>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#foot li").removeClass("clickOn");
            $("#foot li").eq(0).addClass("clickOn");
            if ($("#room_id").val() !== "") {
                var roomList = $("#room_id").val().split(',');
                for (var i = 0; i < roomList.length; i++) {
                    var room = roomList[i];
                    $(".roomSelectUl li").find("a").each(function () {
                        if ($(this).attr("data-id") == room) {
                            var cls = $(this).find(".label").find("img");
                            $(this).addClass("active");
                            cls.attr("src", "/Content/Images/checked.png");
                        }

                    });
                }
                
            }
            
           
        })
        function select_click(sender) {
            var obj = $(sender);
            var cls = obj.find(".label").find("img");
            if (obj.attr("class") == "active") {
                obj.removeClass("active");
                cls.attr("src", "/Content/Images/unchecked.png");
            }
            else {
                obj.addClass("active");
                cls.attr("src", "/Content/Images/checked.png");
            }
        }
        function makeSureClick() {
            var totalId="";
            $(".active").each(function () {
                var self = $(this);
                totalId += self.attr("data-id") + ",";
            });
            if (totalId == "") {
                system_alert("请先选择房间");
                return;
            }
            console.debug(totalId);
            $(".room_id").val(totalId);
            $("#markSure").click();
        }
        function selectAll(sender) {
            var selects = $(".roomSelectUl li").find(".label").find("img");
            selects.each(function () {
                var obj = $(this);
                if ($(sender).attr("flag")=="false") {
                    obj.closest("a").addClass("active");
                    obj.attr("src", "/Content/Images/checked.png");
                    $(sender).attr("flag", "true");
                }
                else {
                    obj.closest("a").removeClass("active");
                    obj.attr("src", "/Content/Images/unchecked.png");
                    $(sender).attr("flag", "false");
                }
            });
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div id='view' style="-webkit-overflow-scrolling: touch; overflow: auto!important;">
        <div id="roomSelectDiv" runat="server">
        <%--<section class="clearfix process">
            <dl class="publishProgress clearfix">
               
                <dd class="step01 l pass">
                    <h3 class="icon iconfont icon-fangjian"></h3>
                    <h4>房间选择</h4>
                </dd>
                <dt class="progressBar01 l">
                    <div class="progressCenter"></div>
                </dt>
                <dd class="step02 l">
                    <h3 class="icon iconfont icon-peisongzhong"></h3>
                    <h4>仓库取货</h4>
                </dd>
            </dl>
        </section>--%>
        <div class="roomSelectUl">
            <ul class="clearfix">
                <asp:Repeater ID="rooms_rp" runat="server">
                    <ItemTemplate>
                       
                        <li>
                            <a href="#" data-id='<%#Eval("mac")%>' onclick="select_click(this);">
                                <img class="roomclass" src="/Content/Images/<%#(bool)Eval("online")?"room.png":"room_off.png" %>" />
                                <p class="roomNumber"><%#Eval("room") %></p>
                                <p class="label" ><img  class="gou" src="/Content/Images/unchecked.png"/></p>
                            </a>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
            <div class="clearfix"></div>
        <div class="btnWrap" style="margin-top: 10px; float:left; margin-bottom:50px; ">
            <input id="room_id" runat="server" type="hidden" class="room_id"/>
            <asp:Button  runat="server" ID="markSure" OnClick="markSure_Click" style="display:none;" Text="确认"/>
                         <a class="makeSure"href="#" style="float:left; width:45%; line-height:40px; height:40px; margin:5px;" flag ="false" onclick="selectAll(this);">全选</a>
		    <a class="makeSure"href="#" style="float:left; width:45%; line-height:40px; height:40px; margin:5px;" onclick="makeSureClick();">确认</a>


	    </div>
        
            </div>
           <div  class="clearfix">
               <div id="pickUp" runat="server">
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
                  <%-- <li class="clearfix">
                                <div class="imgWrap l">
                                    <img src="" alt="1" />
                                </div>
                                <div class="info l">
                                    <h3><span>xxx</span> &nbsp;<span>xxx></span></h3>
                                </div>
                                <div class="num r iconfont icon-cha"><span>2</span></div>
                            </li>
                     <li class="clearfix">
                                <div class="imgWrap l">
                                    <img src="" alt="1" />
                                </div>
                                <div class="info l">
                                    <h3><span>xxx</span> &nbsp;<span>xxx></span></h3>
                                </div>
                                <div class="num r iconfont icon-cha"><span>2</span></div>
                            </li>
                     <li class="clearfix">
                                <div class="imgWrap l">
                                    <img src="" alt="1" />
                                </div>
                                <div class="info l">
                                    <h3><span>xxx</span> &nbsp;<span>xxx></span></h3>
                                </div>
                                <div class="num r iconfont icon-cha"><span>2</span></div>
                            </li>
                     <li class="clearfix">
                                <div class="imgWrap l">
                                    <img src="" alt="1" />
                                </div>
                                <div class="info l">
                                    <h3><span>xxx</span> &nbsp;<span>xxx></span></h3>
                                </div>
                                <div class="num r iconfont icon-cha"><span>2</span></div>
                            </li>  <li class="clearfix">
                                <div class="imgWrap l">
                                    <img src="" alt="1" />
                                </div>
                                <div class="info l">
                                    <h3><span>xxx</span> &nbsp;<span>xxx></span></h3>
                                </div>
                                <div class="num r iconfont icon-cha"><span>2</span></div>
                            </li>--%>
                </ul>
               </div>
                <div class="noTask" runat="server" style="text-align: center; padding-top: 40%; display:none;" id="noTask">
                暂无配送任务
            </div>
           </div>
            
        <div class="noRoom" runat="server" style="text-align:center;padding-top:40%; " id="empty_div">
            <p>暂无配送房间</p>
        </div>
         <div class="footer_bar openwebview">
               <uc:Footer ID="UserFooter" runat="server" EnableViewState="False"></uc:Footer>
        </div>
             
    </form>
</body>
</html>

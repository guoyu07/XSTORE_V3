<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="XStore.WebSite.WebSite._Ascx.Footer" %>
<!--用户-->
<%@ Import Namespace="XStore.Entity" %>
<div style="position: fixed; bottom: 1px; background-color: #ffffff; opacity: 0.8; width:100%;" >
    <nav id="foot">
        <ul class="clearfix">
            <li style="width: 50%;">
                <a href='<%=Constant.JsGoodsDic+"GoodsList.aspx" %>' name="con">
                    <div class="index_bot">
                        <img class="picOff" src="/Content/Images/mySpace.png" alt="" />
                        <img class="picOn" src="/Content/Images/mySpace_on.png" />
                    </div>
                    <p>私享空间</p>
                </a>
            </li>
            <li style="width: 50%;">
                <a href='<%=Constant.JsOrderDic+"MyOrder.aspx" %>' name="con">
                    <div class="index_bot">
                        <img class="picOff" src="/Content/Images/myself.png" alt="" />
                        <img class="picOn" src="/Content/Images/myself_on.png" alt="" />
                    </div>
                    <p>我的</p>
                </a>
            </li>

        </ul>
    </nav>
</div>

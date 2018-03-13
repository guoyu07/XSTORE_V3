<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MangeCenterFooter.ascx.cs" Inherits="XStore.WebSite.WebSite._Ascx.MangeCenterFooter" %>
<%@ Import Namespace="XStore.Entity" %>
<div class="warp">
<nav id="foot">
    <ul class="clearfix">
       
        <li style="width: 100%;">
            <a href='<%=Constant.JsCenterDic+"ManageCenter.aspx" %>' name="con">
                <div class="index_bot">
                    <img class="picOff" src="/Content/Images/myself.png" alt="" />
                    <img class="picOn" src="/Content/Images/myself_on.png" />
                </div>
                <p>我的</p>
            </a>
        </li>

    </ul>
</nav>
    </div>
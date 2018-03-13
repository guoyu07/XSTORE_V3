using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace XStore.WebSite
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/bundles/CommonStyle").Include(
                "~/Content/Login/reset.css",
                "~/Content/Login/common.css",
                "~/Content/layer.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/CommonJs").Include(
                "~/Scripts/jquery-1.10.2.js",
                "~/Scripts/Plugins/layer.js",
                  "~/Scripts/common.js"
                ));
            #region Swipe
            bundles.Add(new StyleBundle("~/bundles/swiper/css").Include(
               "~/Content/swiper.min.css"
               ));
            bundles.Add(new ScriptBundle("~/bundles/swiper/js").Include(
               "~/Scripts/swiper.min.js"
               ));
            #endregion
            bundles.Add(new StyleBundle("~/bundles/weui/css").Include(
               "~/Content/weui.css"
               ));
            bundles.Add(new ScriptBundle("~/bundles/weui/js").Include(
               "~/Scripts/weui.js"
               ));
            #region WeUi

            #endregion


            #region 登陆界面
            bundles.Add(new StyleBundle("~/bundles/login").Include(
               "~/Content/Login/login.css"
               ));
            #endregion
            #region 欢迎界面
            bundles.Add(new StyleBundle("~/bundles/welcome").Include(
               "~/Content/Login/welcome.css"
               ));
            #endregion
            #region 账号绑定
            bundles.Add(new StyleBundle("~/bundles/bind").Include(
            "~/Content/Login/bind.css"
            ));
            #endregion
            #region 离线页面
            bundles.Add(new StyleBundle("~/bundles/nopower").Include(
          "~/Content/Login/nopower.css"
          ));
            #endregion
            #region 商品列表
            bundles.Add(new StyleBundle("~/bundles/goodslist/css").Include(
           "~/Content/Goods/buyerIndex.css"
           ));
            bundles.Add(new ScriptBundle("~/bundles/goodslist/js").Include(
                "~/Scripts/Plugins/vipspa.js",
                "~/Scripts/Modules/mySpace.js"
                ));
            #endregion
            #region 商品详情
            bundles.Add(new StyleBundle("~/bundles/detail/css").Include(
              "~/Content/Goods/detail.css"
              ));
            #endregion
            #region 我的订单
            bundles.Add(new ScriptBundle("~/bundles/myorder/js").Include(
               "~/Scripts/Plugins/layer.js",
               "~/Scripts/common.js",
               "~/Scripts/Modules/myself.js",
               "~/Scripts/Plugins/vipspa.js"
            ));


            #endregion

            #region 支付中心
            bundles.Add(new StyleBundle("~/bundles/paycenter/css").Include(
             "~/Content/Order/payCenter.css"
             ));
            bundles.Add(new ScriptBundle("~/bundles/paycenter/js").Include(
             "~/Scripts/Modules/payCenter.js"
             ));
            #endregion

            #region 支付成功
            bundles.Add(new StyleBundle("~/bundles/paysuccess/css").Include(
            "~/Content/Order/paySuccess.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/paysuccess/js").Include(
             "~/Scripts/jquery.myProgress.js"
             ));
            #endregion

            #region 支付宝支付
            bundles.Add(new ScriptBundle("~/bundles/paysuccess/js").Include(
             "~/Scripts/Util.js"
             ));
            #endregion

            #region 开箱失败
            bundles.Add(new StyleBundle("~/bundles/payfail/css").Include(
            "~/Content/Order/payFail.css"
            ));
            #endregion

            #region 前台个人中心
            bundles.Add(new StyleBundle("~/bundles/employeecenter/css").Include(
                "~/Content/Center/distributer.css",
                "~/Content/footer.css"
           ));
            bundles.Add(new ScriptBundle("~/bundles/employeecenter/js").Include(
             "~/Scripts/Modules/dsMyself.js"
             ));
            #endregion



            #region 常规补货-房间选择
            bundles.Add(new StyleBundle("~/bundles/roomselect/css").Include(
                "~/Content/Opreation/roomSelect.css",
                "~/Content/Center/distributer.css",
                "~/Content/footer.css"
           ));
            bundles.Add(new ScriptBundle("~/bundles/roomselect/js").Include(
             "~/Scripts/Plugins/vipspa.js",
             "~/Scripts/Plugins/vipspa-dev.js"
             ));
            #endregion

            #region 常规补货
            bundles.Add(new StyleBundle("~/bundles/roomfillup/css").Include(
               "~/Content/Opreation/roomFillUp.css",
               "~/Content/Center/distributer.css",
               "~/Content/footer.css"
            ));
            
            #endregion

            #region 常规补货-取货列表
            bundles.Add(new StyleBundle("~/bundles/pickup/css").Include(
                "~/Content/Center/distributer.css",
                "~/Content/footer.css"
           ));
            bundles.Add(new ScriptBundle("~/bundles/pickup/js").Include(
              "~/Scripts/Modules/pickUp.js",
             "~/Scripts/Plugins/vipspa.js",
             "~/Scripts/Plugins/vipspa-dev.js"
             ));
            #endregion

            #region 常规补货-取货完成
            bundles.Add(new StyleBundle("~/bundles/finishpickup/css").Include(
               "~/Content/Opreation/roomsPickUp.css",
                "~/Content/Center/distributer.css",
               "~/Content/footer.css"
          ));
            bundles.Add(new ScriptBundle("~/bundles/finishpickup/js").Include(
            "~/Scripts/Plugins/vipspa.js",
            "~/Scripts/Plugins/vipspa-dev.js"
            ));
            #endregion

            #region 常规补货-房间补货
            bundles.Add(new StyleBundle("~/bundles/roomfixed/css").Include(
               "~/Content/Opreation/roomGoods.css",
                "~/Content/Center/distributer.css",
               "~/Content/footer.css"
          ));
            #endregion

            #region 常规补货-补货记录
            bundles.Add(new StyleBundle("~/bundles/backlog/css").Include(
             "~/Content/Opreation/deliveryNote.css",
            "~/Content/Center/distributer.css",
             "~/Content/mui.min.css",
             "~/Content/footer.css"
            ));
            #endregion


            #region 酒店经理个人中心
            bundles.Add(new StyleBundle("~/bundles/managecenter/css").Include(
             "~/Content/Center/hotelManager.css",
             "~/Content/footer.css"
            ));
            #endregion

            #region 酒店经理-基础信息
            bundles.Add(new StyleBundle("~/bundles/baseinfo/css").Include(
            "~/Content/Information/comprehensive.css",
            "~/Content/footer.css"
           ));
            #endregion

            #region 酒店经理-开箱检查
            bundles.Add(new StyleBundle("~/bundles/roomcheck/css").Include(
           "~/Content/Center/distributer.css",
           "~/Content/footer.css"
          ));

            bundles.Add(new StyleBundle("~/bundles/roomgoods/css").Include(
              "~/Content/Opreation/roomGoods.css",
               "~/Content/Center/distributer.css",
              "~/Content/footer.css"
         ));
            #endregion
            #region 酒店经理-基础信息-房间信息
            bundles.Add(new StyleBundle("~/bundles/roominfo/css").Include(
         "~/Content/Information/comprehensive.css",
         "~/Content/footer.css"
        ));
            #endregion

            #region 酒店经理-人员管理
            bundles.Add(new StyleBundle("~/bundles/employee/css").Include(
        "~/Content/Information/employeeManager.css",
        "~/Content/footer.css"
       ));
            bundles.Add(new StyleBundle("~/bundles/changepassword/css").Include(
       "~/Content/Login/changePsd.css",
       "~/Content/footer.css"
      ));

            #endregion

            #region 酒店经理-销售业绩
            bundles.Add(new StyleBundle("~/bundles/achievement/css").Include(
                 "~/Content/mui.min.css",
            "~/Content/Information/achievement.css",
            "~/Content/footer.css"
           ));
            bundles.Add(new ScriptBundle("~/bundles/achievement/js").Include(
           "~/Scripts/mui.js"
           ));
            #endregion

            #region 酒店经理-业绩查询
            bundles.Add(new StyleBundle("~/bundles/settlement/css").Include(
            "~/Content/mui.min.css",
           "~/Content/Information/settlement.css",
           "~/Content/footer.css"
          ));
            bundles.Add(new ScriptBundle("~/bundles/settlement/js").Include(
           "~/Scripts/mui.js"
           ));
            #endregion

            #region 区域经理-酒店列表
            bundles.Add(new StyleBundle("~/bundles/comprehensive/css").Include(
           "~/Content/Opreation/comprehensive.css",
           "~/Content/footer.css"
          ));
            #endregion

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinPush
{
    public class EnumCommon
    {
        #region -----订单状态-----
        public enum 订单状态 : int
        {
            待付款 = 1,
            待开箱 = 2,
            已开箱 = 3,
            支付失败 = 4,
            开箱失败 = 5,
            申请退款 = 6,
            退款完成 = 7
        
        }
        #endregion

        #region -----入库类型-----
        public enum 入库类型 : int
        { 
            手机入库 = 1
        
        }
        #endregion

        #region -----出库类型-----
        public enum 出库类型 : int
        {
            手机出库 = 1

        }
        #endregion

        #region -----角色权限-----
        public enum 角色权限 : int
        {
            经理 = 1,
            财务 = 2,
            前台 = 3,
            区域经理 = 4,
            集团经理 = 5,
            集团财务 = 6,
            后台财务 = 7,
            后台管理员 = 8,
            测试员 = 9
        }
        #endregion

        #region -----推送权限-----
        public enum 推送权限 : int
        {
            正常订单推送 = 1,
            异常订单推送 = 2,
            业绩统计推送 = 3,
            系统异常推送 = 4
        }
        #endregion
    }
}

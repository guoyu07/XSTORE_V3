using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Timers;
using Newtonsoft.Json;
using WeiXinPush.Model;
using Chloe.MySql;
using XStore.WebSite.DBFactory;
using Chloe;
using XStore.Entity;
using static XStore.Entity.Enum;
using Newtonsoft.Json.Linq;
using XStore.Common;
using XStore.Entity.Model;

namespace WeiXinPush
{
    public partial class WeiXinPushServices : ServiceBase
    {

        private Timer _timer;
        private Timer __timer;
        private Timer _exceptionTimer;
        private Timer _openBoxTimer;
        private Timer _fillUpTimer;
        private Timer _fixedTimer;
        private Timer _failpushTimer;
        public static string connString = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        private int pushHour = int.Parse(ConfigurationManager.AppSettings["PUSHHOUR"]);
        private string root = ConfigurationManager.AppSettings["HomeUrl"].ObjToStr();
        public MySqlContext context;
        public WeiXinPushServices()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                context = new MySqlContext(new MySqlConnectionFactory(connString));

                _timer = new Timer(1000);
                _timer.Elapsed += timer_Elapsed;
                _timer.Start();

                __timer = new Timer(1000);
                __timer.Elapsed += _timer_Elapsed;
                __timer.Start();

                //TODO
                //_exceptionTimer = new Timer(1000);
                //_exceptionTimer.Elapsed += _exceptionTimer_Elapsed;
                //_exceptionTimer.Start();

                _openBoxTimer = new Timer(10000);
                _openBoxTimer.Elapsed += _openboxTimer_Elapsed;
                _openBoxTimer.Start();

                _fillUpTimer = new Timer(1000);
                _fillUpTimer.Elapsed += _fillUpTimer_Elapsed;
                _fillUpTimer.Start();

                //TODO
                //_fixedTimer = new Timer(4000);
                //_fixedTimer.Elapsed += _fixedTime_Elapsed;
                //_fixedTimer.Start();

                _failpushTimer = new Timer(1000);
                _failpushTimer.Elapsed += _failpushTimer_Elapsed;
                _failpushTimer.Start();


            }
            catch (Exception ex)
            {
                Log.WriteLog("微信推送", "数据异常", ex.Message + ";异常位置：" + ex.StackTrace);
            }
        }
        //检测开箱失败订单，兵进行重新开箱操作
        private void _openboxTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _openBoxTimer.Stop();
            var orderInfo = context.Query<OrderInfo>()
                .FirstOrDefault(o => o.paid == true && o.delivered == false && o.date.AddMinutes(30) > DateTime.Now);
            if (orderInfo != null)
            {
                var rbh = new RemoteBoxHelper();
                rbh.OpenRemoteBox(orderInfo.cabinet_mac.ObjToStr(), orderInfo.code.ObjToStr(), orderInfo.pos.ObjToStr());
            }
            _openBoxTimer.Start();
        }
        //private void _fixedTime_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    var selectSql = string.Format(@"select top 1 * from WP_补货单 where Status in(1,3) and datediff(MINUTE,CreateTime,getdate()) <= 30");
        //    DataTable selectDt = comfun.GetDataTableBySQL(selectSql);
        //    if (selectDt.Rows.Count > 0)
        //    {
        //        var orderNo = selectDt.Rows[0]["OrderNo"].ToString();
        //        var position = selectDt.Rows[0]["Position"].ToString();
        //        var mac = selectDt.Rows[0]["Mac"].ToString();
        //        OpenBox(orderNo, mac, position, 0x02);
        //    }
        //}
        private void _fillUpTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour >= 8 && DateTime.Now.Minute > 30)
            {
                if (DateTime.Now.Second == 0)
                {
                    _fillUpTimer.Stop();
                    FillUpGoodsPush();
                    _fillUpTimer.Start();
                }
            }
        }

        private void _exceptionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //SystemExceptionPush();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            __timer.Stop();
            SummarizingOrderInfoPush();
            __timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            OrderInfoPush();
            FailOrderPush();
            _timer.Start();
        }

        #region 推送失败后的重复推送
        private void _failpushTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _failpushTimer.Stop();
            var pushInfo = context.Query<WeiChatPushFailLog>().FirstOrDefault(o => o.issuccess == false && o.createtime.AddMinutes(30) > DateTime.Now);
            if (pushInfo != null)
            {
                var orderInfo = context.Query<OrderInfo>()
                    .LeftJoin<Product>((a, b) => a.product == b.id)
                    .LeftJoin<Cabinet>((a, b, c) => a.cabinet_mac.Equals(c.mac))
                    .LeftJoin<Hotel>((a, b, c, d) => c.hotel == d.id)
                    .Where((a, b, c, d) => a.code.Equals(pushInfo.ordercode))
                    .Select((a, b, c, d) => new
                    {
                        a.delivered,
                        hotelid = d.id,
                        a.code,
                        b.name,
                        a.price1,
                        d.simple_name,
                        c.room,
                        a.date
                    }).FirstOrDefault();
                var color = "#173177";
                //如果开箱失败，则推送开箱失败的订单，这里只是修改颜色
                if (!orderInfo.delivered)
                {
                    color = "#D74C29";
                }
                var title = string.Format("{0}-{1}于 {2} 发生一笔订单交易。", orderInfo.simple_name, orderInfo.room, orderInfo.date.ToString("yyyy-MM-dd HH:mm:ss"));
                var keyword1 = orderInfo.code;
                var keyword2 = orderInfo.name;
                var keyword3 = 1 + "件";
                var keyword4 = orderInfo.price1.ObjToInt(0).CentToRMB(0) + " 元";
                var remark = "感谢您的使用" + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var tempId = "HfV9-ClzJoRJ_2ubWRrw1y3qy9IaZBnwFOt09QLRmHY";
                
                dynamic postData = new ExpandoObject();
                dynamic first = new ExpandoObject();
                first.value = title;
                first.color = color;

                dynamic keyword1Obj = new ExpandoObject();
                keyword1Obj.value = keyword1;
                keyword1Obj.color = color;

                dynamic keyword2Obj = new ExpandoObject();
                keyword2Obj.value = keyword2;
                keyword2Obj.color = color;

                dynamic keyword3Obj = new ExpandoObject();
                keyword3Obj.value = keyword3;
                keyword3Obj.color = color;

                dynamic keyword4Obj = new ExpandoObject();
                keyword4Obj.value = keyword4;
                keyword4Obj.color = color;

                dynamic remarkObj = new ExpandoObject();
                remarkObj.value = remark;
                remarkObj.color = color;

                postData.first = first;
                postData.keyword1 = keyword1Obj;
                postData.keyword2 = keyword2Obj;
                postData.keyword3 = keyword3Obj;
                postData.keyword4 = keyword4Obj;
                postData.remark = remarkObj;
                Log.WriteLog("微信推送", "postData", JsonConvert.SerializeObject(postData));

                Log.WriteLog("微信推送", "openId:", pushInfo.openid);
                pushInfo.issuccess = Send_WX_Message(postData, pushInfo.openid, tempId);
                Log.WriteLog("微信推送", "orderno:", orderInfo.code);
                Log.WriteLog("微信推送", "responseBool:", pushInfo.issuccess.ObjToStr());
                context.Update(pushInfo);
            }
            _failpushTimer.Start();


        }
        #endregion

        protected override void OnStop()
        {
            _timer.Stop();
            //_timer.Close();
            Log.WriteLog("微信推送", "服务器停止时间", "服务器已停止");
        }

        #region 订单推送
        protected void OrderInfoPush()
        {
            try
            {
                var orderInfo = context.Query<OrderInfo>()
                    .LeftJoin<Product>((a, b) => a.product == b.id)
                    .LeftJoin<Cabinet>((a, b, c) => a.cabinet_mac.Equals(c.mac))
                    .LeftJoin<Hotel>((a, b, c, d) => c.hotel == d.id)
                    .Where((a, b, c, d) => a.paid == true && a.delivered == true && (a.has_push.HasValue == false || a.has_push.Value == false) && a.date.AddMinutes(30) > DateTime.Now)
                    .Select((a, b, c, d) => new
                    {
                        hotelid = d.id,
                        a.code,
                        b.name,
                        a.price1,
                        d.simple_name,
                        c.room,
                        a.date
                    }).FirstOrDefault();

                if (orderInfo == null)
                {
                    return;
                }

                var color = "#173177";
                var title = string.Format("{0}-{1}于 {2} 发生一笔订单交易。", orderInfo.simple_name, orderInfo.room, orderInfo.date.ToString("yyyy-MM-dd HH:mm:ss"));
                var keyword1 = orderInfo.code;
                var keyword2 = orderInfo.name;
                var keyword3 = 1 + "件";
                var keyword4 = orderInfo.price1.ObjToInt(0).CentToRMB(0) + " 元";
                var remark = "感谢您的使用" + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var tempId = "aenuM_UsdJ_RixaKWnGEFGTlwuFQqHIyhq6OwhzvcWw";
                dynamic postData = new ExpandoObject();
                dynamic first = new ExpandoObject();
                first.value = title;
                first.color = color;

                dynamic keyword1Obj = new ExpandoObject();
                keyword1Obj.value = keyword1;
                keyword1Obj.color = color;

                dynamic keyword2Obj = new ExpandoObject();
                keyword2Obj.value = keyword2;
                keyword2Obj.color = color;

                dynamic keyword3Obj = new ExpandoObject();
                keyword3Obj.value = keyword3;
                keyword3Obj.color = color;

                dynamic keyword4Obj = new ExpandoObject();
                keyword4Obj.value = keyword4;
                keyword4Obj.color = color;

                dynamic remarkObj = new ExpandoObject();
                remarkObj.value = remark;
                remarkObj.color = color;

                postData.first = first;
                postData.keyword1 = keyword1Obj;
                postData.keyword2 = keyword2Obj;
                postData.keyword3 = keyword3Obj;
                postData.keyword4 = keyword4Obj;
                postData.remark = remarkObj;
                Log.WriteLog("微信推送", "postData", JsonConvert.SerializeObject(postData));

                var pushAuthList = context.Query<WeiChatPushRole>().LeftJoin<User>((a, b) => a.phone.Equals(b.phone))
                    .Where((a, b) => a.successorder == true)
                    .Select((a, b) => b.weichat)
                    .ToList();

                var hotelRoleList = context.Query<UserHotel>()
                    .LeftJoin<User>((a, b) => a.user_username.Equals(b.username))
                    .LeftJoin<UserRole>((a, b, c) => b.username.Equals(c.username))
                    .Where((a, b, c) => a.hotels_id == orderInfo.hotelid && (c.role_id == (int)UserRoleEnum.经理 || c.role_id == (int)UserRoleEnum.区域经理))
                    .Select((a, b, c) => b.weichat).ToList();
                var openidList = new List<string>();
                openidList.AddRange(pushAuthList);
                openidList.AddRange(hotelRoleList);
                openidList = openidList.GroupBy(o => o).Select(o => o.Key).ToList();
                var sendFlag = false;
                foreach (var openid in openidList)
                {
                    Log.WriteLog("微信推送", "openId:", openid);
                    var responseBool = Send_WX_Message(postData, openid, tempId);
                    Log.WriteLog("微信推送", "orderno:", orderInfo.code);
                    Log.WriteLog("微信推送", "responseBool:", responseBool.ToString());
                    //只要有一个发送成功则改为发送成功
                    if (responseBool)
                    {
                        sendFlag = true;
                    }
                    else
                    {
                        //如果发送失败，则存入到另外一张表，由另外一个线程继续发送
                        context.Insert(new WeiChatPushFailLog
                        {
                            createtime = DateTime.Now,
                            issuccess = false,
                            openid = openid,
                            ordercode = orderInfo.code
                        });
                    }
                }
                if (openidList.Count > 0)
                {
                    var orderInfoDB = context.Query<OrderInfo>().FirstOrDefault(o => o.code.Equals(orderInfo.code));
                    orderInfoDB.has_push = sendFlag;
                    if (context.Update(orderInfoDB) > 0)
                    {
                        Log.WriteLog("微信推送", "订单：" + orderInfo.code, "发送模板成功!!!");
                    }
                    else
                    {
                        Log.WriteLog("微信推送", "订单：" + orderInfo.code, "更新发送状态失败");
                    }
                ;
                }
               
             
            }
            catch (Exception ex)
            {
                Log.WriteLog("微信推送", "数据异常：" + ex.Message, ";异常位置：" + ex.StackTrace + ";内部异常信息：" + ex.InnerException.Message + ";内部异常位置：" + ex.InnerException.StackTrace);

            }
        }
        #endregion

        #region 开箱失败订单推送

        public void FailOrderPush()
        {
            try
            {
                var orderInfo = context.Query<OrderInfo>()
                    .LeftJoin<Product>((a, b) => a.product == b.id)
                    .LeftJoin<Cabinet>((a, b, c) => a.cabinet_mac.Equals(c.mac))
                    .LeftJoin<Hotel>((a, b, c, d) => c.hotel == d.id)
                    .Where((a, b, c, d) => a.paid == true && a.delivered == false && (a.has_push.HasValue == false || a.has_push.Value == false) && a.date.AddMinutes(30) >= DateTime.Now && a.date.AddSeconds(30)<=DateTime.Now)
                    .Select((a, b, c, d) => new
                    {
                        hotelid = d.id,
                        a.code,
                        b.name,
                        a.price1,
                        d.simple_name,
                        c.room,
                        a.date
                    }).FirstOrDefault();

                if (orderInfo == null)
                {
                    return;
                }
                var color = "#D74C29";
                var title = string.Format("{0}-{1}于 {2} 发生一笔开箱失败交易订单。", orderInfo.simple_name, orderInfo.room, orderInfo.date.ToString("yyyy-MM-dd HH:mm:ss"));
                var keyword1 = orderInfo.code;
                var keyword2 = orderInfo.name;
                var keyword3 = 1 + "件";
                var keyword4 = orderInfo.price1.ObjToInt(0).CentToRMB(0) + " 元";
                var remark = "特此告知！！！";
                var tempId = "aenuM_UsdJ_RixaKWnGEFGTlwuFQqHIyhq6OwhzvcWw";
                dynamic postData = new ExpandoObject();
                dynamic first = new ExpandoObject();
                first.value = title;
                first.color = color;

                dynamic keyword1Obj = new ExpandoObject();
                keyword1Obj.value = keyword1;
                keyword1Obj.color = color;

                dynamic keyword2Obj = new ExpandoObject();
                keyword2Obj.value = keyword2;
                keyword2Obj.color = color;

                dynamic keyword3Obj = new ExpandoObject();
                keyword3Obj.value = keyword3;
                keyword3Obj.color = color;

                dynamic keyword4Obj = new ExpandoObject();
                keyword4Obj.value = keyword4;
                keyword4Obj.color = color;

                dynamic remarkObj = new ExpandoObject();
                remarkObj.value = remark;
                remarkObj.color = color;

                postData.first = first;
                postData.keyword1 = keyword1Obj;
                postData.keyword2 = keyword2Obj;
                postData.keyword3 = keyword3Obj;
                postData.keyword4 = keyword4Obj;
                postData.remark = remarkObj;
                //辅助开箱的链接
                var url = root + "Shop/pages/AssistOpenBox.aspx?OrderNo=" + orderInfo.code;

                var sendFlag = false;
                var openidList = context.Query<WeiChatPushRole>().LeftJoin<User>((a, b) => a.phone.Equals(b.phone))
                        .Where((a, b) => a.failorder == true)
                        .Select((a, b) => b.weichat)
                        .ToList();
                foreach (var openid in openidList)
                {
                    Log.WriteLog("微信推送", "openId:", openid);
                    var responseBool = Send_WX_Message(postData, openid, tempId);
                    Log.WriteLog("微信推送", "orderno:", orderInfo.code);
                    Log.WriteLog("微信推送", "responseBool:", responseBool.ToString());
                    //只要有一个发送成功则改为发送成功
                    if (responseBool)
                    {
                        sendFlag = true;
                    }
                    else
                    {
                        //如果发送失败，则存入到另外一张表，由另外一个线程继续发送
                        context.Insert(new WeiChatPushFailLog
                        {
                            createtime = DateTime.Now,
                            issuccess = false,
                            openid = openid,
                            ordercode = orderInfo.code
                        });
                    }
                }
                if (openidList.Count > 0)
                {
                    var orderInfoDB = context.Query<OrderInfo>().FirstOrDefault(o => o.code.Equals(orderInfo.code));
                    orderInfoDB.has_push = sendFlag;
                    if (context.Update(orderInfoDB) > 0)
                    {
                        Log.WriteLog("微信推送", "订单：" + orderInfo.code, "发送模板成功!!!");
                    }
                    else
                    {
                        Log.WriteLog("微信推送", "订单：" + orderInfo.code, "更新发送状态失败");
                    }
                ;
                }
              
            }
            catch (Exception ex)
            {
                Log.WriteLog("微信推送", "数据异常：" + ex.Message, ";异常位置：" + ex.StackTrace + ";内部异常信息：" + ex.InnerException.Message + ";内部异常位置：" + ex.InnerException.StackTrace);
            }
        }

        #endregion

        #region 统计推送
        protected void SummarizingOrderInfoPush()
        {
            try
            {
               
                if (DateTime.Now.Hour == pushHour && DateTime.Now.Minute == 59&&DateTime.Now.Second == 59)
                {
                    Log.WriteLog("微信推送", "进入了统计推送：", "");
                    var yestodyList = context.Query<OrderInfo>().Where(o => o.date.Date == DateTime.Now.AddDays(-1).Date).ToList();
                    var monthList = context.Query<OrderInfo>().Where(o => o.date.Year == DateTime.Now.Year && o.date.Month == DateTime.Now.Month && o.date.Day >= 1).ToList();

                    var title = "尊敬的管理员，销售业绩如下：";
                    var keyword1 = "截止:" + DateTime.Now.AddDays(-1).ToString("yyyy年MM月dd日");
                    var keyword2 = string.Format("昨日销售共 {1}件，总计 {0}元；本月销售共 {2}件，总计 {3}元", yestodyList.Sum(o => o.price1).ObjToInt(0).CentToRMB(0), yestodyList.Count, monthList.Count, monthList.Sum(o => o.price1).ObjToInt(0).CentToRMB(0));
                    var keyword3 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    var remark = "感谢您的使用";
                    var tempId = "hO8PfzQOEve1m_XXRlkmJot0S-u5ca3_XodnymasYhc";
                    var color = "#173177";

                    dynamic postData = new ExpandoObject();
                    dynamic first = new ExpandoObject();
                    first.value = title;
                    first.color = color;

                    dynamic keyword1Obj = new ExpandoObject();
                    keyword1Obj.value = keyword1;
                    keyword1Obj.color = color;

                    dynamic keyword2Obj = new ExpandoObject();
                    keyword2Obj.value = keyword2;
                    keyword2Obj.color = color;

                    dynamic keyword3Obj = new ExpandoObject();
                    keyword3Obj.value = keyword3;
                    keyword3Obj.color = color;


                    dynamic remarkObj = new ExpandoObject();
                    remarkObj.value = remark;
                    remarkObj.color = color;

                    postData.first = first;
                    postData.keyword1 = keyword1Obj;
                    postData.keyword2 = keyword2Obj;
                    postData.keyword3 = keyword3Obj;
                    postData.remark = remarkObj;

                    var pushAuthList = context.Query<WeiChatPushRole>().LeftJoin<User>((a, b) => a.phone.Equals(b.phone))
                   .Where((a, b) => a.statistics == true)
                   .Select((a, b) => b.weichat)
                   .ToList();
                    foreach (string pushAuth in pushAuthList)
                    {
                        var returnState = Send_WX_Message(postData, pushAuth, tempId);
                        Log.WriteLog("微信推送", "returnState：", returnState.ToString());
                    };
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("微信推送", "数据异常：" + ex.Message, ";异常位置：" + ex.StackTrace + ";内部异常信息：" + ex.InnerException.Message + ";内部异常位置：" + ex.InnerException.StackTrace);
            }

        }
        #endregion

        //TODO
        #region 系统异常推送

        protected void SystemExceptionPush()
        {
            var selectSql = string.Format(@"SELECT TOP 1 mac, datediff(MINUTE,createtime,getdate()) as offset
        FROM [tshop].[dbo].[WP_设备心跳记录表]
        order by createtime desc");
            var selectDt = comfun.GetDataTableBySQL(selectSql);
            if (selectDt.Rows.Count > 0)
            {
                Log.WriteLog("微信推送", "SystemExceptionPush:", "");
                //系统已经发生故障
                if (selectDt.Rows[0]["offset"].ObjToInt(0) >= 3)
                {
                    Log.WriteLog("微信推送", "系统已经发生故障:", "");
                    var exceptionSql = string.Format(@"select * from WP_SystemExceptionLog where IsException =1");
                    var exceptionDt = comfun.GetDataTableBySQL(exceptionSql);
                    //说明没有待处理的系统异常，是第一次趴窝或者是恢复后又再次趴窝
                    if (exceptionDt.Rows.Count == 0)
                    {
                        Log.WriteLog("微信推送", "说明没有待处理的系统异常，是第一次趴窝或者是恢复后又再次趴窝", "");
                        var insertSql = string.Format(@" insert into WP_SystemExceptionLog(IsException) values(1)");
                        comfun.InsertBySQL(insertSql);
                        ExceptionPush(DateTime.Now);
                    }
                    else if (DateTime.Now.Minute == 59 && DateTime.Now.Second == 59)
                    {
                        Log.WriteLog("微信推送", "一小时发一次", "");
                        ExceptionPush(exceptionDt.Rows[0]["ExceptionTime"].ObjToDateTime());
                    }
                }
                //说明系统已经恢复
                else
                {
                    Log.WriteLog("微信推送", "说明系统已经恢复", "");
                    var exceptionSql = string.Format(@"select count(*) from WP_SystemExceptionLog where IsException =1");
                    var exceptionDt = comfun.GetDataTableBySQL(exceptionSql);
                    //说明没有待处理的系统异常，是第一次趴窝或者是恢复后又再次趴窝
                    if (exceptionDt.Rows.Count > 0 && int.Parse(exceptionDt.Rows[0][0].ToString()) != 0)
                    {
                        Log.WriteLog("微信推送", "说明没有待处理的系统异常，是第一次趴窝或者是恢复后又再次趴窝", "");
                        var updateSql = string.Format(@" update WP_SystemExceptionLog set IsException = 0,RecoveryTime = getdate() where IsException = 1");
                        comfun.UpdateBySQL(updateSql);
                        return;
                    }

                }
            }
        }

        private void ExceptionPush(DateTime time)
        {
            Log.WriteLog("微信推送", "开始发送异常推送", "");
            var title = "尊敬的陈总：";
            var keyword1 = "酒店智能售货机";
            var keyword2 = time.ToString("yyyy-MM-dd HH:mm:ss");
            var keyword3 = "所有设备已全部停止工作，可能需要重启系统";
            var keyword4 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var remark = "请尽快处理！！！";

            //Log.WriteLog("微信推送", "openId:", openId);
            var tempId = "FWNdGDiTWYD4JrNPumPqbR_0AwiShQesxFN8kLAD8Bw";
            var color = "#D74C29";

            dynamic postData = new ExpandoObject();
            dynamic first = new ExpandoObject();
            first.value = title;
            first.color = color;

            dynamic keyword1Obj = new ExpandoObject();
            keyword1Obj.value = keyword1;
            keyword1Obj.color = color;

            dynamic keyword2Obj = new ExpandoObject();
            keyword2Obj.value = keyword2;
            keyword2Obj.color = color;

            dynamic keyword3Obj = new ExpandoObject();
            keyword3Obj.value = keyword3;
            keyword3Obj.color = color;

            dynamic keyword4Obj = new ExpandoObject();
            keyword4Obj.value = keyword4;
            keyword4Obj.color = color;


            dynamic remarkObj = new ExpandoObject();
            remarkObj.value = remark;
            remarkObj.color = color;

            postData.first = first;
            postData.keyword1 = keyword1Obj;
            postData.keyword2 = keyword2Obj;
            postData.keyword3 = keyword3Obj;
            postData.keyword4 = keyword4Obj;
            postData.remark = remarkObj;

            foreach (string openId in GetOpenId((int)EnumCommon.推送权限.系统异常推送))
            {
                Send_WX_Message(postData, openId, tempId);
            };
        }

        #endregion

        #region 补货推送
        protected void FillUpGoodsPush()
        {
           
            if (CacheHelper.GetCache("FillUpList")==null)
            {
                var requestUrl = "http://139.199.160.173:9119/test/hotel?hotelId=0";
                var response = JsonConvert.DeserializeObject<FillUpResponse>(Utils.HttpGet(requestUrl));
                if (response.operationStatus.Equals("SUCCESS"))
                {
                    CacheHelper.SetCache("FillUpList", response.operationMessage, new TimeSpan(12, 0, 0));
                }
                else
                {
                    return;
                }
            }
            var list = (List<HotelQuery>)CacheHelper.GetCache("FillUpList");
            var nowHotel = list.FirstOrDefault(o => o.hasPush == false);
            var pushAuthList = context.Query<WeiChatPushRole>().LeftJoin<User>((a, b) => a.phone.Equals(b.phone))
                 .Where((a, b) => a.successorder == true)
                 .Select((a, b) => b.weichat)
                 .ToList();

            var hotelRoleList = context.Query<UserHotel>()
                .LeftJoin<User>((a, b) => a.user_username.Equals(b.username))
                .LeftJoin<UserRole>((a, b, c) => b.username.Equals(c.username))
                .Where((a, b, c) => a.hotels_id == nowHotel.hotelId&&(c.role_id == (int)UserRoleEnum.经理 || c.role_id == (int)UserRoleEnum.区域经理))
                .Select((a, b, c) => b.weichat).ToList();
            var openidList = new List<string>();
            openidList.AddRange(pushAuthList);
            openidList.AddRange(hotelRoleList);
            openidList = openidList.GroupBy(o => o).Select(o => o.Key).ToList();
            var hotelInfo = context.Query<Hotel>().FirstOrDefault(o => o.id == nowHotel.hotelId);
         
            foreach (var openId in openidList)
            {
                var color = "#865FC5";
                var title = string.Format("尊敬的酒店经理，今日补货信息如下。");
                var keyword1 = hotelInfo.simple_name.ObjToStr();
                var keyword2 = nowHotel.cabNum + " 间";
                var keyword3 = nowHotel.proNum + "件";
                var remark = "截止至 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Substring(0, 10) + " 08:00:00;\n请尽快安排补货任务;";
                var tempId = "8A2OZkYnac3yv0oO8iNkSz_lwfp_clVfagm_AQpFB_o";
                dynamic postData = new ExpandoObject();
                dynamic first = new ExpandoObject();
                first.value = title;
                first.color = color;

                dynamic keyword1Obj = new ExpandoObject();
                keyword1Obj.value = keyword1;
                keyword1Obj.color = color;

                dynamic keyword2Obj = new ExpandoObject();
                keyword2Obj.value = keyword2;
                keyword2Obj.color = color;

                dynamic keyword3Obj = new ExpandoObject();
                keyword3Obj.value = keyword3;
                keyword3Obj.color = color;

                dynamic remarkObj = new ExpandoObject();
                remarkObj.value = remark;
                remarkObj.color = color;

                postData.first = first;
                postData.keyword1 = keyword1Obj;
                postData.keyword2 = keyword2Obj;
                postData.keyword3 = keyword3Obj;
                postData.remark = remarkObj;
                var responseBool = Send_WX_Message(postData, openId, tempId);

            }
            var model = list.FirstOrDefault(o => o.hotelId == nowHotel.hotelId);
            model.hasPush = true;
            CacheHelper.SetCache("FillUpList",list, new TimeSpan(12, 0, 0));
            Log.WriteLog("微信推送", "酒店id：" + nowHotel.hotelId, "更新发送状态失败");
           

        }

        #endregion

        protected List<string> GetOpenId(int _enum)
        {
            string sql = string.Empty;
            switch ((EnumCommon.推送权限)_enum)
            {
                case EnumCommon.推送权限.正常订单推送:
                    sql = string.Format(@"select openid from View_WechatPushAdmin where SendAuth = 1");
                    break;
                case EnumCommon.推送权限.异常订单推送:
                    sql = string.Format(@"select openid from View_WechatPushAdmin where FailSendAuth = 1");
                    break;
                case EnumCommon.推送权限.业绩统计推送:
                    sql = string.Format(@"select openid from View_WechatPushAdmin where StatisticsAuth = 1");
                    break;
                case EnumCommon.推送权限.系统异常推送:
                    sql = string.Format(@"select openid from View_WechatPushAdmin where SystemExceptionAuth = 1");
                    break;
            }
            DataTable dt = comfun.GetDataTableBySQL(sql);
            if (dt.Rows.Count == 0)
            {
                return new List<string>();
            }
            else
            {
                var openIdList = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    openIdList.Add(dr["openid"].ToString());
                }
                return openIdList;
            }
        }

        /// <summary>
        /// 模板推送
        /// </summary>
        /// <param name="bx_content"></param>
        /// <param name="bx_Address"></param>
        /// <param name="openid"></param>
        /// <param name="Remarks"></param>
        public bool Send_WX_Message(Object postDataObj, string openid, string template_id, string url = "")
        {
            try
            {
                //获取AccessToken
                string accessToken = GetAccessToken();
                //Log.WriteLog("微信推送", "access_token:", accessToken);
                //第一步设置所属行业
                msgData msg = new msgData();
                msg.touser = openid;
                if (!string.IsNullOrEmpty(msg.touser))
                {
                    msg.template_id = template_id;

                    msg.url = url;

                    msg.data = postDataObj;

                    string postUrl = string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", accessToken);

                    string postData = JsonConvert.SerializeObject(msg);
                    //Log.WriteLog("微信推送", "模板发送postData:", JsonConvert.SerializeObject(postData));
                    JObject result = JsonConvert.DeserializeObject<JObject>(webRequestPost(postUrl, postData));
                    Log.WriteLog("微信推送", "result:", JsonConvert.SerializeObject(result));
                    if (result["errcode"].ObjToStr().Equals("0"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                Log.WriteLog("微信推送", "模板发送异常:" + ex.Message, ";发送异常的位置：" + ex.StackTrace);
                return false;
            }

        }
        /// <summary>
        /// Post 提交调用抓取
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="param">参数</param>
        /// <returns>string</returns>
        public static string webRequestPost(string url, string param)
        {

            //Log.WriteLog("微信推送", "开始发送模板消息:", "-------");
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(param);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "Post";
            req.Timeout = 120 * 1000;
            req.ContentType = "application/x-www-form-urlencoded;";
            req.ContentLength = bs.Length;
            //Log.WriteLog("微信推送", "开始请求。。。:", "-------");
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Flush();
                reqStream.Close();

            }
            //Log.WriteLog("微信推送", "发送模板消息成功:", "-------");
            using (WebResponse wr = req.GetResponse())
            {
                //在这里对接收到的页面内容进行处理 
                try
                {
                    Stream strm = wr.GetResponseStream();
                    //Log.WriteLog("微信推送", " req.GetResponse():", strm.ToString());
                    StreamReader sr = new StreamReader(strm, System.Text.Encoding.UTF8);
                    string line;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.Append(line + System.Environment.NewLine);
                    }
                    req.Abort();
                    sr.Close();
                    strm.Close();
                    return sb.ToString();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        private string GetAccessToken() {

            var requestUrl = Constant.YunApiV2 + "Shop/ashx/GetAccessToken.ashx";
            var response = JsonConvert.DeserializeObject<AjaxResponse>(Utils.HttpGet(requestUrl));
            if (response.success)
            {
                //Session[Constant.AccessToken] = response.message.ObjToStr();
                return response.message.ObjToStr();
            }
            else
            {
                return string.Empty;
            }

        }
        

    }

}


public struct msgData
{
    public string touser;

    public string template_id;

    public string url;

    public Object data;
}
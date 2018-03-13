using boxes.Common;
using boxes.DBUtility;
using Newtonsoft.Json;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SuperSocket.SocketBase.Config;
using System.Data;
using XStore.Common;
using XStore.Entity.Model;
using XStore.Entity;
using System.Timers;
using Newtonsoft.Json.Linq;

namespace boxes
{
    public partial class BoxForm : Form
    {
        BoxServer boxServer = new BoxServer();

        public BoxForm()
        {
            InitializeComponent();
            InitForm();
        }

        #region 初始化程序
        private void InitForm()
        {
            ServerConfig serverConfig = new ServerConfig
            {
                Port = 2758,
                MaxConnectionNumber = 10000,
                IdleSessionTimeOut = 420
            };

            InitBoxes();
            var initTimer = new System.Timers.Timer(3600000);
            initTimer.Elapsed += initTimer_Elapsed;
            initTimer.Start();

            var offlineTimer = new System.Timers.Timer(1000);
            offlineTimer.Elapsed += offlineTimer_Elapsed;
            offlineTimer.Start();

            if (!boxServer.Setup(serverConfig))
            {
                ShowLog(txtLog, "初始化错误");
                return;
            }
            if (!boxServer.Start())
            {
                ShowLog(txtLog, "启动服务失败");
                return;
            }

            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "session超时：" + boxServer.Config.IdleSessionTimeOut);
            //注册连接事件
            boxServer.NewSessionConnected += BoxProtocolServer_NewSessionConnected;
            //注册请求事件
            boxServer.NewRequestReceived += BoxProtocolServer_NewRequestReceived;
            //注册Session关闭事件
            boxServer.SessionClosed += BoxProtocolServer_SessionClosed;
            //boxServer.Stop();
        }
        #endregion

        #region 初始化箱子在线信息
        private void initTimer_Elapsed(object sender, ElapsedEventArgs e) {

            InitBoxes();
        }
        private void InitBoxes()
        {
            var requestUrl = string.Format("{0}test/cabinets?page=0&size=10000 ", Constant.YunApi);
            var response = JsonConvert.DeserializeObject<JObject>(Utils.HttpGet(requestUrl));
            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + ";InitBoxesReponse:" + response);
            if (response["operationStatus"].ObjToStr().Equals("SUCCESS"))
            {
                var arrList = JsonConvert.DeserializeObject<JArray>(response["operationMessage"].ObjToStr());
                List<OnlineBox> macList = new List<OnlineBox>();

                foreach (var arr in arrList)
                {
                    var sub = JsonConvert.DeserializeObject<JArray>(arr.ToString());
                    var mac = new OnlineBox();
                    mac.mac = sub[0].ObjToStr();
                    mac.online = sub[1].ObjToInt(0) == 0 ? false : true;
                    mac.lineTime = DateTime.Now;
                    macList.Add(mac);
                }
                CacheHelper.SetCache("Boxes", macList);
            }

        }
        #endregion

        #region 离线检测
        private void offlineTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var cache = ((List<OnlineBox>)CacheHelper.GetCache("Boxes")).Where(o => DateTime.Compare(o.lineTime.AddMinutes(5), DateTime.Now) < 0 && o.online == true).Select(o=>o.mac).ToList();
            if (cache.Count>0)
            {
                var macStr = cache.Aggregate((x, y) => x + "," + y);
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + ";离线macStr:" + macStr);
                var requestUrl = string.Format("{0}test/offline?macs={1}", Constant.YunApi, macStr);
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + ";离线requestUrl:" + requestUrl);
                var response = JsonConvert.DeserializeObject<BuyResponse>(Utils.HttpGet(requestUrl));
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + ";离线response:" + response);
                if (!response.operationStatus.Equals("SUCCESS"))
                {
                    LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + ";离线通知接口请求失败");
                }

            }
        }
        #endregion

        #region 服务关闭事件
        private void StopService_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("手动停止服务器之后需要人工重启", "停止服务", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                boxServer.Stop();
                ShowLog(txtLog, "服务已停止！");
                this.Dispose();
                this.Close();
            }

        }
        #endregion

        #region Session关闭事件
        static void BoxProtocolServer_SessionClosed(BoxSession session, SuperSocket.SocketBase.CloseReason value)
        {
            LogHelper.WriteLog("---------------------------------------------------------------------------------------------------");
            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "sessionid：" + session.SessionID + ";mac:" + session.CustomId);
            LogHelper.WriteLog(session.RemoteEndPoint.ToString() + "连接断开. 断开原因:" + value);
            LogHelper.WriteLog("---------------------------------------------------------------------------------------------------");
        }
        #endregion

        #region Session链接事件
        static void BoxProtocolServer_NewSessionConnected(BoxSession session)
        {
           
        }
        #endregion

        #region Session请求事件
        private void BoxProtocolServer_NewRequestReceived(BoxSession session, BoxRequestInfo requestInfo)
        {
            //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "所有的mac：" + JsonConvert.SerializeObject(boxServer.GetAllSessions().Select(o => new { o.CustomId, o.CustomType, o.SessionID }).ToList()));
            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "当前session的ID：" + session.SessionID);
            //如果头命令不是EF03则关闭当前的session
            if (requestInfo.Body.Head.Contains("EF"))
            {

                session.CustomId = requestInfo.Body.Mac;
                session.Mac = requestInfo.Body.FormatMac;
                switch ((类型)requestInfo.Body.Type)
                {
                    case 类型.微信:
                        session.CustomType = 2;
                        LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "微信传过来的命令：" + requestInfo.Body.Command);
                        var command = BoxModel.ToCommand(requestInfo.Body);
                        LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "微信端口命令：" + Converts.GetTPandMac(command));
                        if (!OpenBoxByMac(session.CustomId, command, requestInfo.Body.OrderNo, 1))
                        {
                            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "已经开箱失败：");
                            //需要发送byte数组的命令，返回微信处理
                            //session.Send(JsonConvert.SerializeObject(new ResponseResult() { Status = false, ErrorCode = 01, Message ="箱子未连接"}));
                        }
                        LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "已经开箱成功：");
                        break;
                    case 类型.心跳:
                        session.CustomType = 1;
                        var macDt = DbHelperSQL.GetDataTableBySQL(string.Format("select top 1 * from WP_库位表 where 箱子MAC ='{0}'", session.Mac));
                        //如果没有查到信息，那么说明箱子在三代上
                        if (macDt.Rows.Count == 0)
                        {
                            var cache = (List<OnlineBox>)CacheHelper.GetCache("Boxes");
                            if (cache != null)
                            {
                                var mac = cache.FirstOrDefault(o => o.mac.Equals(session.Mac));
                                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + ";mac:" + mac);

                                if (mac != null)
                                {
                                    //如果之前是离线的，需要通知管理后台
                                    if (!mac.online)
                                    {
                                        var requestUrl = string.Format("{0}test/online?mac={1}", Constant.YunApi, mac.mac);
                                        var response = JsonConvert.DeserializeObject<BuyResponse>(Utils.HttpGet(requestUrl));
                                        LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + ";heartResponse:" + response);
                                        if (!response.operationStatus.Equals("SUCCESS"))
                                        {
                                            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + ";在线通知接口请求失败");
                                        }
                                    }
                                    mac.online = true;
                                    mac.lineTime = DateTime.Now;
                                    CacheHelper.SetCache("Boxes", cache);
                                }
                            }
                        }
                        else
                        {
                            //处理并存储心跳信息
                            SaveHeart(requestInfo.Body);
                            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "心跳命令：" + requestInfo.Body.Command);
                            //判断是否有异常开箱的情况，记录并反馈
                            ShowLog(txtLog, requestInfo.Body.ToString());
                        }
                        break;
                    case 类型.开箱:
                        session.CustomType = 1;
                        LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "开箱回复命令：" + requestInfo.Body.Command);
                        //发送微信反馈
                        SetOrderNo(session, requestInfo.Body.State);
                        ShowLog(txtLog, requestInfo.Body.ToString());
                        break;
                    default:
                        session.CustomType = 1;
                        break;
                }
            }
            //三代微信过来的信息
            else if (requestInfo.Body.Head.Contains("FF"))
            {
                session.CustomId = requestInfo.Body.Mac;

                switch ((类型)requestInfo.Body.Type)
                {
                    case 类型.微信:
                        session.CustomType = 2;
                        LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "微信传过来的命令：" + requestInfo.Body.Command);
                        var command = BoxModel.ToCommand(requestInfo.Body);
                        LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "微信端口命令：" + Converts.GetTPandMac(command));
                        if (!OpenBoxByMac(session.CustomId, command, requestInfo.Body.OrderNo, 1))
                        {
                            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "已经开箱失败：");
                            //需要发送byte数组的命令，返回微信处理
                            //session.Send(JsonConvert.SerializeObject(new ResponseResult() { Status = false, ErrorCode = 01, Message ="箱子未连接"}));
                        }
                        LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "开箱命令发送成功：");
                        break;
                    default:
                        session.CustomType = 1;
                        break;
                }
            }
            else
            {
                session.Close();
                return;
            }


        }
        #endregion

        #region -----方法-----
        public static void ShowLog(object objtxt, string message)
        {
            if (objtxt is RichTextBox)
            {
                RichTextBox txtLog = (RichTextBox)objtxt;
                txtLog.BeginInvoke(new EventHandler(delegate
                {
                    if (txtLog.Text.Length > 65535)
                    {
                        txtLog.Clear();
                    }
                    txtLog.AppendText(message);
                    txtLog.AppendText("\r\n");
                }));
            }
        }
        
        private enum 类型 : byte
        {
            微信 = 2,
            心跳 = 1,
            开箱 = 3
        }
        private void SetOrderNo(BoxSession session, byte[] states)
        {
            try
            {
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "allsessions:" + JsonConvert.SerializeObject(boxServer.GetAllSessions().Select(o=>o.CustomId).ToList()));
                var sessions = boxServer.GetSessions(o => o.CustomId.Equals(session.CustomId) && o.CustomType == 1);
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "sessions:" + JsonConvert.SerializeObject(sessions.Select(o => o.CustomId).ToList()));
                if (sessions.Count() != 0)
                {
                    var orderNo = sessions.First().OrderNo;
                    LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "orderNo:" + orderNo);
                    var errorPosition = string.Empty;
                    for (int i = 0; i < states.Length; i++)
                    {
                        byte state = states[i];
                        if (state != 0x00)
                        {
                            errorPosition += i + 1 + ",";
                        }
                    }
                    var suffix = orderNo.Substring(0, 1);
                    switch (suffix)
                    {
                        case "S":
                            UpdateOrder(errorPosition, orderNo);
                            break;
                        case "B":
                            UpdateBack(errorPosition, orderNo);
                            break;
                        case "J":break;
                        case "C":break;
                        default:
                            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "三版本:" );
                            ThreeUpdateOrder(errorPosition, orderNo);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + ex.Message);
            }
        }
        //修改订单
        private void UpdateOrder(string errorPosition,string orderNo)
        {
            var updateSql = string.Empty;
            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "异常箱子位置：" + errorPosition);
            if (!string.IsNullOrEmpty(errorPosition))
            {
                updateSql = string.Format(@"
UPDATE wp_订单子表 SET 是否开箱 = 1 WHERE 订单编号='{0}'
UPDATE wp_订单子表 SET 是否开箱 = 0 WHERE 订单编号='{0}' AND 位置 IN({1})
UPDATE wp_订单表 SET STATE=5 WHERE 订单编号='{0}'
", orderNo, errorPosition.TrimEnd(','));
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "开箱失败sql：" + updateSql);
                var b = DbHelperSQL.ExecuteSql(updateSql);
            }
            else
            {
                updateSql = string.Format(@"UPDATE wp_订单表 SET STATE=3,hasPush = 0 WHERE 订单编号='{0}'", orderNo);
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "开箱成功sql：" + updateSql);
                var b = DbHelperSQL.ExecuteSql(updateSql);
                var begin_exsql = " Begin Tran ";
                var exsql = string.Empty;
                var end_sql = @" If @@ERROR>0
                                                            Rollback Tran  
                                                        Else
                                                            Commit Tran
                                                        Go";
                string sub_order_sql = string.Format(@"select wp_订单子表.id,商品id,订单编号,wp_库位表.仓库id,
                                WP_商品表.编号,价格 as 单价,(SELECT ISNULL(仓库名,'') AS 仓库名 FROM WP_仓库表 WHERE id=wp_库位表.仓库id) as 仓库名,
                                (SELECT 酒店id FROM WP_仓库表 WHERE id=wp_库位表.仓库id) as 酒店id,库位id,wp_库位表.库位名,位置,数量,品名,箱子MAC 
                                 from wp_订单子表 
                                 left join  WP_商品表 on WP_商品表.id=wp_订单子表.商品id
                                 left join wp_库位表 on wp_订单子表.库位id= wp_库位表.id
                                 where 订单编号='{0}'", orderNo);
                string productNum = string.Empty;
                string kw_name = string.Empty;
                string hotel_name = string.Empty;
                var sub_order_dt = DbHelperSQL.GetDataTableBySQL(sub_order_sql);
                if (sub_order_dt.Rows.Count > 0)
                {
                    foreach (DataRow item in sub_order_dt.Rows)
                    {
                        productNum += item["编号"].ObjToStr() + "|";
                        var kuweiid = item["库位id"].ObjToInt(0);
                        var position = item["位置"].ObjToInt(0);
                        var num = item["数量"].ObjToInt(0);
                        var goods_id = item["商品id"].ObjToInt(0);
                        var price = item["单价"].ObjToDecimal(0);
                        //判断如果出库表没有，则插入出库表，更改箱子状态
                        string sql_outstock = string.Format(@"select 单据编号,位置 from wp_出库表 where 单据编号='{0}' and 位置='{1}'and 库位id='{2}' ", orderNo, position, kuweiid);
                        DataTable dt_outstock = DbHelperSQL.GetDataTableBySQL(sql_outstock);
                        if (dt_outstock.Rows.Count == 0)
                        {
                            exsql += string.Format(@" insert wp_出库表 (单据编号,商品id,数量,出价,总出价额,操作日期,库位id,位置,操作id,出库类型,IsShow)
                                             values('" + orderNo + "','" + goods_id + "','" + num + "','" + price + "','" + price + "',getdate(),'" + kuweiid + "','" + position + "',8,1,1)");
                        }
                        exsql += string.Format(" update WP_箱子表 set 实际商品id=0,售出时间=getdate() where 库位id={0} AND 位置 = {1}", kuweiid, position);


                    }
                    LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "支付完成之后的扣减库存：" + begin_exsql + exsql +
                                       end_sql);
                    DbHelperSQL.ExecuteSql(begin_exsql + exsql + end_sql);
                }
            }
        }

        //修改补货单
        private void UpdateBack(string errorPosition,string orderNo) {
            var updateSql = string.Format(@"UPDATE [dbo].[WP_补货单]
   SET [FailPosition] = '{0}'
      ,[Status] = {2}
 WHERE [OrderNo]= '{1}'", errorPosition, orderNo, string.IsNullOrEmpty(errorPosition) ? 2 : 3);
            DbHelperSQL.ExecuteSql(updateSql);
        }

        //三代系统修改订单
        private void ThreeUpdateOrder(string errorPosition, string orderNo)
        {
            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "errorPosition:" + errorPosition);
            if (string.IsNullOrEmpty(errorPosition))
            {
                var requestUrl = string.Format("{0}test/open?orderId={1}", Constant.YunApi, orderNo);
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "successRequestUrl:" + requestUrl);
                var response = JsonConvert.DeserializeObject<BuyResponse>(Utils.HttpGet(requestUrl));
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "successResponse:" + JsonConvert.SerializeObject(response));
            }
            else
            {
                var requestUrl = string.Format("{0}test/fail?orderId={1}", Constant.YunApi, orderNo);
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "failRequestUrl:" + requestUrl);
                var response = JsonConvert.DeserializeObject<BuyResponse>(Utils.HttpGet(requestUrl));
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "failResponse:" + JsonConvert.SerializeObject(response));
            }
        }

        private void SaveHeart(BoxModel model)
        {
            var errorPosition = string.Empty;
            for (int i = 0; i < model.State.Length; i++)
            {
                byte state = model.State[i];
                if (state == 0x05)
                {
                    errorPosition += i + 1 + ",";
                }
            }
            errorPosition = errorPosition.TrimEnd(',');
            var setErrorSql = string.Empty;
            if (!string.IsNullOrEmpty(errorPosition))
            {
                setErrorSql = string.Format(@"  update WP_箱子表 set BoxStatus = 1 where 库位id in(
  select top 1 id from WP_库位表 where 箱子MAC = '{0}') and 位置 in({1})", model.FormatMac, errorPosition);
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "异常开箱MAC：" + model.FormatMac + ";异常位置：" + errorPosition);
            }

            LogHelper.SaveHeart(model.FormatMac, model.Command);
            var sql = string.Format(@"
declare @cou int
select @cou =count(id)  from WP_设备心跳记录表 where  mac='{0}'
if @cou =0
begin
insert into WP_设备心跳记录表(mac,command) values('{0}','{1}')
end
else
begin
update WP_设备心跳记录表 set  command = '{1}',createtime = getdate() where mac = '{0}'
UPDATE [WP_库位表] SET [状态] = 1,修改时间 = getdate(),离线时长 = 0,版本号='{3}' WHERE 箱子MAC = '{0}'
update WP_BarCode set Version = '{3}' where BarCode ='{0}'
end 
{2}
", model.FormatMac, model.Command, setErrorSql, Converts.GetTPandMac(model.Placeholder));
            try
            {
                var b = DbHelperSQL.ExecuteSql(sql);
            }
            catch (Exception ex2)
            {
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + ex2.Message);
            }
        }



        #region 根据mac地址发送开命令
        private bool OpenBoxByMac(string mac, byte[] command, string orderNo, int type)
        {
            try
            {
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "-准备进行开箱子：");
                //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "所有的mac：" + JsonConvert.SerializeObject(boxServer.GetAllSessions().Select(o => new { o.CustomId, o.CustomType, o.SessionID }).ToList()));
                var list = boxServer.GetAllSessions().Where(o => o.CustomId.Equals(mac) && o.CustomType == type).ToList();
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "-符合条件的箱子数量：" + list.Count);
                List<BoxSession> currentSessionList =
                    boxServer.GetAllSessions().Where(o => o.CustomId.Equals(mac) && o.CustomType == type).ToList();

                //BoxSession currentSession = boxServer.GetSessions(o => o.CustomId.Equals(mac) && o.CustomType == type).FirstOrDefault();
                //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "-当前要开箱的mac：" + JsonConvert.SerializeObject(currentSession));
                if (currentSessionList.Count > 0)
                {
                    foreach (var currentSession in currentSessionList)
                    {
                        currentSession.Send(command, 0, command.Length);
                        currentSession.OrderNo = orderNo;
                       
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "-开箱异常：" + ex.Message + ";异常位置：" + ex.StackTrace);
                throw;
            }

        }
        #endregion
        #endregion

        #region 显示当前连接数
        private void CountTicket_Tick(object sender, EventArgs e)
        {
            WeChartConnectCount.Text = boxServer.GetAllSessions().Count(o => o.CustomType == 2).ToString();
            DevConnectCount.Text = boxServer.GetAllSessions().Where(o => o.CustomType == 1).GroupBy(o => o.CustomId).Count().ToString();
        }
        #endregion

        #region 显示窗体事件
        private void Show_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
        #endregion

        #region 退出程序
        private void Exit_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("手动停止服务器之后需要人工重启", "停止服务", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                boxServer.Stop();
                ShowLog(txtLog, "服务已停止！");
                this.Dispose();
                this.Close();
            }
        }
        #endregion

        #region Form窗体关闭事件

        private void BoxForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
            {
                e.Cancel = true;    //取消"关闭窗口"事件
                this.WindowState = FormWindowState.Minimized;    //使关闭时窗口向右下角缩小的效果
                MinWindow.Visible = true;
                this.Hide();
                return;
            }
        }
        #endregion

        #region 双击最小图标,显示正常窗体
        private void MinWindow_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
        #endregion

        #region 清理日志
        private void clear_Click(object sender, EventArgs e)
        {
            txtLog.Text = string.Empty;
        }
        #endregion




    }
}

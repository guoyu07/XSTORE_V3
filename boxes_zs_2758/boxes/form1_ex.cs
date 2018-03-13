using boxes.Common;
using boxes.TCP;
using boxes.WebSocekt;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace boxes
{
    public partial class Form1
    {
        public const bool ISTest = true;
        private  void  ShowLog(string message)
        {
            //LogHelper.showLog(txtLog, message);
           LogHelper.WriteLog(message);
        }


        class ClientInfo
        {
            public TcpClient myclient;
            public string StrMac;
            /// <summary>
            /// 标记连接类别，1代表客户端（微信，测试客户端），2代表设备
            /// </summary>
            public int typeid = 0;
            /// <summary>
            /// 确认设备是活着的
            /// </summary>
            /// <returns></returns>
            public bool IsAlive()
            {
                bool result = false;
                if (myclient != null && myclient.Connected && string.IsNullOrEmpty(StrMac))
                {
                    result = true;
                }
                return result;
            }
            public string GetStrRemoteEndPoint
            {
                get
                {
                    string strRemoteEndPoint = string.Empty;
                    try
                    {
                        strRemoteEndPoint = myclient.Client.RemoteEndPoint.ToString();
                    }
                    catch
                    {

                    }

                    return strRemoteEndPoint;
                }
            }




            public void SendMsg(string command)
            {

            }
            public bool SendMsg(byte[] command)
            {
                bool result = false;
                try
                {
                    result = server.Send(myclient, command);
                }
                catch
                {
                    result = false;

                }
                return result;
            }
            DateTime firstTime = DateTime.Now;
            public DateTime FirstTime
            {
                get { return firstTime; }
            }
            /// <summary>
            /// 判定是否为同一条连接信息
            /// </summary>
            /// <param name="cltobj"></param>
            /// <param name="cltsour"></param>
            /// <returns></returns>


            //public static bool operator ==(ClientInfo cltobj, ClientInfo cltsour)
            //{
            //    bool result = false;
            //    if (cltobj.myclient.Equals(cltsour.myclient) && cltobj.StrMac.Equals(cltsour.StrMac))
            //    {
            //        result = true;
            //    }
            //    return result;
            //}

            //public static bool operator !=(ClientInfo cltobj, ClientInfo cltsour)
            //{
            //    bool result = true;

            //    if (cltobj.myclient.Equals(cltsour.myclient) && cltobj.StrMac.Equals(cltsour.StrMac))
            //    {
            //        result = false;
            //    }
            //    return result;
            //}

            /// <summary>
            /// 确认某类型的Client的[TCP,mac]组合是否与当前类型的[TCP,mac]组合相同，如果该类型的Client的MAC已存在,更新TCP信息【MAC不动】
            /// </summary>
            /// <param name="_tcpClient"></param>
            /// <param name="_strmac"></param>
            /// <returns></returns>
            public bool CheckExist(TcpClient _tcpClient, string _strmac)
            {
                bool result = false;
                if (myclient != null && StrMac.Equals(_strmac) && _tcpClient != null && _tcpClient.Connected)
                {

                    myclient = _tcpClient;
                    if (typeid == 1)
                    {
                        //LogHelper.WriteLog("客户端更新SOCKET与MAC的组合。MAC：" + _strmac + ";SOCKET：" + _tcpClient.Client.RemoteEndPoint.ToString());
                    }
                    if (typeid == 2)
                    {
                        //LogHelper.WriteLog("设备更新SOCKET与MAC的组合。MAC：" + _strmac + ";SOCKET：" + _tcpClient.Client.RemoteEndPoint.ToString());
                    }
                }

                if ((myclient != null && !string.IsNullOrEmpty(StrMac)) && myclient.Equals(_tcpClient) && StrMac.Equals(_strmac))
                {
                    result = true;
                }
                return result;
            }


            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                if (typeid == 1)
                {
                    sb.Append("【客户端】");
                }
                else
                {
                    if (typeid == 2)
                    {
                        sb.Append("【货箱设备】");
                    }
                    else
                    {
                        sb.Append("【未知类型】");
                    }
                }
                string strRemoteEndPoint = string.Empty;
                try
                {
                    strRemoteEndPoint = myclient.Client.RemoteEndPoint.ToString();
                }
                catch
                {

                }
                sb.Append(" socket is " + strRemoteEndPoint + "; Mac is " + StrMac);
                return sb.ToString();
            }
        }
        /// <summary>
        /// 微信与测试客户端
        /// </summary>
        class WeiXinClient : ClientInfo
        {
            /// <summary>
            /// 16进制形式的字节数组，用的时候转换回去。
            /// </summary>
            public string command;

            /// <summary>
            /// 发送的命令数
            /// </summary>
            public int sendnum;

            /// <summary>
            /// 收到的响应数
            /// </summary>
            public int receivenum;

            /// <summary>
            /// 收到足够的响应就回送数据到微信
            /// </summary>
            /// <returns></returns>
            public bool MaySend()
            {
                if (receivenum > 0 && receivenum == sendnum)
                {
                    return true;
                }
                return false;

            }
        }

        /// <summary>
        /// 硬件设备类
        /// </summary>
        class DeviceClient : ClientInfo
        {
            /// <summary>
            /// 例如:"0202020202"，全开
            /// </summary>
            public string BoxState;
            /// <summary>
            /// 上一次心跳包
            /// </summary>
            public string LastBeat;
            /// <summary>
            /// 上一次开箱响应内容
            /// </summary>
            public string LastCommanResponse;
            DateTime lastTime = DateTime.Now;
            public DateTime LastTime
            {
                set
                {
                    lastTime = value;
                }
                get { return lastTime; }
            }
            /// <summary>
            /// 在线离线状态，默认离线【false】
            /// </summary>
            public bool OnLine = false;


            public void SendOffLineMsg()
            {
                if (OnLine)//处于在线状态
                {
                    string urlnew = APIURL.API_OFFLine + StrMac;
                    if (ISTest)
                    {
                        urlnew = APIURL.API_OFFLine_Test + StrMac;
                    }
                    //LogHelper.WriteLog("发送离线消息，API访问地址：" + urlnew);
                    int i = 1;
                    try
                    {
                        while (true)
                        {
                            string result1 = HttpHelper.HttpPost(urlnew, null, "application/json");
                            if (result1.Contains("房间不存在"))
                            {
                                OnLine = false;
                                //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "该房间不存在，MAC IS " + StrMac);
                                break;
                            }
                            APIMSG msg = JsonConvert.DeserializeObject<APIMSG>(result1);
                            //LogHelper.WriteLog(result1);
                            if (msg.success == true || i > 3)
                            {
                                if (msg.success)
                                {
                                    OnLine = false;
                                    //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "设备离线成功：MAC IS " + StrMac);
                                }

                                break;
                            }
                            i++;
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "调用接口异常：" + e.Message + "接口是:" + urlnew);
                    }
                }

            }


            class APIURL
            {
                public static readonly string API_ONLine_Test = "http://x.x-store.com.cn/api/services/app/room/SetRoomStateToOnLine?serialNumber=";

                public static readonly string API_OFFLine_Test = "http://x.x-store.com.cn/api/services/app/room/SetRoomStateToOutLine?serialNumber=";

                public static readonly string API_ONLine = "http://wx.x-store.com.cn/api/services/app/room/SetRoomStateToOnLine?serialNumber=";

                public static readonly string API_OFFLine = "http://wx.x-store.com.cn/api/services/app/room/SetRoomStateToOutLine?serialNumber=";
            }
            public void SendONLineMsg()
            {
                if (!OnLine)
                {
                    string urlnew = APIURL.API_ONLine + StrMac;
                    if (ISTest)
                    {
                        urlnew = APIURL.API_ONLine_Test + StrMac;
                    }
                    //LogHelper.WriteLog("发送上线消息,API访问地址：" + urlnew);
                    int i = 1;
                    try
                    {
                        while (true)
                        {
                            string result1 = HttpHelper.HttpPost(urlnew, null, "application/json");
                            if (result1.Contains("房间不存在"))
                            {
                                OnLine = true;
                                //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "MAC为" + StrMac + "的房间不存在");
                                break;
                            }
                            else
                            {
                                APIMSG msg = JsonConvert.DeserializeObject<APIMSG>(result1);
                                //LogHelper.WriteLog(result1);
                                if (msg.success == true || i > 3)
                                {
                                    if (msg.success)
                                    {
                                        OnLine = true;
                                        //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "设备上线成功：MAC IS " + StrMac);
                                    }
                                    else
                                        //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "设备上线失败第" + i + "次");
                                    break;
                                }
                                i++;
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "调用接口异常：" + e.Message + "接口是:" + urlnew);
                    }
                }

            }
        }

        /// <summary>
        ///两条列表，存放两种类别的链接，方便迅速查找
        /// </summary>
        class ClientList
        {//用泛型？？
            /// <summary>
            /// 客户端信息列表
            /// </summary>
            List<WeiXinClient> LstWeiXin = new List<WeiXinClient>();
            /// <summary>
            /// 设备信息列表
            /// </summary>
            List<DeviceClient> LstDev = new List<DeviceClient>();
            public delegate void ClientInfo_Print(string mssage);
            public  static ClientInfo_Print ctPrint;
            public  void PrintAll()
            {
                if (ctPrint != null)
                {
                    ctPrint("-------------------Socket Info ：-------------------------------");
                    foreach (ClientInfo ct in LstWeiXin)
                    {
                        ctPrint(ct.ToString());
                    }
                    foreach (ClientInfo ct in LstDev)
                    {
                        ctPrint(ct.ToString());
                    }
                    ctPrint("-------------------"+DateTime.Now.ToString("HH:mm:ss")+"：-------------------------------");
                }
            }

            public int GetWeiXinNumber()
            {
                if (LstWeiXin != null)
                    return LstWeiXin.Count;
                else
                    return 0;
            }
            public int GetDevCount()
            {
                if (LstDev != null)
                {
                    //if (LstDev.Count > 0)
                    //{
                    //    LogHelper.WriteLog("Form1_LstDev:" + JsonConvert.SerializeObject(LstDev));
                    //}
                    return LstDev.Count;
                }
                else
                    return 0;
            }

            public void Clear()
            {
                LstWeiXin.Clear();
                LstDev.Clear();
            }
            /// <summary>
            /// 传入mac与类型，得到链接信息.传说中的索引器
            /// </summary>
            /// <param name="strmac"></param>
            /// <param name="type">1代表微信端，2代表设备</param>
            /// <returns></returns>
            public ClientInfo this[string strmac, int type]
            {
                get
                {
                    if (type == 1)
                    {
                        WeiXinClient tmpwx = null;
                        foreach (WeiXinClient wx in LstWeiXin)
                        {
                            if (wx.StrMac.Equals(strmac))
                            {
                                tmpwx = wx;
                                break;
                            }
                            else
                                continue;
                        }
                        return tmpwx;
                    }
                    else
                    {
                        DeviceClient tmpwx = null;
                        foreach (DeviceClient wx in LstDev)
                        {
                            if (wx.StrMac.Equals(strmac))
                            {
                                tmpwx = wx;
                                break;
                            }
                            else
                                continue;
                        }
                        return tmpwx;
                    }

                }

            }

            public void RemoveClient(TcpClient tcpclt)
            {
                try
                {
                    WeiXinClient tmpwx = new WeiXinClient();
                    foreach (WeiXinClient wx in LstWeiXin)
                    {
                        if (wx.myclient.Equals(tcpclt))
                        {
                            tmpwx = wx;
                        }
                    }
                    if (LstWeiXin.Contains(tmpwx))
                    {
                        string endpoint = tmpwx.GetStrRemoteEndPoint;
                        tmpwx.myclient.ShutDownAndClose();
                        LstWeiXin.Remove(tmpwx);
                        //LogHelper.WriteLog("微信端" + DateTime.Now.ToString("HH:mm:ss") + endpoint + "-->断开连接");


                    }

                    DeviceClient tmpdev = new DeviceClient();
                    foreach (DeviceClient dev in LstDev)
                    {
                        if (dev.myclient.Equals(tcpclt))
                        {
                            tmpdev = dev;
                        }
                    }
                    if (LstDev.Contains(tmpdev))
                    {
                        //再此判断，满足条件再移除，无论在哪调用都要判断
                        TimeSpan tspan = new TimeSpan();
                        tspan = DateTime.Now - tmpdev.LastTime;
                        if (tspan.TotalSeconds > 600)
                        {
                            string endpoint = tmpwx.GetStrRemoteEndPoint;
                            //LogHelper.WriteLog("MAC地址为" + tmpdev.StrMac + "的设备于" + DateTime.Now.ToString("HH:mm:ss") + endpoint + "-->断开连接");
                            //tmpdev.SendOffLineMsg();//检测到断开连接，发送离线
                            tmpdev.myclient.ShutDownAndClose();
                            LstDev.Remove(tmpdev);
                            
                        }
                    }
                }
                catch (Exception e)
                {
                    LogHelper.WriteLog("程序错误" + e.Message + ";" + e.StackTrace);
                }

            }


            /// <summary>
            /// 传入一个连接，类型相同的并且MAC相同的ClientInfo存在就更新，不存在就加入相应的队列。设备上线并广播出去;微信端重新计数
            /// </summary>
            /// <param name="cltinfo"></param>
            public bool AddClient(ClientInfo cltinfo)
            {
                bool isAdd = false;
                if (cltinfo is WeiXinClient)
                {
                    WeiXinClient cltwx = (WeiXinClient)cltinfo;
                    bool exist = false;

                    foreach (WeiXinClient wx in LstWeiXin)
                    {
                        try
                        {
                            bool isneedupdate = false;
                            if (wx.StrMac.Equals(cltwx.StrMac))
                            {
                                if (wx.myclient == null || !wx.myclient.Connected)
                                {
                                    isneedupdate = true;
                                }

                                if (!wx.GetStrRemoteEndPoint.Equals(cltwx.GetStrRemoteEndPoint))
                                {
                                    isneedupdate = true;
                                }
                                if (isneedupdate)
                                {
                                    wx.myclient = cltwx.myclient;
                                    //LogHelper.WriteLog("Client Update [TCP-MAC],Now is " + wx.StrMac + "--" + wx.GetStrRemoteEndPoint);
                                }
                                exist = true;
                            }
                        }

                        catch (Exception ex)
                        {
                            string messages = string.Empty;
                            messages = "捕获异常" + ex.Message + "错误发生位置为" + ex.StackTrace;
                            //LogHelper.WriteLog(messages);
                        }
                    }
                    if (!exist)
                    {  //应该在服务器第一次收到命令时加入
                        isAdd = true;
                        //LogHelper.WriteLog("Client ADD [TCP-MAC],Now is " + cltwx.StrMac + "--" + cltwx.GetStrRemoteEndPoint);
                         
                        LstWeiXin.Add(cltwx);
                    }
                }
                else
                {
                    if (cltinfo is DeviceClient)
                    {
                        DeviceClient cltdc = (DeviceClient)cltinfo;//可以转换的
                        bool exist = false;
                        foreach (DeviceClient dc in LstDev)
                        {
                            try
                            {
                                bool isneedupdate = false;
                                if (dc.StrMac.Equals(cltdc.StrMac))
                                {
                                    if (dc.myclient == null || !dc.myclient.Connected)
                                    {
                                        isneedupdate = true;
                                    }

                                    if (!dc.GetStrRemoteEndPoint.Equals(cltdc.GetStrRemoteEndPoint))
                                    {
                                        isneedupdate = true;
                                    }
                                    if (isneedupdate)
                                    {
                                        dc.myclient = cltdc.myclient;
                                        dc.LastBeat = cltdc.LastBeat;
                                        dc.LastTime = cltdc.LastTime;
                                        dc.LastCommanResponse = cltdc.LastCommanResponse;
                                        dc.BoxState = cltdc.BoxState;
                                        dc.SendONLineMsg();
                                        //LogHelper.WriteLog("Device Update [TCP-MAC],Now is " + dc.StrMac + "--" + dc.GetStrRemoteEndPoint);
                                    }
                                    exist = true;
                                }

                            }
                            catch (Exception ex)
                            {
                                string messages = string.Empty;
                                messages = "捕获异常" + ex.Message + "错误发生位置为" + ex.StackTrace;
                                //LogHelper.WriteLog(messages);
                            }
                        }
                        if (!exist)
                        {
                            isAdd = true;
                            LstDev.Add(cltdc);
                            LogHelper.WriteLog("Device ADD [TCP-MAC],Now is " + cltdc.StrMac + "--" + cltdc.GetStrRemoteEndPoint);
                            cltdc.SendONLineMsg();
                            //通知所有客户端有设备连接上来
                            try
                            {
                                server.SendAll(cltdc.StrMac + ";" + cltdc.GetStrRemoteEndPoint);
                            }
                            catch (Exception ex)
                            {
                                string messages = string.Empty;
                                messages = "捕获异常" + ex.Message + "错误发生位置为" + ex.StackTrace;
                                //LogHelper.WriteLog(messages);
                            }
                        }
                    }
                }
                return isAdd;
            }

            public bool AddClientWithOutCmd(ClientInfo cltinfo)
            {
                bool isAdd = false;
                if (cltinfo is WeiXinClient)
                {
                    WeiXinClient cltwx = (WeiXinClient)cltinfo;
                    bool exist = false;

                    foreach (WeiXinClient wx in LstWeiXin)
                    {
                        try
                        {
                            bool isneedupdate = false;
                            if (wx.StrMac.Equals(cltwx.StrMac))
                            {
                                if (wx.myclient == null || !wx.myclient.Connected)
                                {
                                    isneedupdate = true;
                                }

                                if (!wx.GetStrRemoteEndPoint.Equals(cltwx.GetStrRemoteEndPoint))
                                {
                                    isneedupdate = true;
                                }
                                if (isneedupdate)
                                {
                                    wx.myclient = cltwx.myclient;
                                    //LogHelper.WriteLog("Client Update [TCP-MAC],Now is " + wx.StrMac + "--" + wx.GetStrRemoteEndPoint);
                                }
                                exist = true;
                            }
                        }

                        catch (Exception ex)
                        {
                            string messages = string.Empty;
                            messages = "捕获异常" + ex.Message + "错误发生位置为" + ex.StackTrace;
                            //LogHelper.WriteLog(messages);
                        }
                    }
                    if (!exist)
                    {  //应该在服务器第一次收到命令时加入
                        isAdd = true;
                        //LogHelper.WriteLog("Client ADD [TCP-MAC],Now is " + cltwx.StrMac + "--" + cltwx.GetStrRemoteEndPoint);
                        LstWeiXin.Add(cltwx);
                    }
                }
                else
                {
                    if (cltinfo is DeviceClient)
                    {
                        DeviceClient cltdc = (DeviceClient)cltinfo;//可以转换的
                        bool exist = false;
                        foreach (DeviceClient dc in LstDev)
                        {
                            try
                            {
                                bool isneedupdate = false;
                                if (dc.StrMac.Equals(cltdc.StrMac))
                                {
                                    if (dc.myclient == null || !dc.myclient.Connected)
                                    {
                                        isneedupdate = true;
                                    }

                                    if (!dc.GetStrRemoteEndPoint.Equals(cltdc.GetStrRemoteEndPoint))
                                    {
                                        isneedupdate = true;
                                    }
                                    if (isneedupdate)
                                    {
                                        dc.myclient = cltdc.myclient;
                                        dc.LastBeat = cltdc.LastBeat;
                                        dc.LastTime = cltdc.LastTime;
                                        dc.LastCommanResponse = cltdc.LastCommanResponse;
                                        dc.BoxState = cltdc.BoxState;
                                        dc.SendONLineMsg();
                                        //LogHelper.WriteLog("Device Update [TCP-MAC],Now is " + dc.StrMac + "--" + dc.GetStrRemoteEndPoint);
                                    }
                                    exist = true;
                                }

                            }
                            catch (Exception ex)
                            {
                                string messages = string.Empty;
                                messages = "捕获异常" + ex.Message + "错误发生位置为" + ex.StackTrace;
                                //LogHelper.WriteLog(messages);
                            }
                        }
                        if (!exist)
                        {
                            isAdd = true;
                            LstDev.Add(cltdc);
                            LogHelper.WriteLog("Device ADD [TCP-MAC],Now is " + cltdc.StrMac + "--" + cltdc.GetStrRemoteEndPoint);
                            cltdc.SendONLineMsg();
                            //通知所有客户端有设备连接上来
                            //try
                            //{
                            //    server.SendAll(cltdc.StrMac + ";" + cltdc.GetStrRemoteEndPoint);
                            //}
                            //catch (Exception ex)
                            //{
                            //    string messages = string.Empty;
                            //    messages = "捕获异常" + ex.Message + "错误发生位置为" + ex.StackTrace;
                            //    //LogHelper.WriteLog(messages);
                            //}

                        }
                    }
                }
                return isAdd;
            }
            /// <summary>
            /// 在合适的时机批量离线，移除过时连接
            /// </summary>
            public void MultiSendOffLine()
            {
                List<DeviceClient> lstdev = new List<DeviceClient>();
                foreach (DeviceClient dev in LstDev)
                {
                    if (dev.OnLine)
                    {
                        TimeSpan tspan = new TimeSpan();
                        tspan = DateTime.Now - dev.LastTime;
                        if (tspan.TotalSeconds > 600)
                        {
                            try
                            {
                                dev.SendOffLineMsg();
                            }
                            catch
                            {

                            }
                            finally
                            {
                                lstdev.Add(dev);
                            }
                        }
                    }
                }
                foreach (DeviceClient ldev in lstdev)
                {
                    if (ldev.myclient != null)
                    {
                        //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "移除过时连接" + ldev.GetStrRemoteEndPoint);
                        RemoveClient(ldev.myclient);
                    }
                }
            }

        }
        #region 格式转换相关的类
        private void Test()
        {
            byte[] data = { 0x3e, 0xfc, 0x23, 0xef };
            string ss = ByteToHexString(data);//3EFC23EF
            string bb = ByteToHexStringWith0X(data);//0X3E 0XFC 0X23 0XEF
            byte[] datanew = HexStringToByte(ss);//"3efc23ef"转成{62,252,35,239},即{ 0x3e,0xfc,0x23,0xef};//值是一样的，只是表示的进制不同
            byte[] data2 = System.Text.UTF8Encoding.ASCII.GetBytes("I Love You");//{73,32,76,111,118,101,32,89,111,117}得到对应的ASCII码值。
            string str = System.Text.UTF8Encoding.ASCII.GetString(data2);//还原为I Love You
        }
        /// <summary>
        /// 字节数组转换成大写的一组16进制形式的字符串。比如：{0x2C,0xD7}转换成"2CD7"
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ByteToHexString(byte[] data)
        {
            StringBuilder dataString = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                dataString.AppendFormat("{0:x2}", data[i]);
            }
            return dataString.ToString().Trim().ToUpper();
        }
        //private string findStr(string hexstring) {
        //    var byte_list = HexStringToByte(hexstring);
        //    return ByteToHexStringWith0X(byte_list);
        //}
        /// <summary>
        /// 字节数组转换成大写的一组16进制形式的字符串。比如：{0x2C,0xD7}转换成"0X2C 0XD7"，加了分隔符方便查阅
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ByteToHexStringWith0X(byte[] data)
        {
            StringBuilder dataString = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                dataString.AppendFormat("0X{0:x2} ", data[i]);
            }
            return dataString.ToString().Trim().ToUpper();
        }
        /// <summary>
        /// 大写的一组16进制形式的字符串转成字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private byte[] HexStringToByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 将string转换成指定编码格式的数组。目前支持HEX,ASCII,UTF8,GB2312。HEX需以空格隔开
        /// </summary>
        /// <param name="_EncodeType"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        byte[] ByteToStringWithEncoding(DataEncode _EncodeType, string str)
        {
            byte[] data = null;
            switch (_EncodeType)
            {
                case DataEncode.Hex:
                    string[] HexStr = str.Trim().Split(' ');
                    data = new byte[HexStr.Length];
                    for (int i = 0; i < HexStr.Length; i++)
                    {
                        data[i] = (byte)(Convert.ToInt32(HexStr[i], 16));
                    }
                    break;
                case DataEncode.ASCII:
                    data = new ASCIIEncoding().GetBytes(str);
                    break;
                case DataEncode.UTF8:
                    data = new UTF8Encoding().GetBytes(str);
                    break;
                case DataEncode.GB2312:
                    data = Encoding.GetEncoding("GB2312").GetBytes(str);
                    break;
                default: break;
            }
            return data;
        }

        /// <summary>
        /// 将指定编码格式的数组转换成string。目前支持HEX,ASCII,UTF8,GB2312。HEX以空格隔开
        /// </summary>
        /// <param name="_EncodeType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        string StringToByteWithEncoding(DataEncode _EncodeType, byte[] data)
        {
            string strResult = "";
            switch (_EncodeType)
            {
                case DataEncode.Hex:
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sb.AppendFormat("{0:x2} ", data[i]);
                    }
                    strResult = sb.ToString().Trim().ToUpper();
                    break;
                case DataEncode.ASCII:
                    strResult = new ASCIIEncoding().GetString(data);
                    break;
                case DataEncode.UTF8:
                    strResult = new UTF8Encoding().GetString(data);
                    break;
                case DataEncode.GB2312:
                    strResult = Encoding.GetEncoding("GB2312").GetString(data);
                    break;
            }
            return strResult;
        }
        public enum DataEncode
        {
            Hex,
            ASCII,
            UTF8,
            GB2312
        }
        #endregion

        #region 处理连包
        /// <summary>
        ///找出指定字符串出现的次数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sourcestr"></param>
        /// <returns></returns>
        static int GetStrAppearTimes(string str, string sourcestr)
        {
            Regex regex = new Regex(str, RegexOptions.IgnoreCase);
            var mymatch = regex.Matches(sourcestr);
            return mymatch.Count;
        }
        /// <summary>
        /// 取出以指定字符串开头的字符串的最后一个子串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strsource"></param>
        /// <returns></returns>
        string GetLastStrToEnd(string str, string strsource)
        {
            string result = "";
            try
            {
                int lastlocation = strsource.LastIndexOf(str);
                result = strsource.Substring(lastlocation);
            }
            catch
            {
            }
            return result;// Encoding.ASCII.ToString();
        }
        #endregion

        #region 服务器初始化,设备先全部下线
        private void ALLOffLine()
        {
            try
            {
                string strUrl = "http://wx2.x-store.com.cn/api/services/app/room/SetAllRoomStateToOutline";
                if (ISTest)
                    strUrl = "http://x.x-store.com.cn/api/services/app/room/SetAllRoomStateToOutline";

                //LogHelper.WriteLog("调用设备批量下线接口:" + strUrl);
                string result1 = HttpHelper.HttpPost(strUrl, "");
                //LogHelper.WriteLog("返回值为：" + result1);
            }
            catch (Exception e)
            {
                //LogHelper.WriteLog("调用设备批量下线接口异常" + e.Message);
            }
        }
        #endregion

    }
}

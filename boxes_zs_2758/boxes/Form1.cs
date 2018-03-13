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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using boxes.DBUtility;
namespace boxes
{
    public partial class Form1 : Form
    {

        static AsyncTcpServer server;


        /// <summary>
        /// 存放所有的设备与客户端信息
        /// </summary>
        ClientList CurrentClient = new ClientList();

        public Form1()
        {
            InitializeComponent();
            startserver();
            timer1.Enabled = true;
        }
        /// <summary>
        /// 服务器启动，核心部分
        /// </summary>
        void startserver()
        {
            ClientList.ctPrint += ShowLog;
            try
            {
                ALLOffLine();
                server = new AsyncTcpServer(2758);//-----端口号改成： 2758-----
                server.Encoding = Encoding.ASCII;
                server.ClientConnected += new EventHandler<TcpClientConnectedEventArgs>(server_ClientConnected);
                server.ClientDisconnected += new EventHandler<TcpClientDisconnectedEventArgs>(server_ClientDisconnected);
                server.DatagramReceived += new EventHandler<TcpDatagramReceivedEventArgs<byte[]>>(server_DatagramReceived);
                server.Start();
            }
            catch (Exception)
            {
                txtLog.Text += "服务器启动失败";
            }
        }

        /// <summary>
        /// 以byte[]传递数组// 这里可以作为普通数据的解析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void server_DatagramReceived(object sender, TcpDatagramReceivedEventArgs<byte[]> e)
        {
            try
            {
                //Thread thrd = new Thread(ExecuReceive);
                ThreadPool.QueueUserWorkItem(ExecuReceive);
                //thrd.Start(e);
                //thrd.Join();
            }
            catch (Exception eww)
            {
                txtLog.Text += "\r\n 程序错误：" + eww.Message + "\r\n" + eww.StackTrace;
            }
        }

        string GetMacFromCommand(byte[] data)
        {
            string strmac = string.Empty;
            try
            {
                byte[] byte_MacCurrent = new byte[12];
                Array.Copy(data, 10, byte_MacCurrent, 0, 12);
                strmac = ByteToHexString(byte_MacCurrent);
            }
            catch
            {
                return "";
            }
            return strmac.ToUpper();

        }


        void ExecuReceive(object data)
        {
            lock (CurrentClient)
            {
                AutoResetEvent exitEvent;
                exitEvent = new AutoResetEvent(false);
                //List<byte[]> mac_list = new List<byte[]>();
                //bool open = true;
                TcpDatagramReceivedEventArgs<byte[]> e = (TcpDatagramReceivedEventArgs<byte[]>)data;
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "服务器接收的基础数据：" + data);
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "；服务器收到来自设备的数据：" + Converts.GetTPandMac(e.Datagram) + ";");
                int lengths = e.Datagram.Length;//接受客户端握手信息
                string str_mac = "";
                string str_boxdate = "";
                //byte[] boxdate = new byte[6];
                //byte[] mac = new byte[12];
                byte[] boxdate = new byte[12];
                byte[] mac = new byte[15];
                //byte[] macnew = new byte[13];
                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "服务器命令头：" + e.Datagram[0]);
                switch (e.Datagram[0])
                {
                    //第一个字节是EF
                    case 239://主机返回的数据
                        {
                            #region 发送数据到客户端
                            //Array.Copy(e.Datagram, 10, mac, 0, 12);
                            //Array.Copy(e.Datagram, 22, boxdate, 0, 6);
                            try
                            {
                                Array.Copy(e.Datagram, 10, mac, 0, 15);
                                Array.Copy(e.Datagram, 25, boxdate, 0, 12);
                                str_mac = Converts.GetTPandMac(mac);
                                str_boxdate = Converts.GetTPandMac(boxdate);
                                DeviceClient dev = new DeviceClient();
                                dev.BoxState = str_boxdate;
                                dev.myclient = e.TcpClient;//请求端
                                dev.OnLine = false;
                                dev.StrMac = str_mac;
                                dev.typeid = 2;
                                //byte[] LastRes = HexStringToByte(GetLastStrToEnd("EF02", Converts.GetTPandMac(e.Datagram)));
                                byte[] LastRes = HexStringToByte(GetLastStrToEnd("EF03", Converts.GetTPandMac(e.Datagram)));
                                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "；服务器收到来自设备" + str_mac + "的数据：" + Converts.GetTPandMac(e.Datagram) + ";");
                                //showLog(DateTime.Now.ToString("HH:mm:ss") + "；服务器收到来自设备" + str_mac + "的数据：" + Converts.GetTPandMac(e.Datagram) + ";");
                                if (!CheckDataGram(LastRes))
                                {
                                    //byte[] box_number = new byte[] { 0, 0, 0, 0, 0, 0 };
                                    byte[] box_number = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                                    byte[] byteCommand = GetSendMulty(mac, box_number, 0x06);
                                    string sss = ByteToHexString(byteCommand);
                                    //暂时注释掉,回复指令
                                    //int j = 0;
                                    //while (j < 3)
                                    //{
                                    //    try
                                    //    {
                                    //        Thread.Sleep(750);
                                    //        LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "向目标：" + dev.GetStrRemoteEndPoint + "；向MAC为" + str_mac + "发送重发要求" + sss);
                                    //        dev.SendMsg(byteCommand);
                                    //        break;
                                    //    }
                                    //    catch { j++; }
                                    //}
                                }
                                else
                                {
                                    if (e.Datagram[9] == 0x03)//开箱指令响应  
                                    {
                                        string strLastCommanResponse = ByteToHexString(LastRes);
                                        WeiXinClient wxclient = CurrentClient[str_mac, 1] as WeiXinClient;
                                        if (wxclient != null)
                                        {
                                            wxclient.receivenum += GetStrAppearTimes("EF03", Converts.GetTPandMac(e.Datagram));
                                            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "接收到来自MAC为" + str_mac + "的响应，响应的内容为:" + Converts.GetTPandMac(e.Datagram));
                                            //LogHelper.showLog(txtLog, (DateTime.Now.ToString("HH:mm:ss") + "接收到来自MAC为" + str_mac + "的响应，响应的内容为:" + Converts.GetTPandMac(e.Datagram)));
                                            try
                                            {
                                                if (wxclient.SendMsg(LastRes))//收到的报文回送 
                                                {
                                                    LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "向微信端回送成功:" + wxclient.GetStrRemoteEndPoint + strLastCommanResponse);
                                                    //LogHelper.showLog(txtLog, (DateTime.Now.ToString("HH:mm:ss") + "向微信端回送成功:" + wxclient.GetStrRemoteEndPoint + strLastCommanResponse));
                                                    CurrentClient.RemoveClient(wxclient.myclient);
                                                }
                                                else
                                                {
                                                    LogHelper.showLog(txtLog, (DateTime.Now.ToString("HH:mm:ss") + "向微信端" + wxclient.GetStrRemoteEndPoint + "回送失败:" + strLastCommanResponse));
                                                    //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "向微信端" + wxclient.GetStrRemoteEndPoint + "回送失败:" + strLastCommanResponse);
                                                    CurrentClient.RemoveClient(wxclient.myclient);
                                                }
                                            }
                                            catch
                                            {
                                                CurrentClient.RemoveClient(wxclient.myclient);
                                                //LogHelper.showLog(txtLog, (DateTime.Now.ToString("HH:mm:ss") + "向微信端" + wxclient.GetStrRemoteEndPoint + "回送失败:" + strLastCommanResponse));
                                                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "向微信端" + wxclient.GetStrRemoteEndPoint + "回送失败:" + strLastCommanResponse);
                                            }
                                        }
                                        dev.LastCommanResponse = strLastCommanResponse;
                                        bool isAdded = CurrentClient.AddClient(dev);
                                        CurrentClient.MultiSendOffLine();

                                    }
                                    else if (e.Datagram[9] == 0x01)//心跳
                                    {
                                        #region 将心跳信息存入数据库
                                        byte[] byte_list = HexStringToByte(str_mac);
                                        byte[] new_byte_list = new byte[byte_list.Length];
                                        var new_mac = string.Empty;
                                        for (int i = 0; i < byte_list.Length; i++)
                                        {
                                            byte new_byte = (byte)(byte_list[i] - 0x30);
                                            new_mac += new_byte.ToString();
                                        }
                                        var commend = Converts.GetTPandMac(e.Datagram);
                                        LogHelper.SaveHeart(new_mac, commend);
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
end", new_mac, commend);
                                        try
                                        {
                                            var b = DbHelperSQL.ExecuteSql(sql);
                                        }
                                        catch (Exception ex2)
                                        {
                                            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + ex2.Message);
                                        }
                                        #endregion
                                        /*
                                    dev.LastBeat = ByteToHexString(e.Datagram);   //服务器回应心跳包
                                    //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "来自设备：" + dev.GetStrRemoteEndPoint + "；收到心跳" + dev.LastBeat);
                                    byte[] responsebeats;
                                    string strres;
                                    responsebeats = GetRightBeatsResponse(mac, true);
                                    strres = ByteToHexString(responsebeats);
                                    //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "会送命令：" + responsebeats.ToString() + "；收到心跳" + dev.LastBeat);
                                    if (CheckDataGram(LastRes))
                                    {
                                        //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "验证通过情况：" + responsebeats.ToString() + "；收到心跳" + dev.LastBeat);
                                        //dev.SendMsg(responsebeats);
                                        //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "向" + str_mac + "发送响应心跳：" + strres);
                                    }
                                    */
                                        bool isAdded = CurrentClient.AddClientWithOutCmd(dev);
                                        CurrentClient.MultiSendOffLine();
                                    }
                                    //bool isAdded = CurrentClient.AddClient(dev);
                                    //CurrentClient.MultiSendOffLine();
                                }
                            }
                            catch (Exception e2)
                            {
                                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "；" + e2.Message);
                            }
                            #endregion
                        }
                        break;
                    //微信过来的，发命令
                    case 02:
                        {
                            //mac_list.Add(e.Datagram);
                            //if (open)
                            //{

                            //}
                            try
                            {
                                #region 客户端发送的控制数据
                                //open = false;
                                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "客户端：" + e.TcpClient.Client.RemoteEndPoint.ToString() + "的原始命令：" + ByteToHexString(e.Datagram));
                                LogHelper.showLog(txtLog, (DateTime.Now.ToString("HH:mm:ss") + "客户端：" + e.TcpClient.Client.RemoteEndPoint.ToString() + "的原始命令：" + ByteToHexString(e.Datagram)));
                                Array.Copy(e.Datagram, 1, mac, 0, 15);

                                //mac_list.RemoveAt(0);
                                str_mac = Converts.GetTPandMac(mac);
                                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "MAC：" + str_mac);
                                WeiXinClient wxclt = new WeiXinClient();
                                wxclt.sendnum = 0;
                                wxclt.receivenum = 0;
                                wxclt.myclient = e.TcpClient;
                                wxclt.StrMac = str_mac;
                                wxclt.command = ByteToHexString(e.Datagram);
                                wxclt.typeid = 1;
                                CurrentClient.AddClient(wxclt);
                                DeviceClient dev = CurrentClient[str_mac, 2] as DeviceClient;
                                LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + " MAC：" + str_mac);
                                if (dev != null)
                                {//拆命令发送
                                    LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + " dev不为空：" + str_mac);
                                    LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + " 命令长度：" + e.Datagram.Length);

                                    byte[] box_number = new byte[e.Datagram.Length - 16];

                                    Array.Copy(e.Datagram, 16, box_number, 0, e.Datagram.Length - 16);
                                    LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + " boxnumber：" + Converts.GetTPandMac(box_number));
                                    int sendnum = 0;
                                    #region 一次性
                                    byte[] byteCommand = GetSendMulty(mac, box_number);
                                    string sss = ByteToHexString(byteCommand);
                                    LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + " sss：" + sss);
                                    try
                                    {
                                        Thread.Sleep(750);
                                        LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + " byteCommand：" + byteCommand);
                                        dev.SendMsg(byteCommand);

                                        sendnum++;
                                        break;
                                    }
                                    catch (Exception err)
                                    {
                                        LogHelper.WriteLog("发送指令异常:" + err.Message + "错误发生位置为" + err.StackTrace);
                                        LogHelper.showLog(txtLog, ("发送指令异常:" + err.Message + "错误发生位置为" + err.StackTrace));
                                    }
                                    #endregion
                                    wxclt.sendnum = sendnum;
                                    CurrentClient.AddClient(wxclt);
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                LogHelper.WriteLog("发送指令异常:" + ex.Message + "错误发生位置为" + ex.StackTrace);
                            }


                        }

                        break;
                    default:
                        break;
                }
                Control.CheckForIllegalCrossThreadCalls = false;
                label1.Text = CurrentClient.GetDevCount().ToString();
                label3.Text = CurrentClient.GetWeiXinNumber().ToString();
                //try
                //{

                //}
                //catch (Exception ex)
                //{
                //    string messages = string.Empty;
                //    messages = "捕获异常" + ex.Message + "错误发生位置为" + ex.StackTrace;
                //    //LogHelper.WriteLog(messages);
                //    //LogHelper.showLog(txtLog, messages);
                //}
                //finally
                //{

                //}
            }
        }
        #region ----第一版-----
        //bool CheckDataGram(byte[] datagram)
        //{
        //    string strdatagram = ByteToHexString(datagram);//EF020016112019151A01FEE199B98B39B78C9F8497EA00000000000000000000000000086C
        //    bool result = false;
        //    if (datagram.Length == 37)
        //    {
        //        byte[] bytecrc = new byte[33];
        //        Array.Copy(datagram, 2, bytecrc, 0, 33);

        //        string strcrc = ByteToHexString(bytecrc);//0016112019151A01FEE199B98B39B78C9F8497EA00000000000000000000000000

        //        byte crcsumheigh = Converts.GetCRCSUM(bytecrc)[0];
        //        byte crcsumlow = Converts.GetCRCSUM(bytecrc)[1];
        //        if (crcsumheigh == datagram[35] && crcsumlow == datagram[36])
        //        {
        //            result = true;
        //        }
        //    }
        //    return result;
        //}
        #endregion

        //校验数据
        bool CheckDataGram(byte[] datagram)
        {
            string strdatagram = ByteToHexString(datagram);//EF020016112019151A01FEE199B98B39B78C9F8497EA00000000000000000000000000086C
            bool result = false;
            if (datagram.Length == 50)
            {
                byte[] bytecrc = new byte[46];
                try { Array.Copy(datagram, 2, bytecrc, 0, 46); }
                catch (Exception ex1) { ;}


                string strcrc = ByteToHexString(bytecrc);//0016112019151A01FEE199B98B39B78C9F8497EA00000000000000000000000000

                byte crcsumheigh = Converts.GetCRCSUM(bytecrc)[0];
                byte crcsumlow = Converts.GetCRCSUM(bytecrc)[1];
                if (crcsumheigh == datagram[48] && crcsumlow == datagram[49])
                {
                    result = true;
                }
            }
            return result;
        }
        class APIMSG
        {
            public string result;
            public string targetUrl;
            public bool success;
            public string error;
            public bool unAuthorizedRequest;
            public bool __abp;
        }
        //特性标记，使之可以序列化
        [DataContract]
        class BoxStateT
        {
            [DataMember]
            int index;
            [DataMember]
            int state;
            public BoxStateT(int _index, BoxState _state)
            {
                index = _index;
                state = (int)_state;
            }
        }


        enum BoxState
        {
            /// <summary>
            /// 设备离线
            /// </summary>
            OutLine = -1,

            /// <summary>
            /// 正常的
            /// </summary>
            Success = 0,

            /// <summary>
            /// 空的
            /// </summary>
            Space = 1,

            /// <summary>
            /// 开箱
            /// </summary>
            Opend = 2,

            /// <summary>
            /// 故障的
            /// </summary>
            Error = 3,

            /// <summary>
            /// 停用的
            /// </summary>
            Stop = 4

        }

        byte[] GetRightBeatsResponse(byte[] byte_mac, bool rightOrError)
        {
            Box_Date box_date = new Box_Date();
            byte[] beatResponse = new byte[50];
            try
            {
                Array.Copy(box_date.Date_start, 0, beatResponse, 0, 2);//Head
            }
            catch (Exception ex9) { ;}


            beatResponse[2] = 0x32;
            //byte[] timestamp1 = new byte[6];
            //Array.Copy(box_date.Date_datetime, 0, timestamp1, 0, 6);//timeStamp
            //var ts =  ConvertToTen(timestamp1);
            try
            {
                Array.Copy(box_date.Date_datetime, 0, beatResponse, 3, 6);//timeStamp
            }
            catch (Exception ex8) { ; }

            beatResponse[9] = 0x01;//Beats
            try
            {
                Array.Copy(byte_mac, 0, beatResponse, 10, 15);//timeStamp
            }
            catch (Exception ex7) { ;}


            if (rightOrError)
                beatResponse[25] = 0x00;
            else
                beatResponse[25] = 0x01;
            byte[] byte_zero_12 = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            try { Array.Copy(byte_zero_12, 0, beatResponse, 26, 12); }
            catch (Exception ex6) { ;}


            byte[] rcr = new byte[46];
            try
            {
                Array.Copy(beatResponse, 2, rcr, 0, 46);
            }
            catch (Exception ex5) { ;}


            string crcss = ByteToHexString(rcr);
            string ccccc = ByteToHexString(beatResponse);
            //beatResponse[35] = Converts.GetCRC16(rcr, true)[0];//
            //beatResponse[36] = Converts.GetCRC16(rcr, true)[1];//
            beatResponse[48] = Converts.GetCRCSUM(rcr)[0];//
            beatResponse[49] = Converts.GetCRCSUM(rcr)[1];//
            string cccccccc = ByteToHexString(beatResponse);
            return beatResponse;
        }

        private byte[] ConvertToTen(byte[] array)
        {
            var ten_byte = new List<byte>();
            foreach (byte arr in array)
            {
                try
                {
                    var type = byte.Parse(arr.ToString(), System.Globalization.NumberStyles.Integer);
                    ten_byte.Add(type);
                }
                catch (Exception ex4) { }
            }
            return ten_byte.ToArray();
        }
        /// <summary>
        /// 拼接成一次性开多个箱子的指令,0x03开箱，0x05查询[MAC与校验外其他的字节可忽略]，0x06向对方报告收到错误包[MAC与校验外其他的字节可忽略]
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="box_number"></param>
        /// <returns></returns>
        //byte[] GetSendMulty(byte[] mac, byte[] box_number, byte command = 0x03)
        //{
        //    byte[] byte_open = new byte[12] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        //    int bytelength = box_number.Length > 6 ? 6 : box_number.Length;
        //    var new_box_number = new byte[12] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        //    byte[] bs = BitConverter.GetBytes(0x1234);
        //    Console.WriteLine(bs[0].ToString("X2") + " " + bs[1].ToString("X2"));
        //    //原先的逻辑是，1-6传0x01,0x02,0x03等信息
        //    //for (int i = 0; i < bytelength; i++)
        //    //{
        //    //    int boxNo = (int)box_number[i];
        //    //    if (boxNo > 0)
        //    //        byte_open[boxNo - 1] = 0x01;
        //    //}
        //    for (int i = 1; i < bytelength+1; i++)
        //    {
        //        int boxNo = (int)box_number[i - 1];
        //        switch (boxNo)
        //        {
        //            case 11: byte_open[i * 2 - 1] = 0x01; byte_open[i * 2 - 2] = 0x01; break;
        //            case 10: byte_open[i * 2 - 1] = 0x01; break;
        //            case 01: byte_open[i * 2 - 2] = 0x01; break;
        //            case 00:
        //            default:break;
        //        }
        //    }
        //    byte[] r_byte = new byte[37];
        //    Box_Date box_date = new Box_Date();
        //    byte[] date_control = new byte[38];
        //    date_control[0] = box_date.Date_flg[0];//
        //    Array.Copy(box_date.Date_start, 0, date_control, 1, 2);//
        //    date_control[3] = box_date.Date_length[0];//
        //    Array.Copy(box_date.Date_datetime, 0, date_control, 4, 6);////
        //    date_control[10] = command;//指令码
        //    Array.Copy(mac, 0, date_control, 11, 12);//
        //    //Array.Copy(byte_open, 0, date_control, 23, 6);//原先逻辑
        //    Array.Copy(box_number, 0, date_control, 23, 6);
        //    byte[] rcr = new byte[33];
        //    Array.Copy(date_control, 3, rcr, 0, 33);
        //    string aa = ByteToHexString(date_control);
        //    string aas = ByteToHexString(rcr);
        //    date_control[36] = Converts.GetCRCSUM(rcr)[0];//old
        //    date_control[37] = Converts.GetCRCSUM(rcr)[1];//old
        //    Array.Copy(date_control, 1, r_byte, 0, 37);
        //    return r_byte;
        //}

        byte[] GetSendMulty(byte[] mac, byte[] box_number, byte command = 0x03)
        {
            byte[] byte_open = new byte[12] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            int bytelength = box_number.Length > 12 ? 12 : box_number.Length;

            byte[] bs = BitConverter.GetBytes(0x1234);

            try { Console.WriteLine(bs[0].ToString("X2") + " " + bs[1].ToString("X2")); }
            catch (Exception ex2) { ; }

            for (int i = 0; i < bytelength; i++)
            {
                int boxNo = (int)box_number[i];
                if (boxNo > 0)
                    byte_open[i] = 0x01;
            }

            byte[] r_byte = new byte[50];
            try
            {
                Box_Date box_date = new Box_Date();
                byte[] date_control = new byte[51];
                date_control[0] = box_date.Date_flg[0];//
                Array.Copy(box_date.Date_start, 0, date_control, 1, 2);//
                date_control[3] = box_date.Date_length[0];//
                Array.Copy(box_date.Date_datetime, 0, date_control, 4, 6);////
                date_control[10] = command;//指令码
                Array.Copy(mac, 0, date_control, 11, 15);//
                Array.Copy(box_number, 0, date_control, 26, 12);
                byte[] rcr = new byte[46];
                Array.Copy(date_control, 3, rcr, 0, 46);
                string aa = ByteToHexString(date_control);
                string aas = ByteToHexString(rcr);
                date_control[49] = Converts.GetCRCSUM(rcr)[0];//old
                date_control[50] = Converts.GetCRCSUM(rcr)[1];//old
                Array.Copy(date_control, 1, r_byte, 0, 50);
            }
            catch (Exception ex3) { ; }


            return r_byte;
        }


        /// <summary> 
        /// 显示当期某个客户端已连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void server_ClientConnected(object sender, TcpClientConnectedEventArgs e)
        {
            LogHelper.showLog(txtLog, "主机：" + e.TcpClient.Client.RemoteEndPoint.ToString() + ",连接到服务器\r\n");
            //LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "：" + e.TcpClient.Client.RemoteEndPoint.ToString() + ",连接到服务器\r\n");
        }

        /// <summary>
        /// 显示当前某个客户端已断开连接
        /// 在这里判断货柜主机的是否掉线，客户端掉线在websocket中判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void server_ClientDisconnected(object sender, TcpClientDisconnectedEventArgs e)
        {
            LogHelper.showLog(txtLog, DateTime.Now.ToString("HH:mm:ss") + ":" + e.TcpClient.Client.RemoteEndPoint.ToString() + "->与服务端断开连接\r\n");
            //WriteLog(DateTime.Now.ToString("HH:mm:ss")+":" + e.TcpClient.Client.RemoteEndPoint.ToString() + "->与服务端断开连接\r\n");
            CurrentClient.RemoveClient(e.TcpClient);
        }


        #region "普通事件"
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("手动停止服务器之后需要人工重启", "停止服务", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                // 关闭所有的线程
                if (server.IsRunning)
                {
                    server.Stop();
                }
                this.Dispose();
                this.Close();
            }
        }


        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_show_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }


        /// <summary>
        /// 服务器启动关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (server.IsRunning)
            {
                try
                {
                    server.Stop();
                    lock (CurrentClient)
                    {
                        CurrentClient.Clear();
                    }
                    label1.BeginInvoke(new EventHandler(delegate
                    {

                        label1.Text = "0";
                    }));
                    btnStart.Text = "启动服务";
                }
                catch (Exception ex) { }

            }
            else
            {
                try
                {
                    server = new AsyncTcpServer(2758);
                    server.Encoding = Encoding.ASCII;
                    server.ClientConnected += new EventHandler<TcpClientConnectedEventArgs>(server_ClientConnected);
                    server.ClientDisconnected += new EventHandler<TcpClientDisconnectedEventArgs>(server_ClientDisconnected);

                    server.DatagramReceived += new EventHandler<TcpDatagramReceivedEventArgs<byte[]>>(server_DatagramReceived);
                    server.Start(2049);
                    btnStart.Text = "停止服务";
                }
                catch (Exception ex)
                {
                    LogHelper.showLog(txtLog, "服务器启动失败" + ex.Message);
                }

            }
        }
        //private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    e.Cancel = true;


        //}


        private void txtLog_DoubleClick(object sender, EventArgs e)
        {
            txtLog.BeginInvoke(new EventHandler(delegate
            {
                txtLog.Clear();
            }));
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            CurrentClient.PrintAll();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txtLog.Text = string.Empty;
            CurrentClient.PrintAll();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;    //取消"关闭窗口"事件
                this.WindowState = FormWindowState.Minimized;    //使关闭时窗口向右下角缩小的效果
                notifyIcon1.Visible = true;
                this.Hide();
                return;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)    //最小化到系统托盘
            {
                notifyIcon1.Visible = true;    //显示托盘图标
                this.Hide();    //隐藏窗口
            }
        }

        int count = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (count >= 60 * 30)
            {
                count = 0;
                CurrentClient.PrintAll();
                if (this.WindowState != FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Normal;
                }
                this.Show();
            }
            else
            {
                count++;
            }
        }
        #endregion
    }

}
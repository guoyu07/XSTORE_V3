using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using XStore.Common;

namespace WeiXinPush
{
    public class RemoteBoxHelper
    {
        protected NetworkStream sendStream;
        Thread threadclient = null;
        Socket socketclient = null;
        private int port = 2758;

        //private string ipAddress = "119.29.94.189";
        private string ipAddress = "139.199.160.173";

        public Dictionary<string, string> OpenRemoteBox(string serialNumber, string orderNo, string warehouseIndexs,byte type=0x01)
        {

            if (string.IsNullOrEmpty(serialNumber))
                throw new Exception("序列号无效");
            if (string.IsNullOrEmpty(warehouseIndexs))
                throw new Exception("货箱序号无效");
            var list = warehouseIndexs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(o => int.Parse(o)).ToList();
            byte[] byte_open = new byte[12] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            for (int i = 0; i < list.Count; i++)
            {
                byte_open[list[i]] = 0x01;
            }
            var dic = new Dictionary<string, string>();
            byte[] order_no = Encoding.UTF8.GetBytes(orderNo);
            byte[] ping = Encoding.UTF8.GetBytes(serialNumber);
            //byte[] commandByteArr = TcpClientHelper.GetSendMulty(ping, byte_open, 0x02);
            byte[] commandByteArr = TcpClientHelper.GetSendMulty(ping, byte_open, order_no, type);
            var commandStr = Converts.GetTPandMac(commandByteArr);
            //SocketException exception;
            //定义一个套接字监听
            socketclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //获取文本框中的IP地址
            IPAddress address = IPAddress.Parse(ipAddress);
            //将获取的IP地址和端口号绑定在网络节点上
            IPEndPoint point = new IPEndPoint(address, port);
            socketclient.Connect(point);
            socketclient.Send(commandByteArr);
            var resultStr = string.Empty;
            socketclient.Close();
            dic["success"] = "成功";
            return dic;

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
        private string ByteToHexString(byte[] data)
        {
            StringBuilder dataString = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                dataString.AppendFormat("{0:x2}", data[i]);
            }
            return dataString.ToString().Trim().ToUpper();
        }

        //TODO
        private static Dictionary<string, string> GetOpendStateByReviceMessage(string msg, string warehouseIndexs)
        {
            //EF022510091111281C038652E1CC31C988810E44787A020000000404000000000000004303
            byte[] byte_open = new byte[12] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var list = warehouseIndexs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var result = new Dictionary<string, string>();

            //命令字判断，是否当前操作
            if (msg.Substring(18, 2) != "03") return result;

            foreach (var i in list)
            {
                var number = int.Parse(i);
                var startIndex = 44 + (number - 1) * 2;
                var stateCode = msg.Substring(startIndex, 2);
                var isOpend = stateCode == "01" || stateCode == "02";
                result.Add(i, isOpend.ToString());
            }

            return result;
        }

        private static byte[] StringToByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            var returnBytes = new byte[hexString.Length / 2];
            for (var i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        private static string GetTPandMac(IEnumerable<byte> arrays)
        {
            var mac = arrays.Aggregate("", (current, b) => current + string.Format("{0:X2}", b));
            mac = mac.TrimEnd();
            return mac;
        }
    }
}

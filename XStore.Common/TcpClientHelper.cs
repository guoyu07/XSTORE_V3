using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using XStore.Common;

namespace XStore.Common
{
    public class TcpClientHelper
    {
        private string server = "119.29.94.189";
        private string commend = "";
        public void SendCommend()
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 3756;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(commend);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[37];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
            
        }
        public static byte[] GetSendMulty(byte[] mac, byte[] box_number, byte command = 0x03)
        {
            byte[] byte_open = new byte[12] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            int bytelength = box_number.Length > 12 ? 12 : box_number.Length;

            byte[] bs = BitConverter.GetBytes(0x1234);

            try { Console.WriteLine(bs[0].ToString("X2") + " " + bs[1].ToString("X2")); }
            catch (Exception) {; }

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
            catch (Exception) {; }


            return r_byte;
        }
        public static byte[] GetSendMulty(byte[] mac, byte[] box_number,byte[] order_no, byte command = 0x01)
        {
            byte[] byte_open = new byte[12] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            int bytelength = box_number.Length > 12 ? 12 : box_number.Length;

            byte[] bs = BitConverter.GetBytes(0x1234);

            try { Console.WriteLine(bs[0].ToString("X2") + " " + bs[1].ToString("X2")); }
            catch (Exception) {; }

            for (int i = 0; i < bytelength; i++)
            {
                int boxNo = (int)box_number[i];
                if (boxNo > 0)
                    byte_open[i] = 0x01;
            }
            byte[] date_control = new byte[50];
            try
            {
                Client_Date box_date = new Client_Date();
                Array.Copy(box_date.Date_start, 0, date_control, 0, 2);//EF03
                date_control[2] = 0x50;//
                date_control[3] = command;//指令码
                Array.Copy(mac, 0, date_control, 4, 15);//
                Array.Copy(box_number, 0, date_control, 19, 12);
                Array.Copy(order_no, 0, date_control, 31, order_no.Count());////
                Array.Copy(Encoding.UTF8.GetBytes(order_no.Count().ToString()), 0, date_control, 47, 1);////
                byte[] rcr = new byte[46];
                Array.Copy(date_control, 2, rcr, 0, 46);
                date_control[48] = Converts.GetCRCSUM(rcr)[0];//old
                date_control[49] = Converts.GetCRCSUM(rcr)[1];//old

            }
            catch (Exception ex) {; }


            return date_control;
        }
        /// <summary>
        /// 字节数组转换成大写的一组16进制形式的字符串。比如：{0x2C,0xD7}转换成"2CD7"
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ByteToHexString(byte[] data)
        {
            StringBuilder dataString = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                dataString.AppendFormat("{0:x2}", data[i]);
            }
            return dataString.ToString().Trim().ToUpper();
        }
    }
}
public class Box_Date
{
    byte[] date_flg = new byte[1] { 0X01 };//协议标志位，标识是否为客户端操作

    public byte[] Date_flg
    {
        get { return date_flg; }

    }
    byte[] date_start = new byte[2] { 0xEF, 0X03 };//数据帧头码(2Bytes)

    public byte[] Date_start
    {
        get { return date_start; }

    }
    byte[] date_length = new byte[1] { 0x32 };//数据帧字节数（1Bytes)

    public byte[] Date_length
    {
        get { return date_length; }

    }
    byte[] date_datetime = Converts.DateTimeToBytes();//new byte[6];//时间戳（6个字节）

    public byte[] Date_datetime
    {
        get { return date_datetime; }

    }


    byte[] date_functioncommandword = new byte[1];//功能命令字（1Bytes)（下标9）

    public byte[] Date_functioncommandword
    {
        get { return date_functioncommandword; }
        set { date_functioncommandword = value; }
    }
    byte[] date_mac = new byte[15];//12字节机柜的MAC

    public byte[] Date_mac
    {
        get { return date_mac; }
        set { date_mac = value; }
    }
    byte[] date_command = new byte[25];//控制对象的数据区

    public byte[] Date_command
    {
        get { return date_command; }
        set { date_command = value; }
    }
}

public class Client_Date
{
    byte[] date_flg = new byte[1] { 0X01 };//协议标志位，标识是否为客户端操作

    public byte[] Date_flg
    {
        get { return date_flg; }

    }
    byte[] date_start = new byte[2] { 0xFF, 0X02 };//数据帧头码(2Bytes)

    public byte[] Date_start
    {
        get { return date_start; }

    }

    byte[] date_length = new byte[1] { 0x32 };//数据帧字节数（1Bytes)

    public byte[] Date_length
    {
        get { return date_length; }
        set { date_length = value; }
    }

    byte[] date_mac = new byte[15];//15字节机柜的MAC

    public byte[] box_state = new byte[12];
    public byte[] Date_state
    {
        get { return box_state; }
        set { box_state = value; }
    }
    byte[] date_command = new byte[25];//控制对象的数据区

    byte[] order_no = new byte[15];//new byte[15];//订单（6个字节）
    public byte[] Date_orderno {
        get { return order_no; }
        set { order_no = value; }
    }

    byte[] order_count = new byte[1];//new byte[1];//订单长度（6个字节）
    public byte[] Date_ordercount
    {
        get { return order_count; }
        set { order_count = value; }
    }

}
public class ResponseResult
{
    public bool Status { get; set; }
    public int ErrorCode { get; set; }
    public string Message { get; set; }
    public string Data { get; set; }

}

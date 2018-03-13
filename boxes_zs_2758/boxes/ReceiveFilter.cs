using boxes.Common;
using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Text;
using XStore.Common;

namespace boxes
{
    public class ReceiveFilter: FixedSizeReceiveFilter<BoxRequestInfo>

    {
        ////开始和结束标记也可以是两个或两个以上的字节
        //private readonly static byte[] BeginMark = HexStringToByteArray("EF");
        //private readonly static byte[] EndMark = HexStringToByteArray("0D0A");

        public ReceiveFilter():base(50)//总的字节长度 
        {
        }

        protected override BoxRequestInfo ProcessMatchedRequest(byte[] buffer, int offset, int length, bool toBeCopied)
        {
            var boxModel = new BoxModel();
            var headByte = new byte[2];
            var command = buffer.CloneRange(offset, 50);
   
            if (!CheckDataGram(command))
            {
                return new BoxRequestInfo(new BoxModel());
            }
            boxModel.Head = ByteToHexString(buffer.CloneRange(offset, 2));//开始标识的解析，2个字节
            boxModel.Lenght = BitConverter.ToUInt16(buffer,offset + 2);
     
            if (boxModel.Head.Equals("EF02"))
            {
                boxModel.Type =2;
                boxModel.OpenType = buffer[offset + 3];
                boxModel.Mac = Converts.GetTPandMac(buffer.CloneRange(offset + 4, 15));
                boxModel.FormatMac = FormatMac(boxModel.Mac);
                boxModel.Command = Converts.GetTPandMac(command);
                boxModel.State = buffer.CloneRange(offset + 19, 12);
                var oderCount = int.Parse(Encoding.UTF8.GetString(buffer.CloneRange(offset + 46, 2)));
                var orderNo = buffer.CloneRange(offset + 31, oderCount);
                LogHelper.WriteLog("命令过来的type类型："+(int)boxModel.OpenType);
                switch ((Enum.OpenType)((int)boxModel.OpenType))
                {
                    case Enum.OpenType.订单开箱:
                        boxModel.OrderNo = "S" + Encoding.UTF8.GetString(orderNo);
                        break;
                    case Enum.OpenType.补货开箱:
                        boxModel.OrderNo = "B" + Encoding.UTF8.GetString(orderNo);
                        break;
                    case Enum.OpenType.开箱检查:
                        boxModel.OrderNo = "J" + Encoding.UTF8.GetString(orderNo);
                        break;
                    case Enum.OpenType.测试开箱:
                        boxModel.OrderNo = "C" + Encoding.UTF8.GetString(orderNo);
                        break;
                }
               
            }
            else if(boxModel.Head.Equals("EF03"))
            {
                boxModel.Type = buffer[offset + 9];
                boxModel.Time = Converts.GetTPandMac(buffer.CloneRange(offset + 3, 6));
                boxModel.Mac = Converts.GetTPandMac(buffer.CloneRange(offset + 10, 15));
                boxModel.FormatMac = FormatMac(Converts.GetTPandMac(buffer.CloneRange(offset + 10, 15)));
                boxModel.State = buffer.CloneRange(offset + 25, 12);
                boxModel.Signal = buffer.CloneRange(offset + 37, 1);
                boxModel.Placeholder = buffer.CloneRange(offset + 47, 1);
                boxModel.Check = buffer.CloneRange(offset + 48, 2);
                boxModel.Command = Converts.GetTPandMac(command);
            }
            //三代箱子
            else if (boxModel.Head.Equals("FF02"))
            {
                boxModel.Type = 2;
                boxModel.OpenType = buffer[offset + 3];
                boxModel.Mac = Converts.GetTPandMac(buffer.CloneRange(offset + 4, 15));
                boxModel.FormatMac = FormatMac(boxModel.Mac);
                boxModel.Command = Converts.GetTPandMac(command);
                boxModel.State = buffer.CloneRange(offset + 19, 12);
                var oderCount = int.Parse(Encoding.UTF8.GetString(buffer.CloneRange(offset + 47, 1)));
                var orderNo = buffer.CloneRange(offset + 31, oderCount);
                LogHelper.WriteLog("命令过来的type类型：" + (int)boxModel.OpenType);
                boxModel.OrderNo = Encoding.UTF8.GetString(orderNo);

            }
            return new BoxRequestInfo(boxModel);
        }
        private static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
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
        private string FormatMac(string str_mac) {
            byte[] byte_list = HexStringToByte(str_mac);
            byte[] new_byte_list = new byte[byte_list.Length];
            var new_mac = string.Empty;
            for (int i = 0; i < byte_list.Length; i++)
            {
                byte new_byte = (byte)(byte_list[i] - 0x30);
                new_mac += new_byte.ToString();
            }
            return new_mac;
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

        private bool CheckDataGram(byte[] datagram)
        {
            string strdatagram = ByteToHexString(datagram);//EF020016112019151A01FEE199B98B39B78C9F8497EA00000000000000000000000000086C
            bool result = false;
            if (datagram.Length == 50)
            {
                byte[] bytecrc = new byte[46];
                try { Array.Copy(datagram, 2, bytecrc, 0, 46); }
                catch (Exception ex) { result = false ; }

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
    }
}

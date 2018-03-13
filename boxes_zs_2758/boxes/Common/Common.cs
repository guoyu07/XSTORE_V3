using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace boxes.Common
{
    /// <summary>
    /// 公共方法封装
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// 通讯ID长度
        /// </summary>
        public const byte IdLength = 36;

        /// <summary>
        /// 获取本机的IP地址
        /// </summary>
        /// <returns>IP地址</returns>
        public static IPAddress GetSVRIP()
        {
            IPHostEntry iph = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in iph.AddressList)
            {
                //只获取IPV4版本地址
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            }
            return null;
        }

        /// <summary>
        /// 将一个数组中的连续部分复制到另一数组
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="length">长度</param>
        public static byte[] CopyArrayData(byte[] source, int startIndex, int length)
        {
            byte[] result = new byte[length];

            for (var i = 0; i < length; i++)
            {
                result[i] = source[startIndex + i];
            }

            return result;
        }
    }
}

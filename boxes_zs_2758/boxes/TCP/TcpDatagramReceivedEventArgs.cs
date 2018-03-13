using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace boxes.TCP
{
    // <summary>
    /// 接收到数据报文事件参数
    /// </summary>
    /// <typeparam name="T">报文类型</typeparam>
    public class TcpDatagramReceivedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 接收到数据报文事件参数
        /// </summary>
        /// <param name="tcpClient">客户端</param>
        /// <param name="datagram">报文</param>
        public TcpDatagramReceivedEventArgs(TcpClient tcpClient, T datagram)
        {
            TcpClient = tcpClient;
            Datagram = datagram;
        }

        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient TcpClient { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        public T Datagram { get; private set; }
    }
}

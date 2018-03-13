using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace boxes.TCP
{
    /// <summary>
    /// 与客户端的连接已断开事件参数
    /// </summary>
    public class TcpClientDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 与客户端的连接已断开事件参数
        /// </summary>
        /// <param name="tcpClient">客户端</param>
        public TcpClientDisconnectedEventArgs(TcpClient tcpClient)
        {
            if (tcpClient == null)
                throw new ArgumentNullException("tcpClient");

            this.TcpClient = tcpClient;
        }

        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient TcpClient { get; set; }
    }
}

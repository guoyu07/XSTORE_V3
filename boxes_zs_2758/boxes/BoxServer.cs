using System.Configuration;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Config;

namespace boxes
{
    public class BoxServer: AppServer<BoxSession,BoxRequestInfo>
    {
        public BoxServer() : base(new BoxReceiveFilterFactory())  //使用默认的接受过滤器工厂 (DefaultReceiveFilterFactory)
        {
            
        }
        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {

            return true;
        }
    }
}

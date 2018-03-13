using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using System.Net;

namespace boxes
{
    public class BoxReceiveFilterFactory : IReceiveFilterFactory<BoxRequestInfo>
    {
        public IReceiveFilter<BoxRequestInfo> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            return new ReceiveFilter();
        }
    }
}

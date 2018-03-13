using boxes.Common;
using boxes.TCP;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;


namespace boxes
{
    public class BoxSession:AppSession<BoxSession,BoxRequestInfo>
    {
        public string CustomId { get; set; } = string.Empty;
        public int CustomType { get; set; } = 0;
        public int Type { get; set; } = 1;
        public string OrderNo { get; set; } = string.Empty;
        public string Mac { get; set; } = string.Empty;
        protected override void OnSessionStarted()
        {
          
            base.OnSessionStarted();
        }
        protected override void OnSessionClosed(CloseReason reason)
        {
            base.OnSessionClosed(reason);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace XStore.Common.WeiXinPay
{
    public class WxPayException : Exception
    {
        public WxPayException(string msg)
            : base(msg)
        {

        }
    }
}

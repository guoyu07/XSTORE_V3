using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxes.Common
{
    public class OpCode
    {
        public const int Text = 1;
        public const int Binary = 2;
        public const int Close = 8;
        public const int Ping = 9;
        public const int Pong = 10;
    }
}

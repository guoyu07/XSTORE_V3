using boxes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XStore.Common;

namespace boxes
{
    public  class BoxModel
    {
        /// <summary>
        /// 帧头码
        /// </summary>
        public string Head { get; set; } = string.Empty;
        /// <summary>
        /// 数据帧字节数
        /// </summary>
        public ushort Lenght { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 功能码
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// 功能码
        /// </summary>
        public byte OpenType { get; set; }
        /// <summary>
        /// 箱柜地址码
        /// </summary>
        public string Mac { get; set; }
        /// <summary>
        /// 箱柜地址码(装换后)
        /// </summary>
        public string FormatMac { get; set; }
        /// <summary>
        /// 柜门的状态
        /// </summary>
        public byte[] State { get; set; }
        /// <summary>
        /// 信号强度
        /// </summary>
        public byte[] Signal { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public byte[] Placeholder { get; set; }
        /// <summary>
        /// 和校验
        /// </summary>
        public byte[] Check { get; set; }
        /// <summary>
        /// command
        /// </summary>
        /// <returns></returns>
        public string Command { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; } = string.Empty;
        public override string ToString()
        {
            return string.Format("{9}-帧头码:{0},数据帧字节数:{1},时间戳:{2},功能码:{3},箱柜地址码:{4},柜门的状态:{5},信号强度：{6},版本号:{7},合校验:{8}",
                Head, Lenght, Time, Type, FormatMac, Converts.GetTPandMac(State), Converts.GetTPandMac(Signal), Converts.GetTPandMac(Placeholder), Converts.GetTPandMac(Check),DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        //EF0332110B150E1C3401383631383533303332303036363033030303000300000000000000130000000000000500060B03FA
        public static byte[] ToCommand(BoxModel model)
        {
            LogHelper.WriteLog(DateTime.Now.ToString("HH:mm:ss") + "需要拼接的命令：" + JsonConvert.SerializeObject(model));
            byte[] macByte = Encoding.UTF8.GetBytes(model.FormatMac);
            return TcpClientHelper.GetSendMulty(macByte, model.State);
        }

       
    }
}

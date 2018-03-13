using System;
using System.Collections.Generic;
using System.Text;

namespace XStore.Common.WeiXinPay
{
    public class NativePay
    {   
        /**
        * 生成直接支付url，支付url有效期为2小时,模式二
        * @param productId 商品ID
        * @return 模式二URL
        */
        public string GetPayUrl(WxPayData data)
        {
            try
            {
                Log.Info(this.GetType().ToString(), "Native pay mode 2 url is producing...");

                ///原生扫码支付
                WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
                string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接

                Log.Info(this.GetType().ToString(), "Get native pay mode 2 url : " + url);
                return url;
            }
            catch
            {
                return string.Empty;
            }
        }

        /**
        * 参数数组转换为url格式
        * @param map 参数名与参数值的映射表
        * @return URL字符串
        */
        private string ToUrlParams(SortedDictionary<string, object> map)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in map)
            {
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }
    }
}

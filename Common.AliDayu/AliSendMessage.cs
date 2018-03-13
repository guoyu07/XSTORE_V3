using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace Common.AliDayu
{
    public class AliSendMessage
    {
        private static string url = "https://eco.taobao.com/router/rest";
        private static string appkey = "23600531";
        private static string secret = "6f24b142d59e56381a26e2310fc0db72";
        private static string sign = "幸事多";


        public static bool Send(string phone, string message)
        {
            string temp = "SMS_40965177";
            return AliSend(phone, message, temp);  
           
        }
        public static bool SendSellMsg(string phone, string message)
        {
            string temp = "SMS_78240040";
            return AliSend(phone, message, temp);
        }
        public static bool AliSend(string phone, string message, string temp)
        {
            try
            {
                ITopClient client = new DefaultTopClient(url, appkey, secret, "json");
                AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
                req.Extend = "";
                req.SmsType = "normal";
                req.SmsFreeSignName = sign;
                req.SmsParam = message;
                req.RecNum = phone;
                req.SmsTemplateCode = temp;
                AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
                JObject result = JsonConvert.DeserializeObject<JObject>(rsp.Body);
                return (bool)((JObject)((JObject)result["alibaba_aliqin_fc_sms_num_send_response"])["result"])["success"];
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
    
}

using Chloe.MySql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStore.Common;
using XStore.Common.Helper;
using XStore.Console.DBFactory;
using XStore.Entity;
using XStore.Entity.Model;

namespace XStore.Console
{
    class Program
    {
        public static string connString = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        static void Main(string[] args)
        {
            char[] charArray1 = new char[] { };
           
 
            //OpenBox();

        }
        private static void SetMacList() {
            var requestUrl = string.Format("{0}test/cabinets?page=0&size=1000 ", Constant.YunApi);
            var response = JsonConvert.DeserializeObject<JObject>(Utils.HttpGet(requestUrl));
            if (response["operationStatus"].ObjToStr().Equals("SUCCESS"))
            {
                var arrList = JsonConvert.DeserializeObject<JArray>(response["operationMessage"].ObjToStr());
                List<OnlineBox> macList = new List<OnlineBox>();

                foreach (var arr in arrList)
                {
                    var sub = JsonConvert.DeserializeObject<JArray>(arr.ToString());
                    var mac = new OnlineBox();
                    mac.mac = sub[0].ObjToStr();
                    mac.online = sub[1].ObjToInt(0) == 0 ? false : true;
                    mac.lineTime = DateTime.Now;
                    macList.Add(mac);
                }
                CacheHelper.SetCache("Boxes", macList);
            }
        }
        private static void OpenBox() {
            var rbh = new RemoteBoxHelper();
           var  context = new MySqlContext(new MySqlConnectionFactory(connString));
            var orderInfo = context.Query<OrderInfo>()
               .FirstOrDefault(o => o.paid == true && o.delivered == false && o.date.AddMinutes(30) > DateTime.Now);
            if (orderInfo != null)
            {
                rbh.OpenRemoteBox(orderInfo.cabinet_mac.ObjToStr(), orderInfo.code.ObjToStr(), orderInfo.pos.ObjToStr());
            }
            //rbh.OpenRemoteBox("861853033030503", "00000010", "1");
        }
    }
}

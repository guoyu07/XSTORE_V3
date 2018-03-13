using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using XStore.Common;
using XStore.Common.Helper;
using XStore.Entity;

namespace XStore.WebSite.WebSite.Operation
{
    public partial class BoxCheck : MacPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "幸事多私享空间-测试开箱";
            if (!IsPostBack)
            {
                PageInit();
            }
        }

        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }
        protected void PageInit()
        {
            var list = new List<numClass>();
            list.Add(new numClass() { num = 1 });
            list.Add(new numClass() { num = 2 });
            list.Add(new numClass() { num = 3 });
            list.Add(new numClass() { num = 4 });
            list.Add(new numClass() { num = 5 });
            list.Add(new numClass() { num = 6 });
            list.Add(new numClass() { num = 7 });
            list.Add(new numClass() { num = 8 });
            list.Add(new numClass() { num = 9 });
            list.Add(new numClass() { num = 10 });
            list.Add(new numClass() { num = 11 });
            list.Add(new numClass() { num = 12 });
            box_rp.DataSource = list;
            box_rp.DataBind();

        }
        public class numClass
        {
            public int num { get; set; }
        }
        protected void position_click(object sender, EventArgs e)
        {
            var num = (sender as HtmlAnchor).Attributes["num"].ObjToInt(0) - 1;
            var list = new List<string>();
            list.Add(num.ObjToStr());
            OpenBox(list);
        }
        private void OpenBox(List<string> list)
        {

            var rbh = new RemoteBoxHelper();
            var postion_list = list.Aggregate<string>((x, y) => x + "," + y).ToString();
            try
            {
                rbh.OpenRemoteBox(cabinet.mac,string.Empty,postion_list);
                context.Insert(new CabinetCheckLog
                {
                    createtime = DateTime.Now,
                    issuccess = true,
                    mac = cabinet.mac,
                    message = postion_list + ":执行开箱操作",
                    openid = OpenId,
                    phone = userInfo.phone,
                    username = userInfo.username
                });
            }
            catch (Exception)
            {
                MessageBox.Show(this,"system_alert","执行开箱失败");
                return;
            }

        }

        protected void openAll_Click(object sender, EventArgs e)
        {
            var list = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            OpenBox(list.Select(o=>o.ObjToStr()).ToList());
            PageInit();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxes.DBUtility
{
    public class PubConstant
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                string _connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                if (ConfigurationManager.AppSettings["ConnectionString"] == null)//客户端连接下面
                {


#region 测试服务器
                   // _connectionString = "Data Source=139.199.160.173;Initial Catalog=tshop;Persist Security Info=True;User ID=xxx;Password=123456";
#endregion
#region 正式服务器
                    _connectionString = "Data Source=.;Initial Catalog=tshop;Persist Security Info=True;User ID=znhx;Password=znhx@abc123.#com;";
#endregion


                }
                return _connectionString;
            }
        }

        /// <summary>
        /// 得到web.config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            string connectionString = ConfigurationManager.AppSettings[configName];
            string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];

            return connectionString;
        }
    }
}

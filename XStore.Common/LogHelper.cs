using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XStore.Common
{
    public class LogHelper
    {
        #region 写入日志，排错与甩锅必备良品
        static public void WriteLog(string errorMsg)
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
                string dirInfo = "C:\\ErrorLog\\" + DateTime.Now.ToString("yyyy_MM");
                string path = dirInfo + "\\" + fileName;
                if (!Directory.Exists(dirInfo))
                {
                    Directory.CreateDirectory(dirInfo);
                }
                if (!File.Exists(path))
                {
                    FileInfo myfile = new FileInfo(path);
                    FileStream fs = myfile.Create();
                    fs.Close();
                }
                StreamWriter sw = File.AppendText(path);
                sw.WriteLine(errorMsg);
                sw.WriteLine("");
                sw.Flush();
                sw.Close();
            }
            catch
            { }


        }
        static public void WriteLogs(string errorMsg)
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
                string dirInfo = "C:\\ErrorLogs\\" + DateTime.Now.ToString("yyyy_MM");
                string path = dirInfo + "\\" + fileName;
                if (!Directory.Exists(dirInfo))
                {
                    Directory.CreateDirectory(dirInfo);
                }
                if (!File.Exists(path))
                {
                    FileInfo myfile = new FileInfo(path);
                    FileStream fs = myfile.Create();
                    fs.Close();
                }
                StreamWriter sw = File.AppendText(path);
                sw.WriteLine(errorMsg);
                sw.WriteLine("");
                sw.Flush();
                sw.Close();
            }
            catch
            { }


        }
        static public void SaveHeart(string mac,string cmd)
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
                string dirInfo =  "C:\\HeartLog\\" + mac + "\\" + DateTime.Now.ToString("yyyy_MM");
                string path = dirInfo + "\\" + fileName;
                if (!Directory.Exists(dirInfo))
                {
                    Directory.CreateDirectory(dirInfo);
                }
                if (!File.Exists(path))
                {
                    FileInfo myfile = new FileInfo(path);
                    FileStream fs = myfile.Create();
                    fs.Close();
                }
                StreamWriter sw = File.AppendText(path);
                sw.WriteLine(DateTime.Now.ToString()+":"+cmd);
                sw.WriteLine("");
                sw.Flush();
                sw.Close();
            }
            catch
            { }


        }
       

        #endregion
    }
    public static class TcpClient_Ex
    {
        public static string GetStrSocket(this TcpClient tcp)
        {
            string str = "";
            try
            {
                str = tcp.Client.RemoteEndPoint.ToString();
            }
            catch
            {
            }
            return str;
        }
        public static bool ShutDownAndClose(this TcpClient tcp)
        {
            bool result = false;
            try
            {
                if (tcp != null && tcp.Connected && tcp.Client.Connected)
                {
                    tcp.Client.Shutdown(SocketShutdown.Both);
                    Thread.Sleep(20);
                    tcp.Client.Close();
                    result = true;
                }
            }
            catch
            {

            }
            return result;
        }
    }
}

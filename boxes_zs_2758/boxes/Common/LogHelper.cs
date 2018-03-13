using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace boxes.Common
{
    public class LogHelper
    {
        #region 写入日志，排错与甩锅必备良品
        static public void WriteLog(string errorMsg)
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
                string dirInfo = System.Windows.Forms.Application.StartupPath + "\\ErrorLog\\" + DateTime.Now.ToString("yyyy_MM");
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
                string dirInfo = System.Windows.Forms.Application.StartupPath + "\\HeartLog\\" + mac + "\\" + DateTime.Now.ToString("yyyy_MM");
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
        static public void showLog(object objtxt, string message)
        {
            if (objtxt is System.Windows.Forms.RichTextBox)
            {
                System.Windows.Forms.RichTextBox txtLog = (System.Windows.Forms.RichTextBox)objtxt;
                txtLog.BeginInvoke(new EventHandler(delegate
                {
                    if (txtLog.Text.Length > 65535)
                    {
                        //txtLog.BeginInvoke(new EventHandler(delegate
                        //{
                        txtLog.Clear();
                        // }));
                    }
                    txtLog.AppendText(message);
                    txtLog.AppendText("\r\n");
                }));
            }
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

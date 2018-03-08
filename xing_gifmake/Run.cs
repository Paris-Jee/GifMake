using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace xing_gifmake
{
    public class Run
    {
        //test

        //获取当前应用程序启动目录
        string starpath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        #region 委托事件，用于通知操作完成
        public delegate void Handle(object rid);
        public event Handle RunedEvent;
        private void Event_(object rid)
        {

            RunedEvent?.Invoke(rid);

        }
        #endregion

        #region 执行指令
        public void Execute(string p, object rid)
        {


            Thread thread = new Thread(new ThreadStart(() =>
            {
                RunProcess(p, rid);
                System.Windows.Threading.Dispatcher.Run();
            }));

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }
       

        void RunProcess(string Parameters, object rid)
        {

            //设置要启动的ffmpeg程序完整路径
            var oInfo = new ProcessStartInfo(starpath + "\\ffmpeg\\ffmpeg.exe", Parameters);

            //[必须]设置启动进程的工作目录，不然后面的srt字幕嵌入指令会找不到srt字幕文件的哦，然后srt字幕自然就是放在这个目录内
            oInfo.WorkingDirectory = starpath + "\\ffmpeg";

            oInfo.UseShellExecute = false;//获取或设置一个值指示是否使用操作系统shell启动过程。

            oInfo.CreateNoWindow = true;//获取或设置一个值指示是否开始在一个新的窗口过程。

            oInfo.RedirectStandardOutput = true;//获取或设置指示是否将应用程序的文本输出写入 Process.StandardOutput 流中的值。

            oInfo.RedirectStandardError = true;//获取或设置指示是否将应用程序的错误输出写入 Process.StandardError 流中的值。

            oInfo.RedirectStandardInput = true;//获取或设置一个值,指出是否输入读取应用程序的过程。StandardInput流。


            var proc = Process.Start(oInfo);

            proc.EnableRaisingEvents = true;

            //proc.Exited += new EventHandler(Proc_Exited);
            proc.Exited += delegate (Object o, EventArgs e) { Proc_Exited(rid, o, e); };

            proc.BeginErrorReadLine();

            proc.WaitForExit();

            proc.Close();//关闭进程

            proc.Dispose();//释放资源


        }
        #endregion

        #region 操作完成，进程退出
        void Proc_Exited(object RID, object sender, EventArgs e)
        {

            //操作完成
            Event_(RID);
        }
        #endregion

    }
}

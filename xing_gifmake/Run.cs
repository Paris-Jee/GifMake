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

        //获取当前应用程序启动目录
        string starpath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        object rid;

        Process proc;
        #region 委托事件，用于通知操作完成
        public delegate void Handle(object rid);
        public event Handle RunedEvent;
        private void Event_(object rid)
        {

            RunedEvent?.Invoke(rid);

        }
        #endregion

        public Run()
        {
            proc = new Process();
            //启用关联进程终止通知事件
            proc.EnableRaisingEvents = true;

            proc.Exited += Proc_Exited; 
        }

     
        #region 执行指令

        public void Execute(string p, object rid)
        {

            this.rid = rid;

            Thread thread = new Thread(new ThreadStart(() =>
            {
                RunProcess(p);
                System.Windows.Threading.Dispatcher.Run();
            }));

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }

        
        void RunProcess(string Parameters)
        {


            //设置要启动的ffmpeg程序完整路径
            var oInfo = new ProcessStartInfo(starpath + "\\ffmpeg\\ffmpeg.exe", Parameters);

            //[必须]设置启动进程的工作目录，不然后面的srt字幕嵌入指令会找不到srt字幕文件的哦，然后srt字幕自然就是放在这个目录内
            oInfo.WorkingDirectory = starpath + "\\ffmpeg";

            oInfo.UseShellExecute = false;//获取或设置一个值指示是否使用操作系统shell启动过程。

            oInfo.CreateNoWindow = true;//获取或设置一个值指示是否开始在一个新的窗口过程。


            oInfo.RedirectStandardInput = true;//获取或设置一个值,指出是否输入读取应用程序的过程。StandardInput流。


            proc.StartInfo = oInfo;
     
            proc.Start();

           

           
            proc.WaitForExit();

           


        }


        #endregion

        #region 进程退出事件
        void Proc_Exited(object sender, EventArgs e)
        {
            //操作完成
            Event_(rid);
        }
        #endregion

    }
}

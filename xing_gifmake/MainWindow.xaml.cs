using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace xing_gifmake
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //获取当前应用程序启动目录
        string starpath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        //gif保存路径
        string gifsavepath = "";

        Run run = new Run();

        public MainWindow()
        {

            InitializeComponent();

            
            run.RunedEvent += RunedEvent;

        }

        #region 指令运行完成事件监听
        private void RunedEvent(object rid)
        {
            if (rid.ToString() == "ass")
            {
                
                string assstr = File.ReadAllText(starpath + "\\ffmpeg\\wsyw.ass");
                //因为输出图片尺寸过小所以这里调整字幕字体大小，从16>30，直接用简单粗暴的文本替换实现。
                assstr = assstr.Replace(",16,", ",30,");
                File.WriteAllText(starpath + "\\ffmpeg\\wsyw.ass", assstr);

                //将ass字幕嵌入视频，这里需要等待2分钟左右的时间才能完成字幕嵌入，不知道为何。
                string Parameters = String.Format("-i " + starpath + "\\ffmpeg\\wsyw.mp4 -vf ass=wsyw.ass -y " + starpath + "\\ffmpeg\\wsyw_cache.mp4");

                run.Execute(Parameters, "qr");

            }
            else if (rid.ToString() == "qr")
            {


                //嵌入字幕完成，导出gif
                string Parameters = String.Format(" -i " + starpath + "\\ffmpeg\\wsyw_cache.mp4 -y -s 406x224 " + gifsavepath);

                run.Execute(Parameters, "gif");

            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {

                    makebtn.IsEnabled = true;

                    makebtn.Content = "生成GIF";

                    MessageBox.Show("久等，生成GIF完成了~");

                }));
            }
        }
        #endregion

        #region 生成GIF按钮点击
        private void makebtn_Click(object sender, RoutedEventArgs e)
        {
            MakeSrt();

            makebtn.IsEnabled = false;

            makebtn.Content = "正在操作，请耐心等待...";

          
        }
        #endregion

        #region 生成台词（字幕文件）
        //关于srt字幕可以参阅资料 https://jingyan.baidu.com/article/ce09321b7b75042bff858f0e.html
        void MakeSrt()
        {
            string srt = "1\r\n00:00:00.970 --> 00:00:01.500\r\n" + l1.Text + "\r\n\r\n";
            srt += "2\r\n00:00:03.110 --> 00:00:04.390\r\n" + l2.Text + "\r\n\r\n";
            srt += "3\r\n00:00:05.180 --> 00:00:07.260\r\n" + l3.Text + "\r\n\r\n";
            srt += "4\r\n00:00:07.260 --> 00:00:09.910\r\n" + l4.Text + "\r\n\r\n";
            srt += "5\r\n00:00:10.000 --> 00:00:11.260\r\n" + l5.Text + "\r\n\r\n";
            srt += "6\r\n00:00:11.630 --> 00:00:12.700\r\n" + l6.Text + "\r\n\r\n";
            srt += "7\r\n00:00:13.610 --> 00:00:16.010\r\n" + l7.Text + "\r\n\r\n";
            srt += "8\r\n00:00:18.080 --> 00:00:19.600\r\n" + l8.Text + "\r\n\r\n";

            File.WriteAllText(starpath + "\\ffmpeg\\wsyw.srt", srt);

            //转为ass字幕
            string Parameters1 = String.Format("-i " + starpath + "\\ffmpeg\\wsyw.srt -y " + starpath + "\\ffmpeg\\wsyw.ass");
            run.Execute(Parameters1, "ass");

        }
        #endregion

        #region 选择保存路径按钮点击
        private void savepath_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "GIF动态图|*.gif";
            if (sfd.ShowDialog() == true)
            {

                savepath.Text = sfd.FileName;
                gifsavepath = savepath.Text;
            }

        }
        #endregion
    }
}

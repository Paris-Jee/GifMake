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
using System.Windows.Threading;
using static xing_gifmake.Subtitle;

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

        //使用的模板视频文件
        string tp = "wsyw";

        //操作控制台
        Run run = new Run();

        //台词输入框数组
        List<TextBox> tblist = null;

        //台词总数
        int tcs = 0;

        //定时器，用于实时显示台词
        DispatcherTimer dt;

        public MainWindow()
        {

            InitializeComponent();

            ChanagedTemplate(0);

            run.RunedEvent += RunedEvent;

            //初始化计时器
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(0.5);

            dt.Tick += dt_tick;
            dt.Start();
            mediaElement.MediaEnded += (e, c) =>
            {
                mediaElement.Stop();
                mediaElement.Play();
            };
        }
        List<TC> tclist = new List<TC>();

        private void dt_tick(object sender, EventArgs e)
        {

            //定时刷新台词
            tctb.Text = Subtitle.GetTC(tclist,mediaElement.Position.TotalMilliseconds);
        }

        #region 指令运行完成事件监听
        private void RunedEvent(object rid)
        {
            Debug.WriteLine(rid);

            if (rid.ToString() == "ass")
            {

                string assstr = File.ReadAllText(starpath + "\\ffmpeg\\ass.ass");
                //因为输出图片尺寸过小所以这里调整字幕字体大小，从16>30，直接用简单粗暴的文本替换实现。
                assstr = assstr.Replace(",16,", ",30,");
                File.WriteAllText(starpath + "\\ffmpeg\\ass.ass", assstr);

                //将ass字幕嵌入视频
                string Parameters = String.Format("-i " + starpath + "\\ffmpeg\\" + tp + ".mp4 -vf ass=ass.ass -y " + starpath + "\\ffmpeg\\" + tp + "_cache.mp4");

                run.Execute(Parameters, "qr");

            }
            else if (rid.ToString() == "qr")
            {


                //嵌入字幕完成，导出gif
                string Parameters = String.Format(" -i " + starpath + "\\ffmpeg\\" + tp + "_cache.mp4 -y -s 406x224 " + gifsavepath);

                run.Execute(Parameters, "gif");

            }
            else if (rid.ToString() == "gif")
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {

                    //makebtn.IsEnabled = true;

                    //makebtn.Content = "生成GIF";
                    button.IsEnabled = true;
                    button.Content = "生成GIF";
                    settinggb.IsEnabled = true;

                    MessageBox.Show("GIF生成完毕~","久等");
                   

                }));
            }
        }
        #endregion





        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                ChanagedTemplate(comboBox.SelectedIndex);
            }
        }

        #region 变更模板
     

        public void ChanagedTemplate(int t)
        {

            switch (t)
            {
                case 0:
                    tcs = 8;
                    tp = "wsyw";
                    break;
                case 1:
                    tcs = 4;
                    tp = "wjz";
                    break;
                case 2:
                    tcs = 6;
                    tp = "dg";
                    break;
            }
            
            tclist = Subtitle.GetTCList(t);

            mediaElement.Source = new Uri(starpath + "\\ffmpeg\\" + tp + ".mp4", UriKind.RelativeOrAbsolute);
            mediaElement.Play();
            tblist = new List<TextBox>();
            sp.Children.Clear();
            for (int i = 0; i < tcs; i++)
            {

                sp.Children.Add(new Label()
                {
                    Content = "第 " + (1 + i) + " 句"
                });
                TextBox tb = new TextBox();
                tb.TextChanged += tboxtextchanged;
                tblist.Add(tb);
                sp.Children.Add(tb);
            }
        }

        private void tboxtextchanged(object sender, TextChangedEventArgs e)
        {
            for(int i = 0; i < tcs; i++)
            {
                tclist[i].tc = tblist[i].Text;
            }
        }
        #endregion
        #region 生成srt文件
        public void CreateSrt()
        {
            string[] tc = new string[tcs];
            for (int i = 0; i < tcs; i++)
            {
                tc[i] = tblist[i].Text;
            }
            if (comboBox.SelectedIndex == 0)
            {
                Subtitle.CreateSrt_Sorry(tc);
            }
            else if (comboBox.SelectedIndex == 1)
            {
                Subtitle.CreateSrt_WJZ(tc);
            }
            else if (comboBox.SelectedIndex == 2)
            {
                Subtitle.CreateSrt_DG(tc);
            }
        }
        #endregion

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "GIF动态图|*.gif";
            sfd.Title = "选择保存路径";
            if (sfd.ShowDialog() == true)
            {

                //textBox.Text = sfd.FileName;
                gifsavepath = sfd.FileName;
                //生成srt字幕
                CreateSrt();
                //转为ass字幕
                string Parameters1 = String.Format("-i " + starpath + "\\ffmpeg\\srt.srt -y " + starpath + "\\ffmpeg\\ass.ass");
                run.Execute(Parameters1, "ass");
                button.Content = "正在生成GIF...请等待";

                settinggb.IsEnabled = false;
                button.IsEnabled = false;
            }



        }

        private void textBox_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}

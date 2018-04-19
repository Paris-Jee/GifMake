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
        #region 一些变量
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

        //模板
        Template template;

        //定时器，用于实时刷新字幕
        DispatcherTimer dt;


        //王境泽台词时间戳
        string wjz_time = "00:00:00.00-00:00:01.04,00:00:01.46-00:00:03.02,00:00:03.09-00:00:04.50,00:00:05.00-00:00:06.00";
        //为所欲为
        string wsyw_time = "00:00:00.970-00:00:01.500,00:00:03.110-00:00:04.390,00:00:05.180-00:00:07.260,00:00:07.260-00:00:09.910,00:00:10.000-00:00:11.260,00:00:11.630-00:00:12.700,00:00:13.610-00:00:16.010,00:00:18.080-00:00:19.600";
        //打工
        string dg_time = "00:00:00.00-0:00:01.80,00:00:01.88-0:00:03.76,00:00:03.81-0:00:04.66,00:00:04.66-0:00:05.90,00:00:06.02-0:00:08.42,00:00:08.42-0:00:10.75";


        #endregion

        #region main
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
        #endregion

        #region 计时器刷新字幕
        private void dt_tick(object sender, EventArgs e)
        {
            if (template != null)
            {
                //定时刷新台词
                tctb.Text = template.GetSubtitle(mediaElement.Position.TotalMilliseconds);
            }
        }
        #endregion

        #region FFMPEG指令操作状态事件
        private void RunedEvent(object rid)
        {
            //完成srt转ass字幕操作
            if (rid.ToString() == "ass")
            {
                //因为输出图片尺寸过小会导致字幕看不清，所以这里调整字幕字体大小，从16>30，直接用简单粗暴的文本替换实现。
                string assstr = File.ReadAllText(starpath + "\\ffmpeg\\ass.ass");
                assstr = assstr.Replace(",16,", ",30,");

                File.WriteAllText(starpath + "\\ffmpeg\\ass.ass", assstr);

                //将ass字幕嵌入视频
                string Parameters = String.Format("-i " + starpath + "\\ffmpeg\\" + tp + ".mp4 -vf ass=ass.ass -y " + starpath + "\\ffmpeg\\" + tp + "_cache.mp4");

                run.Execute(Parameters, "qr");

            }
            else if (rid.ToString() == "qr")
            {


                //嵌入字幕完成，导出gif

                //406x224是代表导出分辨率为：宽406，高224像素的gif图像

                //压缩分辨率是为了减小gif图像体积方便用在qq上发送

                string Parameters = String.Format(" -i " + starpath + "\\ffmpeg\\" + tp + "_cache.mp4 -y -s 406x224 " + gifsavepath);

                run.Execute(Parameters, "gif");

            }
            else if (rid.ToString() == "gif")
            {
                

                this.Dispatcher.BeginInvoke(new Action(() =>
                {

                   

                    button.IsEnabled = true;
                    button.Content = "生成GIF";
                    settinggb.IsEnabled = true;

                    MessageBox.Show("GIF生成完毕~", "久等");


                }));
            }
        }
        #endregion




        #region 下拉选择框切换模板
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                ChanagedTemplate(comboBox.SelectedIndex);
            }
        }
        #endregion

        #region 变更模板方法
        public void ChanagedTemplate(int t)
        {

            switch (t)
            {
                case 0:

                    tp = "wsyw";

                    template = new Template(wsyw_time);

                    break;
                case 1:

                    tp = "wjz";

                    template = new Template(wjz_time);

                    break;
                case 2:

                    tp = "dg";

                    template = new Template(dg_time);

                    break;
            }


            //设置模板视频路径并播放
            mediaElement.Source = new Uri(starpath + "\\ffmpeg\\" + tp + ".mp4", UriKind.RelativeOrAbsolute);
            mediaElement.Play();

            //重新生成台词编辑界面
            tblist = new List<TextBox>();
            sp.Children.Clear();

            //动态添加台词输入框
            for (int i = 0; i < template.SubtitleCount; i++)
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

        #endregion

        #region 台词输入框内容变更事件
        private void tboxtextchanged(object sender, TextChangedEventArgs e)
        {


            for (int i = 0; i < template.SubtitleCount; i++)
            {
                //设置台词
                template.SetSubtitle(i, tblist[i].Text);
            }
        }
        #endregion

        //#region 生成srt文件
        //public void CreateSrt()
        //{
        //    string[] tc = new string[tcs];
        //    for (int i = 0; i < tcs; i++)
        //    {
        //        tc[i] = tblist[i].Text;
        //    }
        //    if (comboBox.SelectedIndex == 0)
        //    {
        //        Subtitle.CreateSrt_Sorry(tc);
        //    }
        //    else if (comboBox.SelectedIndex == 1)
        //    {
        //        Subtitle.CreateSrt_WJZ(tc);
        //    }
        //    else if (comboBox.SelectedIndex == 2)
        //    {
        //        Subtitle.CreateSrt_DG(tc);
        //    }
        //}
        //#endregion

        #region 生成按钮点击事件
        private void button_Click(object sender, RoutedEventArgs e)
        {


            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "GIF动态图|*.gif";
            sfd.Title = "选择保存路径";
            if (sfd.ShowDialog() == true)
            {

                //设置保存路径
                gifsavepath = sfd.FileName;
                //生成srt字幕文件
                if (template.CreateSrtFile())
                {
                    //转为ass字幕
                    string Parameters1 = String.Format("-i " + starpath + "\\ffmpeg\\srt.srt -y " + starpath + "\\ffmpeg\\ass.ass");
                    run.Execute(Parameters1, "ass");
                    button.Content = "正在生成GIF...请等待";

                    settinggb.IsEnabled = false;
                    button.IsEnabled = false;
                }
                else
                {
                    MessageBox.Show("生成srt文件时失败！");
                }
            }




        }
        #endregion



    }
}

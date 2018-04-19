using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xing_gifmake.Model;

namespace xing_gifmake
{
    public class Template
    {
        private string RunPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        //时间戳
        private string Timestamp { get; set; }

        /// <summary>
        /// 字幕集合
        /// </summary>
        private List<SubtitleModel> Subtitles = new List<SubtitleModel>();

        /// <summary>
        /// 台词总数
        /// </summary>
        public int SubtitleCount { get; set; }

        public Template(string Timestamp)
        {
            this.Timestamp = Timestamp;

            SubtitleCount = Timestamp.Split(',').Length;

            CreateSubtitleModel();
        }

        private void CreateSubtitleModel()
        {
            Subtitles.Clear();
            foreach (string t in Timestamp.Split(','))
            {
                string s = t.Split('-').First();
                string e = t.Split('-').Last();
                Subtitles.Add(new SubtitleModel()
                {
                    StartTime = s,
                    EndTime = e,
                    Subtitle = ""
                });
            }
        }
        /// <summary>
        /// 设置指定位置台词
        /// </summary>
        /// <param name="i"></param>
        /// <param name="str"></param>
        public void SetSubtitle(int i, string str)
        {
            Subtitles[i].Subtitle = str;
        }

        /// <summary>
        /// 获取台词
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetSubtitle(int i)
        {
            return Subtitles[i].Subtitle;
        }

        /// <summary>
        /// 通过当前时间戳获取台词
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public string GetSubtitle(double ms)
        {
            var r = "";
            var d = Subtitles.Where(m => m.StartTimeOfDouble <= ms && m.EndTimeOfDouble >= ms);
            if (d.Count() > 0)
            {
                r = d.First().Subtitle;
            }
            return r;
        }

        /// <summary>
        /// 创建srt字幕文件
        /// </summary>
        /// <returns></returns>
        public bool CreateSrtFile()
        {
            try
            {
                string srtstr = "";
                int i = 1;
                foreach (SubtitleModel sm in Subtitles)
                {
                    srtstr += i + "\r\n" + sm.StartTime + " --> " + sm.EndTime + "\r\n" + sm.Subtitle + "\r\n\r\n";
                    i++;
                }
                File.WriteAllText(RunPath + "\\ffmpeg\\srt.srt", srtstr);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

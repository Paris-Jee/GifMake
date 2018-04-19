using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xing_gifmake
{
    /// <summary>
    /// 已弃用（2018-4-19
    /// </summary>
    public class Subtitle
    {


        //获取当前应用程序启动目录
        static string starpath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        

        #region 生成sorry为所欲为字幕srt文件
        /// <summary>
        /// 生成sorry为所欲为字幕srt文件
        /// </summary>
        /// <param name="savepath"></param>
        /// <param name="tc">台词，共8句</param>
        /// <returns></returns>
        public static bool CreateSrt_Sorry(string[] tc)
        {
            if (tc.Length != 8)
            {
                return false;
            }
            else
            {
                try
                {
                    string srt = "1\r\n00:00:00.970 --> 00:00:01.500\r\n" + tc[0] + "\r\n\r\n";
                    srt += "2\r\n00:00:03.110 --> 00:00:04.390\r\n" + tc[1] + "\r\n\r\n";
                    srt += "3\r\n00:00:05.180 --> 00:00:07.260\r\n" + tc[2] + "\r\n\r\n";
                    srt += "4\r\n00:00:07.260 --> 00:00:09.910\r\n" + tc[3] + "\r\n\r\n";
                    srt += "5\r\n00:00:10.000 --> 00:00:11.260\r\n" + tc[4] + "\r\n\r\n";
                    srt += "6\r\n00:00:11.630 --> 00:00:12.700\r\n" + tc[5] + "\r\n\r\n";
                    srt += "7\r\n00:00:13.610 --> 00:00:16.010\r\n" + tc[6] + "\r\n\r\n";
                    srt += "8\r\n00:00:18.080 --> 00:00:19.600\r\n" + tc[7] + "\r\n\r\n";

                    File.WriteAllText(starpath + "\\ffmpeg\\srt.srt", srt);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        #endregion

        #region 生成王境泽srt文件
        public static bool CreateSrt_WJZ(string[] tc)
        {
            if (tc.Length != 4)
            {
                return false;
            }
            else
            {

                try
                {
                    string srt = "1\r\n00:00:00.00 --> 00:00:01.04\r\n" + tc[0] + "\r\n\r\n";
                    srt += "2\r\n00:00:01.46 --> 00:00:03.02\r\n" + tc[1] + "\r\n\r\n";
                    srt += "3\r\n00:00:03.09 --> 00:00:04.50\r\n" + tc[2] + "\r\n\r\n";
                    srt += "4\r\n00:00:05.00 --> 00:00:06.00\r\n" + tc[3] + "\r\n\r\n";
                    File.WriteAllText(starpath + "\\ffmpeg\\srt.srt", srt);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
        #endregion

        #region 生成打工srt文件
        public static bool CreateSrt_DG(string[] tc)
        {
            if (tc.Length != 6)
            {
                return false;
            }
            else
            {

                try
                {


                    string srt = "1\r\n00:00:00.00 --> 0:00:01.80\r\n" + tc[0] + "\r\n\r\n";
                    srt += "2\r\n00:00:01.88 --> 0:00:03.76\r\n" + tc[1] + "\r\n\r\n";
                    srt += "3\r\n00:00:03.81 --> 0:00:04.66\r\n" + tc[2] + "\r\n\r\n";
                    srt += "4\r\n00:00:04.66 --> 0:00:05.90\r\n" + tc[3] + "\r\n\r\n";
                    srt += "5\r\n00:00:06.02 --> 0:00:08.42\r\n" + tc[4] + "\r\n\r\n";
                    srt += "6\r\n00:00:08.42 --> 0:00:10.75\r\n" + tc[5] + "\r\n\r\n";
                    File.WriteAllText(starpath + "\\ffmpeg\\srt.srt", srt);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
        #endregion

        public static List<TC> GetTCList(int type)
        {
            List<TC> l = new List<TC>();
            if (type == 0)
            {
                l.Add(new TC()
                {
                    strstart = "00:00:00.970",
                    strend = "00:00:01.500"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:03.110",
                    strend = "00:00:04.390"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:05.180",
                    strend = "00:00:07.260"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:07.260",
                    strend = "00:00:09.910"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:10.000",
                    strend = "00:00:11.260"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:11.630",
                    strend = "00:00:12.700"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:13.610",
                    strend = "00:00:16.010"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:18.080",
                    strend = "00:00:19.600"
                });
            }
            else if (type == 1)
            {



                l.Add(new TC()
                {
                    strstart = "00:00:00.00",
                    strend = "00:00:01.04"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:01.46",
                    strend = "00:00:03.02"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:03.09",
                    strend = "00:00:04.50"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:05.00",
                    strend = "00:00:06.00"
                });
            }
            else if (type == 2)
            {

                l.Add(new TC()
                {
                    strstart = "00:00:00.00",
                    strend = "0:00:01.80"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:01.88",
                    strend = "0:00:03.76"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:03.81",
                    strend = "0:00:04.66"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:04.66",
                    strend = "0:00:05.90"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:06.02",
                    strend = "0:00:08.42"
                });
                l.Add(new TC()
                {
                    strstart = "00:00:08.42",
                    strend = "0:00:10.75"
                });
            }

            return l;
        }

        public class TC
        {
            private string strstart_;
            public string strstart
            {
                get
                {
                    return strstart_;
                }
                set
                {
                    strstart_ = value;
                    TimeSpan ts = new TimeSpan(0, 0, 0, int.Parse(value.Split(':').Last().Split('.').First()), int.Parse(value.Split('.').Last()));

                    start = ts.TotalMilliseconds;

                }
            }
            private string strend_;
            public string strend
            {
                get
                {
                    return strend_;
                }
                set
                {
                    strend_ = value;
                    TimeSpan ts = new TimeSpan(0, 0, 0, int.Parse(value.Split(':').Last().Split('.').First()), int.Parse(value.Split('.').Last()));

                    end = ts.TotalMilliseconds;
                }
            }

            public double start { get; set; }
            public double end { get; set; }
            public string tc { get; set; }




        }
        public static string GetTC(List<TC> tc, double ms)
        {
            var t = "";
            try
            {
                var c = tc.Where(m => ms >= m.start && ms <= m.end).First();
                t = c.tc;
            }
            catch
            {

            }


            return t;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xing_gifmake.Model
{
    public class SubtitleModel
    {
        private string StartTime_;
        public string StartTime
        {
            get
            {

                return StartTime_;
            }
            set
            {
                StartTime_ = value;
                StartTimeOfDouble = GetMilliseconds(value);
            }
        }
        private string EndTime_;
        public string EndTime { get
            {
                return EndTime_;
            }
            set
            {
                EndTime_ = value;
                EndTimeOfDouble = GetMilliseconds(value);
            }
        }

        public string Subtitle { get; set; }

        public double StartTimeOfDouble { get; set; }
        public double EndTimeOfDouble { get; set; }

        private double GetMilliseconds(string value)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0, int.Parse(value.Split(':').Last().Split('.').First()), int.Parse(value.Split('.').Last()));

            return ts.TotalMilliseconds;
        }
      
    }
}

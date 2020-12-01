using Newtonsoft.Json;
using System;
using YoYo_Web_App.Domain.JsonConverters;

namespace YoYo_Web_App.Domain.Models
{
    [JsonConverter(typeof(TimeJsonConverter))]
    public class Time
    {
        private int _hour;
        private int _minutes;

        public int Hours
        {
            get { return _hour; }
            set
            {
                // if (value > 23) throw new Exception("Invalid time specified.");
                _hour = value;
            }
        }

        public int Minutes
        {
            get { return _minutes; }
            set
            {
                // if (value > 59) throw new Exception("Invalid time specified.");
                _minutes = value;
            }
        }

        public Time()
        {

        }
        public Time(int h, int m)
        {
            //if (h > 23 || m > 59)
            //{
            //    throw new ArgumentException("Invalid time specified.");
            //}
            Hours = h; Minutes = m;
        }

        public void AddHours(int h)
        {
            this.Hours += h;
        }

        public void AddMinutes(int m)
        {
            this.Minutes += m;
            while (this.Minutes > 59)
            {
                this.Minutes -= 60;
                this.AddHours(1);
            }
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}", this.Hours, this.Minutes);
        }
    }
}

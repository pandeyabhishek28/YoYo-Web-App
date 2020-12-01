using System;

namespace YoYo_Web_App.Domain.Models
{
    public class TestInfo
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TestRunningStatus RunStatus { get; set; }

        /// <summary>
        /// Gets or sets the remaining time when next shuttle is going to start.
        /// </summary>
        public int NextShuttle { get; set; }
        public double Speed { get; set; }
        public int SpeedLevel { get; set; }
        public int ShuttleNo { get; set; }

        /// <summary>
        /// Gets or sets the total elapsed time in seconds.
        /// </summary>
        public double TotalElapsedTime { get; set; }
        public double TotalDistance { get; set; }
        public double LevelShuttleTime { get; set; }

        /// <summary>
        /// Gets or sets the elapsed time of current shuttle. 
        /// </summary>
        public double ElapsedShuttleTime { get; set; }
    }
}

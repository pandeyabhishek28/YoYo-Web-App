using System;
using System.ComponentModel.DataAnnotations;

namespace YoYo_Web_App.Domain.Models
{
    public class TestParticipantInfo
    {
        [Key]
        public Guid ID { get; set; }
        public Athlete Memeber { get; set; }
        public int SpeedLevel { get; set; }
        public int ShuttleNo { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Time TimeTaken { get; set; }
        public bool Started { get; set; }
        public bool Warned { get; set; }
        public bool Stopped { get; set; }
        public bool Completed { get; set; }
        public TestResult Result { get; set; }
    }
}

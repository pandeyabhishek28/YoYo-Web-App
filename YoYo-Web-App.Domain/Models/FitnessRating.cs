namespace YoYo_Web_App.Domain.Models
{
    public class FitnessRating
    {
        public int AccumulatedShuttleDistance { get; set; }
        public int SpeedLevel { get; set; }
        public int ShuttleNo { get; set; }
        public double Speed { get; set; }
        public double LevelTime { get; set; }
        public Time CommulativeTime { get; set; }
        public Time StartTime { get; set; }
        public double ApproxVo2Max { get; set; }
    }
}

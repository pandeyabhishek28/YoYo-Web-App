namespace YoYo_Web_App.DTOModels
{
    public class FitnessRatingDTO
    {
        public int AccumulatedShuttleDistance { get; set; }
        public int SpeedLevel { get; set; }
        public int ShuttleNo { get; set; }
        public double Speed { get; set; }
        public double LevelTime { get; set; }
        public TimeDTO CommulativeTime { get; set; }
        public TimeDTO StartTime { get; set; }
        public double ApproxVo2Max { get; set; }
    }
}

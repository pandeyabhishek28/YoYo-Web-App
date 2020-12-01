namespace YoYo_Web_App.DTOModels
{
    public class TimeDTO
    {
        public int Hours { get; set; }

        public int Minutes { get; set; }

        public override string ToString()
        {
            return $"{Hours.ToString("00")}:{Minutes.ToString("00")}";
        }
    }
}

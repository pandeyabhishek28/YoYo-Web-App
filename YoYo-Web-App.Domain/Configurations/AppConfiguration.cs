namespace YoYo_Web_App.Domain.Configurations
{
    public class AppConfiguration
    {
        public const string AppConfigKey = "AppConfig";
        public string DataFileName { get; set; }
        public string DataFilePath { get; set; }

        /// <summary>
        /// Gets or sets the time interval in seconds, it is in double so that we can set .5 or .3 as a value
        /// because we will be setting interval in milliseconds.
        /// </summary>
        public double TimeIntervalInSeconds { get; set; }
        public double KmPerHrToMPersConversationFactor { get; set; }
    }
}

namespace Server.Settings
{
    public class AppSettings
    {            
        public string Secret { get; set; } // Authentication Key
        public string UrlOrigin { get; set; } // Server URL
        public string UrlClient { get; set; } // Client URL
    }
}

namespace AOL_Portal.Configuration
{
    public class ApiConfig
    {
        public string ApiKey { get; set; }
        public string JwtSecret { get; set; }
        public int JwtExpiryMinutes { get; set; } = 60;
    }
}

using Microsoft.Extensions.Options;

namespace AOL_Portal.Configuration
{
    public class ApplicationConfig
    {
        public string CryptoKey { get; set; }
        public string CallbackUrl { get; set; }
        public string BaseUrl { get; set; }
        public int IsTestMode { get; set; }
       

    }
    public class EmailConfig
    {
        public string SmtpServer { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int SmtpPort { get; set; }
        public string FromEmail { get; set; }
    }
    public class ApplicationConfigService
    {

        public readonly ApplicationConfig ApplicationConfigSettings;
        public readonly EmailConfig EmailConfigSettings;
        public ApplicationConfigService(IOptions<ApplicationConfig> settings, IOptions<EmailConfig> emailConfigSettings)
        {
            this.ApplicationConfigSettings = settings.Value;
            this.EmailConfigSettings = emailConfigSettings.Value;
        }
    }
}

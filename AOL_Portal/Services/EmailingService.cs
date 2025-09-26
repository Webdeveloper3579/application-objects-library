using System.Net;
using System.Net.Mail;
using AOL_Portal.Configuration;

namespace AOL_Portal.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendMailRegistrationConfirmation(string callbackUrl, string toEmail, string userdisplayname, string token);
        Task SendMailResetPassword(string toEmail, string userdisplayname, string verificationCode);
    }
    public class EmailService(IConfiguration configuration, ApplicationConfigService appconfig) : IEmailService
    {
        private string _smtpServer = appconfig.EmailConfigSettings.SmtpServer;
        private int _smtpPort = appconfig.EmailConfigSettings.SmtpPort;
        private string _smtpUsername = appconfig.EmailConfigSettings.Username;
        private string _smtpPassword = CryptoClass.Decrypt(appconfig.ApplicationConfigSettings.CryptoKey, appconfig.EmailConfigSettings.Password);
        private string _fromEmail = appconfig.EmailConfigSettings.FromEmail;

        public async Task SendEmailAsync(string to, string subject, string body)
        {

            var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(_fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(to);

            await client.SendMailAsync(message);
        }
        public async Task SendMailRegistrationConfirmation(string callbackUrl, string toEmail, string userdisplayname, string token)
        {
            try
            {
                string url = $"{callbackUrl}RegisterConfirmation?token={token}";
                string body = $"Dear {userdisplayname}<br/><br/>You have been created as an user on the AOL Portal site.<br/><br/><br/><br/>Please click on the below link to complete your registration:<br/><br/> <a href ='{url}' title ='User Email Confirm'>Verify Email</a><br/><br/><br/> Kind Regards<br/><br/><br/>UMAS Admin Team";

                await SendEmailAsync(toEmail, "UMFAS Registration - Email confirmation", body).ConfigureAwait(false);
            }
            catch (Exception)
            {
            }
        }
        public async Task SendMailResetPassword(string toEmail, string userdisplayname, string verificationCode)
        {
            try
            {
                var emailBody = $@"
                        <h2>Password Reset</h2>
                        <p>Your verification code is: <strong>{verificationCode}</strong></p>
                        <p>Please use this code to reset your password.</p><br/><br/><br/> Kind Regards<br/><br/><br/>UMAS Admin Team"";";

                await SendEmailAsync(toEmail, "UMFAS - Password Reset Verification Code", emailBody).ConfigureAwait(false);
            }
            catch (Exception)
            {
            }
        }

        
    }
}

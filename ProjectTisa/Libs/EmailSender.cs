using System.Net.Mail;
using System.Net;
using ProjectTisa.Controllers.GeneralData.Configs;

namespace ProjectTisa.Libs
{
    /// <summary>
    /// Class for sending emails using SMTP server from appsettings.json
    /// </summary>
    public class EmailSender
    {
        private static readonly SmtpClient _smtpClient = new("smtp.gmail.com");
        private static string _fromEmail = string.Empty;
        public static void Configure(SmtpData smtpData)
        {
            _fromEmail = smtpData.FromEmail;
            _smtpClient.Port = smtpData.Port;
            _smtpClient.EnableSsl = smtpData.Ssl;
            _smtpClient.UseDefaultCredentials = smtpData.DefaultCredentais;
            _smtpClient.Credentials = new NetworkCredential(smtpData.FromEmail, smtpData.Password);
        }
        public static void SendEmailCode(string email, string code, string caption = "Email Verification", string message = "Your verification code is: {code}")
        {
            _smtpClient.Send(new MailMessage(_fromEmail, email, caption, message.Replace("{code}", code)));
        }
    }
}

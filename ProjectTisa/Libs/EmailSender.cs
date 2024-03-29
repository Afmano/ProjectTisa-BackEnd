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
        /// <summary>
        /// Initial configuration for sender. Call before sending emails.
        /// </summary>
        /// <param name="smtpData">Smtp configuration data from <c>appsetting.json</c>.</param>
        public static void Configure(SmtpData smtpData)
        {
            _fromEmail = smtpData.FromEmail;
            _smtpClient.Port = smtpData.Port;
            _smtpClient.EnableSsl = smtpData.Ssl;
            _smtpClient.UseDefaultCredentials = smtpData.DefaultCredentais;
            _smtpClient.Credentials = new NetworkCredential(smtpData.FromEmail, smtpData.Password);
        }
        /// <summary>
        /// Send email with string code to user's email.
        /// </summary>
        /// <param name="emailTo">Email to send.</param>
        /// <param name="code">Code to send via email.</param>
        /// <param name="caption">Caption of message.</param>
        /// <param name="message">Message of email with <c>{code}</c> pattern.</param>
        public static void SendEmailCode(string emailTo, string code, string caption = "Email Verification", string message = "Your verification code is: {code}")
        {
            _smtpClient.Send(new MailMessage(_fromEmail, emailTo, caption, message.Replace("{code}", code)));
        }
    }
}

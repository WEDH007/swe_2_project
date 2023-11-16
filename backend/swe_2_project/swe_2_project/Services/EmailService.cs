using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using swe_2_project.Models;

namespace swe_2_project.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailUsername"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config["EmailHost"], 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config["EmailUsername"], _config["EmailPassword"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}


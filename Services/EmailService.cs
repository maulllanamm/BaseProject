using DTO;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Services.Interface;

namespace Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailManagement _emailManagement;

        public EmailService(IOptions<EmailManagement> emailConfig)
        {
            _emailManagement = emailConfig.Value;
        }

        public void SendEmail(EmailDTO request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(request.From));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = request.Body
            };

            using var smtp = new SmtpClient();
            smtp.Connect(_emailManagement.Host, _emailManagement.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailManagement.Username, _emailManagement.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}

using ContactApp.Application.Dto;
using ContactApp.Application.Interfaces.Services;
using ContactApp.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace ContactApp.Infrastructure.External.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string email, string name, ContactDetailDto contactDetails)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                Subject = "Thank you for contacting us",
                Body = BuildEmailBody(name, contactDetails),
                IsBodyHtml = true
            };

            mail.To.Add(new MailAddress(email));
            using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = _emailSettings.UseSsl
            };

            await client.SendMailAsync(mail);
        }
        private string BuildEmailBody(string name, ContactDetailDto contactDetails)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"<h2>Dear {name},</h2>");
            builder.AppendLine("<p>Thank you for submitting your contact information.</p>");

            if (contactDetails != null)
            {
                builder.AppendLine("<h3>Additional Information:</h3>");
                builder.AppendLine("<ul>");

                if (!string.IsNullOrEmpty(contactDetails.Phone))
                    builder.AppendLine($"<li>Phone: {contactDetails.Phone}</li>");

                if (!string.IsNullOrEmpty(contactDetails.Website))
                    builder.AppendLine($"<li>Website: {contactDetails.Website}</li>");

                if (contactDetails.Address != null)
                {
                    builder.AppendLine("<li>Address:<ul>");
                    if (!string.IsNullOrEmpty(contactDetails.Address.Street))
                        builder.AppendLine($"<li>{contactDetails.Address.Street}</li>");
                    if (!string.IsNullOrEmpty(contactDetails.Address.City))
                        builder.AppendLine($"<li>{contactDetails.Address.City}</li>");
                    builder.AppendLine("</ul></li>");
                }

                builder.AppendLine("</ul>");
            }

            builder.AppendLine("<p>We will be in touch with you shortly.</p>");
            builder.AppendLine("<p>Best regards,<br>The Contact App Team</p>");

            return builder.ToString();
        }
    }
}

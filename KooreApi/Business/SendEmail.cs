// using SendGrid's C# Library
// https://github.com/sendgrid/sendgrid-csharp
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace KooreApi
{
    class Email
    {
        public static async Task Execute(string subject, string name, string email, string htmlContent)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("rodrigo@diversetechlab.com", "no-reply");
            var to = new EmailAddress(email, name);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
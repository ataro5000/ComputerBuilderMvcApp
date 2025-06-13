using Microsoft.AspNetCore.Identity.UI.Services; // Required for IEmailSender
using SendGrid;
using SendGrid.Helpers.Mail;


namespace ComputerBuilderMvcApp.Services
{
   
    public class EmailSender(IConfiguration configuration) : IEmailSender
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Use _configuration to get the API key from secrets.json or other config sources
            var apiKey = _configuration["SendGridApiKey"]; 
            var fromEmailConfig = _configuration["SendGridFromEmail"];
            var fromNameConfig = _configuration["SendGridFromName"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("SendGrid API key is not configured.");
            }
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmailConfig, fromNameConfig ?? "Your Application Name"); // Use configured name or a default
            var to = new EmailAddress(email); 
            var plainTextContent = htmlMessage;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlMessage);

            try
            {
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"EmailSender: Email to {to.Email} reported as sent successfully by SendGrid!");
                }
                else
                {
                    var responseBody = await response.Body.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
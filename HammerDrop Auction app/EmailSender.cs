using HammerDrop_Auction_app;
using System.Net;
using System.Net.Mail;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(
                    _config["EmailSettings:Email"],
                    _config["EmailSettings:Password"]
                ),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["EmailSettings:Email"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage); 

        }
        catch (Exception ex)
        {
            Console.WriteLine("EMAIL ERROR FULL STACK:");
            Console.WriteLine(ex.ToString());

            throw new Exception("Email sending failed: " + ex.Message, ex);
        }
    }
}

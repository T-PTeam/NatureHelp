using Infrastructure.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public SmtpEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(string toEmail, string subject, string htmlContent)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:From"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = htmlContent
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_configuration["EmailSettings:Host"],
                                int.Parse(_configuration["EmailSettings:Port"]),
                                SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"],
                                     _configuration["EmailSettings:Password"]);

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}

using Application.Interfaces.Services.Organization;
using Infrastructure.Interfaces;

namespace Application.Services.Organization;
public class EmailService : IEmailService
{
    private readonly IEmailSender _emailSender;

    public EmailService(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
    {
        if (string.IsNullOrWhiteSpace(toEmail) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(htmlContent))
        {
            throw new ArgumentException("Email parameters cannot be null or empty.");
        }

        await _emailSender.SendAsync(toEmail, subject, htmlContent);
    }
}

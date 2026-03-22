namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(string toEmail, string confirmationUrl);
    Task SendPasswordResetEmailAsync(string toEmail, string resetUrl);
}
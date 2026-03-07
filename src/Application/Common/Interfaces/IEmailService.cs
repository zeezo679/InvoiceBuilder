namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(string toEmail, string subject, string verificationUri);
    
}
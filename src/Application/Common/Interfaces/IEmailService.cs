namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(string email, string confirmationToken);
}
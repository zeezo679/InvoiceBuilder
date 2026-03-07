using Application.Common.Interfaces;
using Application.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailOptions _options;

    public EmailService(IOptions<EmailOptions> options)
    {
        _options = options.Value;
    }

    public async Task SendConfirmationEmailAsync(string toEmail, string subject, string verificationUri)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(_options.FromName ?? string.Empty, _options.FromEmail ?? string.Empty));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        
        message.Body = new BodyBuilder
        {
            HtmlBody = GenerateHtmlBody(verificationUri)
        }.ToMessageBody();

        using var client = new SmtpClient();

        await client.ConnectAsync(
            _options.Host,
            _options.Port,
            SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(_options.Username, _options.Password);

        await client.SendAsync(message);

        await client.DisconnectAsync(true);
    }

    private string GenerateHtmlBody(string verificationUri)
    {
        return $"""
                <h2>Verify your email</h2>
                <p>Click the button below to verify your account.</p>
                <a href="{verificationUri}">Verify Email</a>
                <p>This link expires in 24 hours.</p>
                <p>If you didn't create this account, ignore this email.</p>
                """;
    }
}
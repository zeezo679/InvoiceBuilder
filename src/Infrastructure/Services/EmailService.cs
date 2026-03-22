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

    public Task SendConfirmationEmailAsync(string toEmail, string confirmationUrl)
    {
        return SendEmailAsync(
            toEmail,
            subject: "Verify your Email Address",
            uri: confirmationUrl,
            bodyGenerator: GenerateConfirmationHtmlBody);
    }

    public Task SendPasswordResetEmailAsync(string toEmail, string resetUrl)
    {
        return SendEmailAsync(
            toEmail,
            subject: "Reset your Password",
            uri: resetUrl,
            bodyGenerator: GeneratePasswordResetHtmlBody);
    }

    // Private core — all SMTP logic lives here once
    private async Task SendEmailAsync(
        string toEmail,
        string subject,
        string uri,
        Func<string, string> bodyGenerator)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
            throw new ArgumentException("Email address cannot be empty.", nameof(toEmail));

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.FromName ?? string.Empty, _options.FromEmail ?? string.Empty));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new BodyBuilder
        {
            HtmlBody = bodyGenerator(uri) // ← call the delegate
        }.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_options.Username, _options.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    private string GenerateConfirmationHtmlBody(string uri) => $"""
                                                                <h2>Verify your email</h2>
                                                                <p>Click the button below to verify your account.</p>
                                                                <a href="{uri}">Verify Email</a>
                                                                <p>This link expires in 24 hours.</p>
                                                                <p>If you didn't create this account, ignore this email.</p>
                                                                """;

    private string GeneratePasswordResetHtmlBody(string uri) => $"""
                                                                 <h2>Reset your password</h2>
                                                                 <p>Click the button below to reset your password.</p>
                                                                 <a href="{uri}">Reset Password</a>
                                                                 <p>This link expires in 1 hour.</p>
                                                                 <p>If you didn't request this, ignore this email.</p>
                                                                 """;
}
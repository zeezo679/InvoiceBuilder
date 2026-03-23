namespace Api.requests;

public record EmailVerificationRequest(string email, string Token);
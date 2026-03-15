namespace Api.requests;

public record EmailVerificationRequest(string UserId, string Token);
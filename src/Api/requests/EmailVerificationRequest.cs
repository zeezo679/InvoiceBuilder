namespace Api.requests;

public class EmailVerificationRequest
{
    public string UserId { get; set; }
    public string Token { get; set; }
}
namespace Api.requests;

public record ResetPasswordRequestBody(string NewPassword, string ConfirmPassword);
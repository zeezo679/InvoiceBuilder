namespace Application.Auth.VerifiyEmail;
using MediatR;
using ErrorOr;


public sealed record VerifyEmailCommand(string UserId, string Token) :
    IRequest<ErrorOr<Unit>>;
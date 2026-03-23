namespace Application.Auth.VerifiyEmail;
using MediatR;
using ErrorOr;


public sealed record VerifyEmailCommand(string email, string Token) :
    IRequest<ErrorOr<Unit>>;
namespace Application.Auth.Register;

using MediatR;
using ErrorOr;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<RegisterResult>>;

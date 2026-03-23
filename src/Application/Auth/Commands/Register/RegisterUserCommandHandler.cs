using Application.Common.Interfaces;

namespace Application.Auth.Register;

using MediatR;
using ErrorOr;
using Microsoft.AspNetCore.Identity;


public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ErrorOr<RegisterResult>>
{
    private readonly IIdentityService _identityService;

    public RegisterUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<ErrorOr<RegisterResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        //check if user already exists
        var userExists = await _identityService.UserExistsAsync(request.Email);
        if (userExists)
            return Error.Conflict("User.DuplicateEmail", "user with this email already exists");
        
        var result = await _identityService.CreateUserAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);
        

        return result.Match<ErrorOr<RegisterResult>>(
            userId => new RegisterResult(userId, request.Email),
            errors => errors);
    }
}
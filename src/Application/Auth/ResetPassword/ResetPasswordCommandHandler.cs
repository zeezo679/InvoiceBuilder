using Application.Common.Interfaces;
using MediatR;
using ErrorOr;

namespace Application.Auth.ResetPassword;

public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ErrorOr<Unit>>
{
    private readonly IIdentityService _identityService;

    public ResetPasswordCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ErrorOr<Unit>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword, request.ConfirmPassword);
        
        if (result.IsError)
            return Error.Failure(description: "Failed to reset password");
        
        return Unit.Value;
    }
}
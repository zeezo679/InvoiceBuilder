using Application.Common.Interfaces;
using MediatR;
using ErrorOr;

namespace Application.Auth.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand,ErrorOr<Unit>>
{
    private readonly IIdentityService _identityService;

    public ForgotPasswordCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ErrorOr<Unit>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.ForgotPasswordAsync(request.Email);

        if (result.IsError)
            return Error.Failure(description: "Failed to process forgot password request");
        
        return Unit.Value;
    }
}
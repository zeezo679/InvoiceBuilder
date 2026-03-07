using System.Text;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace Application.Auth.VerifiyEmail;
using ErrorOr;
using MediatR;

public sealed class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, ErrorOr<Unit>>
{
    private readonly IIdentityService _identityService;

    public VerifyEmailCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<ErrorOr<Unit>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

        var result = await _identityService.ConfirmEmailAsync(request.UserId, decodedToken);

        if (!result)
            return Error.Failure(description: "Invalid or Expired verification link");

        //success
        return Unit.Value;
    }
}
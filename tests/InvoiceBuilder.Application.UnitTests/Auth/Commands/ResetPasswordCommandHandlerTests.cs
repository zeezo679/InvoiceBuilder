using System;
using Application.Auth.ResetPassword;
using Application.Common.Interfaces;
using ErrorOr;
using FluentAssertions;
using MediatR;
using Moq;

namespace InvoiceBuilder.Application.UnitTests.Auth.Commands;

public class ResetPasswordCommandHandlerTests
{
[Fact]
public async Task Handle_WhenResetPasswordSucceeds_ReturnsUnit()
{
    // Arrange
    var identityServiceMock = new Mock<IIdentityService>();
    identityServiceMock
        .Setup(s => s.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(Result.Success);

    var handler = new ResetPasswordCommandHandler(identityServiceMock.Object);
    var command = new ResetPasswordCommand(
        Email: "john.doe@example.com",
        Token: "valid-token",
        NewPassword: "NewPassword123!",
        ConfirmPassword: "NewPassword123!"
    );

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsError.Should().BeFalse();
    result.Value.Should().Be(Unit.Value);
}

[Fact]
public async Task Handle_WhenResetPasswordFails_ReturnsFailureError()
{
    // Arrange
    var identityServiceMock = new Mock<IIdentityService>();
    identityServiceMock
        .Setup(s => s.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(Error.Failure());

    var handler = new ResetPasswordCommandHandler(identityServiceMock.Object);
    var command = new ResetPasswordCommand(
        Email: "john.doe@example.com",
        Token: "invalid-token",
        NewPassword: "NewPassword123!",
        ConfirmPassword: "NewPassword123!"
    );

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsError.Should().BeTrue();
    result.FirstError.Type.Should().Be(ErrorType.Failure);
    result.FirstError.Description.Should().Be("Failed to reset password");
}
}

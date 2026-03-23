using System;
using Application.Auth.ForgotPassword;
using Application.Common.Interfaces;
using ErrorOr;
using FluentAssertions;
using MediatR;
using Moq;

namespace InvoiceBuilder.Application.UnitTests.Auth.Commands;

public class ForgotPasswordCommandHandlerTests
{
    [Fact]
public async Task Handle_WhenForgotPasswordSucceeds_ReturnsUnit()
{
    // Arrange
    var identityServiceMock = new Mock<IIdentityService>();
    identityServiceMock
        .Setup(s => s.ForgotPasswordAsync(It.IsAny<string>()))
        .ReturnsAsync(Result.Success);

    var handler = new ForgotPasswordCommandHandler(identityServiceMock.Object);
    var command = new ForgotPasswordCommand(Email: "john.doe@example.com");

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsError.Should().BeFalse();
    result.Value.Should().Be(Unit.Value);
}

[Fact]
public async Task Handle_WhenForgotPasswordFails_ReturnsFailureError()
{
    // Arrange
    var identityServiceMock = new Mock<IIdentityService>();
    identityServiceMock
        .Setup(s => s.ForgotPasswordAsync(It.IsAny<string>()))
        .ReturnsAsync(ErrorOr.Error.Failure());

    var handler = new ForgotPasswordCommandHandler(identityServiceMock.Object);
    var command = new ForgotPasswordCommand(Email: "john.doe@example.com");

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsError.Should().BeTrue();
    result.FirstError.Type.Should().Be(ErrorType.Failure);
    result.FirstError.Description.Should().Be("Failed to process forgot password request");
}
}

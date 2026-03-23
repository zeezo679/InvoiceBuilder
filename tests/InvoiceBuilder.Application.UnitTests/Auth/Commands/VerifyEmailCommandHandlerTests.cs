using System;
using System.Text;
using Application.Auth.VerifiyEmail;
using Application.Common.Interfaces;
using ErrorOr;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Moq;

namespace InvoiceBuilder.Application.UnitTests.Auth.Commands;

public class VerifyEmailCommandHandlerTests
{
    [Fact]
public async Task Handle_WhenEmailVerificationSucceeds_ReturnsUnit()
{
    // Arrange
    var identityServiceMock = new Mock<IIdentityService>();
    identityServiceMock
        .Setup(s => s.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(true);

    var handler = new VerifyEmailCommandHandler(identityServiceMock.Object);
    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("valid-token")); //encoding must be done also to avoid the test from failing due to token format issues
    var command = new VerifyEmailCommand(
        email: "john.doe@example.com",
        Token: encodedToken
    );

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsError.Should().BeFalse();
    result.Value.Should().Be(Unit.Value);
}

[Fact]
public async Task Handle_WhenEmailVerificationFails_ReturnsFailureError()
{
    // Arrange
    var identityServiceMock = new Mock<IIdentityService>();
    identityServiceMock
        .Setup(s => s.ConfirmEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync(false);

    var handler = new VerifyEmailCommandHandler(identityServiceMock.Object);
    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("invalid-token"));
    var command = new VerifyEmailCommand(
        email: "john.doe@example.com",
        Token: encodedToken
    );

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    result.IsError.Should().BeTrue();
    result.FirstError.Type.Should().Be(ErrorType.Failure);
    result.FirstError.Description.Should().Be("Invalid or Expired verification link");
}
}

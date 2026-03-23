using System;
using Application.Auth.Register;
using Application.Common.Interfaces;
using Moq;
using FluentAssertions;
using ErrorOr;

namespace InvoiceBuilder.Application.UnitTests.Auth.Commands;


public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var identityServiceMock = new Mock<IIdentityService>();
        identityServiceMock.Setup(s => s.UserExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
        
        var handler = new RegisterUserCommandHandler(identityServiceMock.Object);
        var command = new RegisterUserCommand
        (
            FirstName: "John",
            LastName: "Doe",
            Email: "john.doe@example.com",
            Password: "Password123!"
        );  

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue(); //means that the result is an error
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task Handle_WhenUserIsCreatedSuccessfully_ReturnsSuccess()
    {
        //Arrange
        var identityServiceMock = new Mock<IIdentityService>();

        identityServiceMock
        .Setup(s => s.UserExistsAsync(It.IsAny<string>()))
        .ReturnsAsync(false);

        identityServiceMock
        .Setup(s => s.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        .ReturnsAsync("new-user-id");
        
        var handler = new RegisterUserCommandHandler(identityServiceMock.Object);
        var command = new RegisterUserCommand
        (
            FirstName: "John",
            LastName: "Doe",
            Email: "john.doe@example.com",
            Password: "Password123!"
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();    
        result.Value.UserId.Should().Be("new-user-id");
        result.Value.Email.Should().Be("john.doe@example.com");


        // this will fail because it is comparing another instance of RegisterResult, we need to compare the properties instead
        // result.Value.Should().Be(new RegisterResult(email: "john.doe@example.com", userId: "new-user-id")); 
    }
}

using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Users.CreateUser;

public partial class CreateUserHandler(
    IUserRepository userRepository,
    ILogger<CreateUserHandler> logger) : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        LogStartingCreateUserHandlerForEmail(logger, request.Email);

        try
        {
            // Check if user already exists
            var existingUser = await userRepository.GetUserByEmail(request.Email);
            if (existingUser != null)
            {
                LogUserCreationFailedEmailAlreadyExists(logger, request.Email);
                return new CreateUserResult
                {
                    Success = false,
                    ErrorMessage = "A user with this email already exists."
                };
            }

            // Hash the password using BCrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Create the user entity - navigation properties will be loaded by EF Core
            var user = new User
            {
                Email = request.Email,
                Password = hashedPassword,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CountryId = request.CountryId,
                SubdivisionId = request.SubdivisionId,
                RoleId = 1, // Default role ID (should be seeded as "User" role)
                LanguageCode = "en", // Default language is English
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Role = null!, // Will be loaded by EF Core
                Country = null!, // Will be loaded by EF Core
                Language = null! // Will be loaded by EF Core
            };

            // Save the user
            var createdUser = await userRepository.CreateUser(user);

            LogSuccessfullyCreatedUserWithIdUseridForEmail(logger, createdUser.Id, request.Email);

            return new CreateUserResult
            {
                Success = true,
                UserId = createdUser.Id
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurredWhileCreatingUserForEmail(logger, ex.Message, request.Email);
            return new CreateUserResult
            {
                Success = false,
                ErrorMessage = "An error occurred while creating your account. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateUser handler for email: {email}")]
    static partial void LogStartingCreateUserHandlerForEmail(ILogger<CreateUserHandler> logger, string email);

    [LoggerMessage(LogLevel.Information, "User creation failed - email already exists: {Email}")]
    static partial void LogUserCreationFailedEmailAlreadyExists(ILogger<CreateUserHandler> logger, string Email);

    [LoggerMessage(LogLevel.Information, "Successfully created user with ID: {UserId} for email: {Email}")]
    static partial void LogSuccessfullyCreatedUserWithIdUseridForEmail(ILogger<CreateUserHandler> logger, long UserId, string Email);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating user for email: {Email} | Exception: {Exception}")]
    static partial void LogErrorOccurredWhileCreatingUserForEmail(ILogger<CreateUserHandler> logger, string Exception, string Email);
}


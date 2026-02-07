using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Users.Login;

public partial class LoginHandler(
    IUserRepository userRepository,
    ILogger<LoginHandler> logger) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        LogStartingLoginHandler(logger, request.Email);

        try
        {
            // Get user by email
            var user = await userRepository.GetUserByEmail(request.Email);
            if (user == null)
            {
                LogLoginFailedInvalidCredentials(logger, request.Email);
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = "Invalid credentials."
                };
            }

            // Verify password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isPasswordValid)
            {
                LogLoginFailedInvalidCredentials(logger, request.Email);
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = "Invalid credentials."
                };
            }

            // Check if user is active
            if (!user.IsActive)
            {
                LogLoginFailedUserInactive(logger, request.Email);
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = "Your account has been deactivated."
                };
            }

            LogLoginSuccessful(logger, user.Id, user.Email);

            return new LoginResult
            {
                Success = true,
                UserId = user.Id,
                FirstName = user.FirstName,
                RoleId = user.RoleId,
                RoleName = user.Role.Name
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurredDuringLogin(logger, ex.Message, request.Email);
            return new LoginResult
            {
                Success = false,
                ErrorMessage = "An error occurred during login. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting Login handler for email: {email}")]
    static partial void LogStartingLoginHandler(ILogger<LoginHandler> logger, string email);

    [LoggerMessage(LogLevel.Warning, "Login failed - invalid credentials for email: {email}")]
    static partial void LogLoginFailedInvalidCredentials(ILogger<LoginHandler> logger, string email);

    [LoggerMessage(LogLevel.Warning, "Login failed - user inactive for email: {email}")]
    static partial void LogLoginFailedUserInactive(ILogger<LoginHandler> logger, string email);

    [LoggerMessage(LogLevel.Information, "Successfully logged in user {userId} - {email}")]
    static partial void LogLoginSuccessful(ILogger<LoginHandler> logger, long userId, string email);

    [LoggerMessage(LogLevel.Error, "Error occurred during login for email: {email} | Exception: {exception}")]
    static partial void LogErrorOccurredDuringLogin(ILogger<LoginHandler> logger, string exception, string email);
}


using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Users.Login;

public partial class LoginHandler(
    IUserRepository userRepository,
    ISessionRepository sessionRepository,
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

            // Create session
            var session = new Session
            {
                SessionId = Guid.NewGuid(),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddMinutes(30),
                User = user
            };

            var createdSession = await sessionRepository.CreateSession(session);

            LogLoginSuccessful(logger, user.Id, createdSession.SessionId.ToString());

            return new LoginResult
            {
                Success = true,
                SessionToken = createdSession.SessionId.ToString(),
                UserId = user.Id
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

    [LoggerMessage(LogLevel.Information, "Starting Login handler for email: {Email}")]
    static partial void LogStartingLoginHandler(ILogger<LoginHandler> logger, string Email);

    [LoggerMessage(LogLevel.Warning, "Login failed - invalid credentials for email: {Email}")]
    static partial void LogLoginFailedInvalidCredentials(ILogger<LoginHandler> logger, string Email);

    [LoggerMessage(LogLevel.Warning, "Login failed - user inactive for email: {Email}")]
    static partial void LogLoginFailedUserInactive(ILogger<LoginHandler> logger, string Email);

    [LoggerMessage(LogLevel.Information, "Successfully logged in user {UserId} with session {SessionId}")]
    static partial void LogLoginSuccessful(ILogger<LoginHandler> logger, long UserId, string SessionId);

    [LoggerMessage(LogLevel.Error, "Error occurred during login for email: {Email} | Exception: {Exception}")]
    static partial void LogErrorOccurredDuringLogin(ILogger<LoginHandler> logger, string Exception, string Email);
}


using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Users.ValidateSession;

public partial class ValidateSessionHandler(
    ISessionRepository sessionRepository,
    ILogger<ValidateSessionHandler> logger) : IRequestHandler<ValidateSessionQuery, ValidateSessionResult>
{
    public async Task<ValidateSessionResult> Handle(ValidateSessionQuery request, CancellationToken cancellationToken)
    {
        LogStartingValidateSessionHandler(logger);

        try
        {
            var isValid = await sessionRepository.ValidateSession(request.SessionToken);

            if (isValid)
            {
                // Refresh the session timer
                await sessionRepository.RefreshSession(request.SessionToken);
                
                var session = await sessionRepository.GetSessionByToken(request.SessionToken);
                LogSessionValidationSuccessful(logger, session?.UserId ?? 0);
                
                return new ValidateSessionResult
                {
                    IsValid = true,
                    UserId = session?.UserId
                };
            }

            LogSessionValidationFailed(logger);
            return new ValidateSessionResult
            {
                IsValid = false
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurredDuringSessionValidation(logger, ex.Message);
            return new ValidateSessionResult
            {
                IsValid = false
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting ValidateSession handler")]
    static partial void LogStartingValidateSessionHandler(ILogger<ValidateSessionHandler> logger);

    [LoggerMessage(LogLevel.Information, "Session validation successful for UserId: {userId}")]
    static partial void LogSessionValidationSuccessful(ILogger<ValidateSessionHandler> logger, long userId);

    [LoggerMessage(LogLevel.Warning, "Session validation failed - invalid or expired session")]
    static partial void LogSessionValidationFailed(ILogger<ValidateSessionHandler> logger);

    [LoggerMessage(LogLevel.Error, "Error occurred during session validation | Exception: {exception}")]
    static partial void LogErrorOccurredDuringSessionValidation(ILogger<ValidateSessionHandler> logger, string exception);
}


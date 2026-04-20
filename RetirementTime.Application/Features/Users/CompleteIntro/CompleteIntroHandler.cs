using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Users.CompleteIntro;

public partial class CompleteIntroHandler(
    IUserRepository userRepository,
    ILogger<CompleteIntroHandler> logger) : IRequestHandler<CompleteIntroCommand, CompleteIntroResult>
{
    public async Task<CompleteIntroResult> Handle(CompleteIntroCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.UserId);

        try
        {
            var success = await userRepository.UpdateHasCompletedIntro(request.UserId, request.HasCompletedIntro);

            if (!success)
            {
                LogUserNotFound(logger, request.UserId);
                return new CompleteIntroResult
                {
                    Success = false,
                    ErrorMessage = "An error occurred. Please try again later."
                };
            }

            LogSuccessfullyCompleted(logger, request.UserId);
            return new CompleteIntroResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new CompleteIntroResult
            {
                Success = false,
                ErrorMessage = "An error occurred. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CompleteIntro handler for UserId: {UserId}")]
    static partial void LogStartingHandler(ILogger<CompleteIntroHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully completed intro for UserId: {UserId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CompleteIntroHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Warning, "CompleteIntro failed - user not found for UserId: {UserId}")]
    static partial void LogUserNotFound(ILogger<CompleteIntroHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Error, "Error occurred in CompleteIntro for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CompleteIntroHandler> logger, string Exception, long UserId);
}


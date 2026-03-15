using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.UserProgress.GetUserProgress;

public partial class GetUserProgressHandler(
    IUserProgressRepository userProgressRepository,
    ILogger<GetUserProgressHandler> logger) : IRequestHandler<GetUserProgressQuery, GetUserProgressResult>
{
    public async Task<GetUserProgressResult> Handle(GetUserProgressQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.UserId);

        try
        {
            var progress = await userProgressRepository.GetByUserId(request.UserId);

            LogSuccessfullyCompleted(logger, request.UserId);

            return new GetUserProgressResult
            {
                Success = true,
                HasFinishedBeginnerGuide = progress?.HasFinishedBeginnerGuide ?? false
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new GetUserProgressResult
            {
                Success = false,
                ErrorMessage = "An error occurred. Please try again later.",
                HasFinishedBeginnerGuide = false
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetUserProgress handler for UserId: {UserId}")]
    static partial void LogStartingHandler(ILogger<GetUserProgressHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully completed GetUserProgress for UserId: {UserId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetUserProgressHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Error, "Error occurred in GetUserProgress for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetUserProgressHandler> logger, string Exception, long UserId);
}


using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.UserProgress.CompleteBeginnerGuide;

public partial class CompleteBeginnerGuideHandler(
    IUserProgressRepository userProgressRepository,
    IUserRepository userRepository,
    ILogger<CompleteBeginnerGuideHandler> logger) : IRequestHandler<CompleteBeginnerGuideCommand, CompleteBeginnerGuideResult>
{
    public async Task<CompleteBeginnerGuideResult> Handle(CompleteBeginnerGuideCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.UserId);

        try
        {
            var user = await userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                return new CompleteBeginnerGuideResult
                {
                    Success = false,
                    ErrorMessage = "User not found."
                };
            }

            var progress = new RetirementTime.Domain.Entities.UserProgress
            {
                UserId = request.UserId,
                HasFinishedBeginnerGuide = true,
                User = user
            };

            await userProgressRepository.Upsert(progress);

            LogSuccessfullyCompleted(logger, request.UserId);

            return new CompleteBeginnerGuideResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new CompleteBeginnerGuideResult
            {
                Success = false,
                ErrorMessage = "An error occurred. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CompleteBeginnerGuide handler for UserId: {userId}")]
    static partial void LogStartingHandler(ILogger<CompleteBeginnerGuideHandler> logger, long userId);

    [LoggerMessage(LogLevel.Information, "Successfully completed BeginnerGuide for UserId: {userId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CompleteBeginnerGuideHandler> logger, long userId);

    [LoggerMessage(LogLevel.Error, "Error occurred in CompleteBeginnerGuide for UserId: {userId} | Exception: {exception}")]
    static partial void LogErrorOccurred(ILogger<CompleteBeginnerGuideHandler> logger, string exception, long userId);
}


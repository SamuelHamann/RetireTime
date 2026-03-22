using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Users.GetIntroStatus;

public partial class GetIntroStatusHandler(
    IUserRepository userRepository,
    ILogger<GetIntroStatusHandler> logger) : IRequestHandler<GetIntroStatusQuery, GetIntroStatusResult>
{
    public async Task<GetIntroStatusResult> Handle(GetIntroStatusQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.UserId);

        try
        {
            var user = await userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                LogUserNotFound(logger, request.UserId);
                return new GetIntroStatusResult { Success = false, ErrorMessage = "User not found." };
            }

            LogSuccessfullyCompleted(logger, request.UserId, user.HasCompletedIntro);
            return new GetIntroStatusResult { Success = true, HasCompletedIntro = user.HasCompletedIntro };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new GetIntroStatusResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetIntroStatus handler for UserId: {UserId}")]
    static partial void LogStartingHandler(ILogger<GetIntroStatusHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved intro status for UserId: {UserId} — HasCompletedIntro: {HasCompletedIntro}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetIntroStatusHandler> logger, long UserId, bool HasCompletedIntro);

    [LoggerMessage(LogLevel.Warning, "GetIntroStatus failed — user not found for UserId: {UserId}")]
    static partial void LogUserNotFound(ILogger<GetIntroStatusHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Error, "Error occurred in GetIntroStatus for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetIntroStatusHandler> logger, string Exception, long UserId);
}


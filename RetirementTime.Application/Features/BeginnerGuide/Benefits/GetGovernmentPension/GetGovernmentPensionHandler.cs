using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.GetGovernmentPension;

public partial class GetGovernmentPensionHandler(
    IBeginnerGuideGovernmentPensionRepository governmentPensionRepository,
    ILogger<GetGovernmentPensionHandler> logger) : IRequestHandler<GetGovernmentPensionQuery, GovernmentPensionDto?>
{
    public async Task<GovernmentPensionDto?> Handle(GetGovernmentPensionQuery request, CancellationToken cancellationToken)
    {
        LogStartingQuery(logger, request.UserId);

        try
        {
            var governmentPension = await governmentPensionRepository.GetByUserIdAsync(request.UserId);

            if (governmentPension is null)
            {
                LogNoneFound(logger, request.UserId);
                return null;
            }

            var result = new GovernmentPensionDto
            {
                Id = governmentPension.Id,
                YearsWorked = governmentPension.YearsWorked,
                HasSpecializedPublicSectorPension = governmentPension.HasSpecializedPublicSectorPension,
                SpecializedPensionName = governmentPension.SpecializedPensionName
            };

            LogQuerySuccessful(logger, request.UserId);

            return result;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return null;
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetGovernmentPension query for UserId: {UserId}")]
    static partial void LogStartingQuery(ILogger<GetGovernmentPensionHandler> logger, long userId);

    [LoggerMessage(LogLevel.Information, "No government pension found for UserId: {UserId}")]
    static partial void LogNoneFound(ILogger<GetGovernmentPensionHandler> logger, long userId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved government pension for UserId: {UserId}")]
    static partial void LogQuerySuccessful(ILogger<GetGovernmentPensionHandler> logger, long userId);

    [LoggerMessage(LogLevel.Error, "Error occurred while retrieving government pension for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetGovernmentPensionHandler> logger, string exception, long userId);
}


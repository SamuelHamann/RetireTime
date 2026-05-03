using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.GetSharePurchasePlan;

public partial class GetSharePurchasePlanHandler(
    ISharePurchasePlanRepository repository,
    ILogger<GetSharePurchasePlanHandler> logger) : IRequestHandler<GetSharePurchasePlanQuery, List<SharePurchasePlan>>
{
    public async Task<List<SharePurchasePlan>> Handle(GetSharePurchasePlanQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var items = await repository.GetByScenarioIdAsync(request.ScenarioId, request.TimelineId);
            LogSuccessfullyCompleted(logger, items.Count, request.ScenarioId);
            return items;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return [];
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetSharePurchasePlan handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetSharePurchasePlanHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} share purchase plans for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetSharePurchasePlanHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting share purchase plans for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetSharePurchasePlanHandler> logger, string Exception, long ScenarioId);
}

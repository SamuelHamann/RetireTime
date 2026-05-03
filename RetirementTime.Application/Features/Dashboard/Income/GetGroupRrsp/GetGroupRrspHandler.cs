using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.GetGroupRrsp;

public partial class GetGroupRrspHandler(
    IGroupRrspRepository repository,
    ILogger<GetGroupRrspHandler> logger) : IRequestHandler<GetGroupRrspQuery, List<GroupRrsp>>
{
    public async Task<List<GroupRrsp>> Handle(GetGroupRrspQuery request, CancellationToken cancellationToken)
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

    [LoggerMessage(LogLevel.Information, "Starting GetGroupRrsp handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetGroupRrspHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} Group RRSP plans for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetGroupRrspHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting Group RRSP plans for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetGroupRrspHandler> logger, string Exception, long ScenarioId);
}

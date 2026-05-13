using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.GetPensionDefinedContribution;

public partial class GetPensionDefinedContributionHandler(
    IPensionDefinedContributionRepository repository,
    ILogger<GetPensionDefinedContributionHandler> logger) : IRequestHandler<GetPensionDefinedContributionQuery, List<PensionDefinedContribution>>
{
    public async Task<List<PensionDefinedContribution>> Handle(GetPensionDefinedContributionQuery request, CancellationToken cancellationToken)
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

    [LoggerMessage(LogLevel.Information, "Starting GetPensionDefinedContribution handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetPensionDefinedContributionHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} defined contribution plans for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetPensionDefinedContributionHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting defined contribution plans for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetPensionDefinedContributionHandler> logger, string Exception, long ScenarioId);
}

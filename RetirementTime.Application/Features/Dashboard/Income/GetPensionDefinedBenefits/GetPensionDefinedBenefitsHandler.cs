using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.GetPensionDefinedBenefits;

public partial class GetPensionDefinedBenefitsHandler(
    IPensionDefinedBenefitsRepository repository,
    ILogger<GetPensionDefinedBenefitsHandler> logger) : IRequestHandler<GetPensionDefinedBenefitsQuery, List<PensionDefinedBenefits>>
{
    public async Task<List<PensionDefinedBenefits>> Handle(GetPensionDefinedBenefitsQuery request, CancellationToken cancellationToken)
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

    [LoggerMessage(LogLevel.Information, "Starting GetPensionDefinedBenefits handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetPensionDefinedBenefitsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} defined benefits pensions for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetPensionDefinedBenefitsHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting defined benefits pensions for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetPensionDefinedBenefitsHandler> logger, string Exception, long ScenarioId);
}

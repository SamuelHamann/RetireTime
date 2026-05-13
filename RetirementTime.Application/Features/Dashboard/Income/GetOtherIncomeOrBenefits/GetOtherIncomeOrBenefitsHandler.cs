using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.GetOtherIncomeOrBenefits;

public partial class GetOtherIncomeOrBenefitsHandler(
    IOtherIncomeOrBenefitsRepository repository,
    ILogger<GetOtherIncomeOrBenefitsHandler> logger) : IRequestHandler<GetOtherIncomeOrBenefitsQuery, List<OtherIncomeOrBenefits>>
{
    public async Task<List<OtherIncomeOrBenefits>> Handle(GetOtherIncomeOrBenefitsQuery request, CancellationToken cancellationToken)
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

    [LoggerMessage(LogLevel.Information, "Starting GetOtherIncomeOrBenefits handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetOtherIncomeOrBenefitsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} other income/benefits for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetOtherIncomeOrBenefitsHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting other income/benefits for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetOtherIncomeOrBenefitsHandler> logger, string Exception, long ScenarioId);
}

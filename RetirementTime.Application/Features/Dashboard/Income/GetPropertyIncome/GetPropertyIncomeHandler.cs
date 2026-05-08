using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.GetPropertyIncome;

public partial class GetPropertyIncomeHandler(
    IPropertyIncomeRepository repository,
    ILogger<GetPropertyIncomeHandler> logger) : IRequestHandler<GetPropertyIncomeQuery, List<PropertyIncome>>
{
    public async Task<List<PropertyIncome>> Handle(GetPropertyIncomeQuery request, CancellationToken cancellationToken)
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

    [LoggerMessage(LogLevel.Information, "Starting GetPropertyIncome handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetPropertyIncomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} property incomes for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetPropertyIncomeHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting property income for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetPropertyIncomeHandler> logger, string Exception, long ScenarioId);
}


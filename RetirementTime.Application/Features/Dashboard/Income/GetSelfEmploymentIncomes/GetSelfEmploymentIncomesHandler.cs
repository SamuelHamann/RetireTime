using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.GetSelfEmploymentIncomes;

public partial class GetSelfEmploymentIncomesHandler(
    ISelfEmploymentIncomeRepository selfEmploymentIncomeRepository,
    ILogger<GetSelfEmploymentIncomesHandler> logger) : IRequestHandler<GetSelfEmploymentIncomesQuery, List<SelfEmploymentIncome>>
{
    public async Task<List<SelfEmploymentIncome>> Handle(GetSelfEmploymentIncomesQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var items = await selfEmploymentIncomeRepository.GetByScenarioIdAsync(request.ScenarioId);

            LogSuccessfullyCompleted(logger, items.Count, request.ScenarioId);

            return items;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return [];
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetSelfEmploymentIncomes handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetSelfEmploymentIncomesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} self-employment incomes for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetSelfEmploymentIncomesHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting self-employment incomes for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetSelfEmploymentIncomesHandler> logger, string Exception, long ScenarioId);
}

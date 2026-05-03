using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.GetEmploymentIncomes;

public partial class GetEmploymentIncomesHandler(
    IEmploymentIncomeRepository employmentIncomeRepository,
    ILogger<GetEmploymentIncomesHandler> logger) : IRequestHandler<GetEmploymentIncomesQuery, List<EmploymentIncome>>
{
    public async Task<List<EmploymentIncome>> Handle(GetEmploymentIncomesQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var items = await employmentIncomeRepository.GetByScenarioIdAsync(request.ScenarioId, request.TimelineId);

            LogSuccessfullyCompleted(logger, items.Count, request.ScenarioId);

            return items;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return [];
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetEmploymentIncomes handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetEmploymentIncomesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} employment incomes for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetEmploymentIncomesHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting employment incomes for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetEmploymentIncomesHandler> logger, string Exception, long ScenarioId);
}

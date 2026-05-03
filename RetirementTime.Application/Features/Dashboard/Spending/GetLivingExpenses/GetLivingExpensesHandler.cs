using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Spending.GetLivingExpenses;

public partial class GetLivingExpensesHandler(
    ISpendingRepository repository,
    ILogger<GetLivingExpensesHandler> logger) : IRequestHandler<GetLivingExpensesQuery, GetLivingExpensesResult>
{
    public async Task<GetLivingExpensesResult> Handle(GetLivingExpensesQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var expenses    = await repository.GetLivingExpensesAsync(request.ScenarioId, request.TimelineId);
            var frequencies = await repository.GetFrequenciesAsync();

            LogSuccessfullyCompleted(logger, request.ScenarioId);
            return new GetLivingExpensesResult { Expenses = expenses, Frequencies = frequencies };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new GetLivingExpensesResult();
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetLivingExpenses handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetLivingExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved living expenses for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetLivingExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error getting living expenses for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetLivingExpensesHandler> logger, string Exception, long ScenarioId);
}

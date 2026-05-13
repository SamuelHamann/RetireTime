using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Spending.GetDiscretionaryExpenses;

public partial class GetDiscretionaryExpensesHandler(
    ISpendingRepository repository,
    ILogger<GetDiscretionaryExpensesHandler> logger) : IRequestHandler<GetDiscretionaryExpensesQuery, GetDiscretionaryExpensesResult>
{
    public async Task<GetDiscretionaryExpensesResult> Handle(GetDiscretionaryExpensesQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var expenses    = await repository.GetDiscretionaryExpensesAsync(request.ScenarioId, request.TimelineId);
            var frequencies = await repository.GetFrequenciesAsync();

            LogSuccessfullyCompleted(logger, request.ScenarioId);
            return new GetDiscretionaryExpensesResult { Expenses = expenses, Frequencies = frequencies };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new GetDiscretionaryExpensesResult();
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetDiscretionaryExpenses handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetDiscretionaryExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved discretionary expenses for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetDiscretionaryExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error getting discretionary expenses for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetDiscretionaryExpensesHandler> logger, string Exception, long ScenarioId);
}

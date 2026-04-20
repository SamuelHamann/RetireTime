using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Debt.GetDebts;

public partial class GetDebtsHandler(
    IGenericDebtRepository repository,
    ILogger<GetDebtsHandler> logger) : IRequestHandler<GetDebtsQuery, GetDebtsResult>
{
    public async Task<GetDebtsResult> Handle(GetDebtsQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var debts = await repository.GetByDebtTypeIdsAsync(request.ScenarioId, request.DebtTypeIds);
            var frequencies = await repository.GetFrequenciesAsync();
            var debtTypes = request.DebtTypeIds.Length > 1
                ? await repository.GetDebtTypesByIdsAsync(request.DebtTypeIds)
                : [];

            LogSuccessfullyCompleted(logger, debts.Count, request.ScenarioId);
            return new GetDebtsResult { Debts = debts, Frequencies = frequencies, DebtTypes = debtTypes };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new GetDebtsResult();
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetDebts handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetDebtsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} debts for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetDebtsHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting debts for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetDebtsHandler> logger, string Exception, long ScenarioId);
}

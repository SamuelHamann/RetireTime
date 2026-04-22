using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.GetOtherPersistingIncomes;

public partial class GetOtherPersistingIncomesHandler(
    IOtherPersistingIncomeRepository repository,
    IFrequencyRepository frequencyRepository,
    ILogger<GetOtherPersistingIncomesHandler> logger) : IRequestHandler<GetOtherPersistingIncomesQuery, GetOtherPersistingIncomesResult>
{
    public async Task<GetOtherPersistingIncomesResult> Handle(GetOtherPersistingIncomesQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);
        try
        {
            var items = await repository.GetByScenarioIdAsync(request.ScenarioId);
            var frequencies = await frequencyRepository.GetFrequencies();
            LogSuccessfullyCompleted(logger, items.Count, request.ScenarioId);
            return new GetOtherPersistingIncomesResult
            {
                Items = items.Select(e => new OtherPersistingIncomeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Amount = e.Amount,
                    FrequencyId = e.FrequencyId,
                    Taxable = e.Taxable
                }).ToList(),
                Frequencies = frequencies
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new GetOtherPersistingIncomesResult();
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetOtherPersistingIncomes handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetOtherPersistingIncomesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} other persisting incomes for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetOtherPersistingIncomesHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error retrieving other persisting incomes for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetOtherPersistingIncomesHandler> logger, string Exception, long ScenarioId);
}

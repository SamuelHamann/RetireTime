using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.GetRealEstateIncomes;

public partial class GetRealEstateIncomesHandler(
    IRealEstateIncomeRepository repository,
    ILogger<GetRealEstateIncomesHandler> logger) : IRequestHandler<GetRealEstateIncomesQuery, List<RealEstateIncomeDto>>
{
    public async Task<List<RealEstateIncomeDto>> Handle(GetRealEstateIncomesQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);
        try
        {
            var items = await repository.GetByScenarioIdAsync(request.ScenarioId);
            LogSuccessfullyCompleted(logger, items.Count, request.ScenarioId);
            return items.Select(e => new RealEstateIncomeDto
            {
                Id = e.Id,
                InvestmentPropertyId = e.InvestmentPropertyId,
                PropertyName = e.PropertyName,
                Amount = e.Amount,
                FrequencyId = e.FrequencyId
            }).ToList();
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return [];
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetRealEstateIncomes handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetRealEstateIncomesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} real estate incomes for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetRealEstateIncomesHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error retrieving real estate incomes for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetRealEstateIncomesHandler> logger, string Exception, long ScenarioId);
}


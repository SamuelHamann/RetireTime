using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Debt.GetHomeMortgage;

public partial class GetHomeMortgageHandler(
    IGenericDebtRepository repository,
    ILogger<GetHomeMortgageHandler> logger) : IRequestHandler<GetHomeMortgageQuery, GetHomeMortgageResult>
{
    public async Task<GetHomeMortgageResult> Handle(GetHomeMortgageQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var debt = await repository.GetHomeMortgageAsync(request.ScenarioId);
            var frequencies = await repository.GetFrequenciesAsync();

            LogSuccessfullyCompleted(logger, request.ScenarioId);
            return new GetHomeMortgageResult { Debt = debt, Frequencies = frequencies };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new GetHomeMortgageResult();
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetHomeMortgage handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetHomeMortgageHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved home mortgage for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetHomeMortgageHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting home mortgage for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetHomeMortgageHandler> logger, string Exception, long ScenarioId);
}

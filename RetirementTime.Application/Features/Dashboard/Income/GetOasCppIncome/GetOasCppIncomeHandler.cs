using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.GetOasCppIncome;

public partial class GetOasCppIncomeHandler(
    IOasCppIncomeRepository repository,
    ILogger<GetOasCppIncomeHandler> logger) : IRequestHandler<GetOasCppIncomeQuery, OasCppIncome?>
{
    public async Task<OasCppIncome?> Handle(GetOasCppIncomeQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var item = await repository.GetByScenarioIdAsync(request.ScenarioId);
            LogSuccessfullyCompleted(logger, request.ScenarioId);
            return item;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return null;
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetOasCppIncome handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetOasCppIncomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved OAS/CPP income for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetOasCppIncomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting OAS/CPP income for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetOasCppIncomeHandler> logger, string Exception, long ScenarioId);
}

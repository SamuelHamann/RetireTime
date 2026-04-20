using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.GetInvestmentAccounts;

public partial class GetInvestmentAccountsHandler(
    IAssetsInvestmentAccountRepository repository,
    ILogger<GetInvestmentAccountsHandler> logger) : IRequestHandler<GetInvestmentAccountsQuery, GetInvestmentAccountsResult>
{
    public async Task<GetInvestmentAccountsResult> Handle(GetInvestmentAccountsQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var accounts = await repository.GetByScenarioIdAsync(request.ScenarioId);
            var accountTypes = await repository.GetAccountTypesAsync();

            LogSuccessfullyCompleted(logger, accounts.Count, request.ScenarioId);
            return new GetInvestmentAccountsResult { Accounts = accounts, AccountTypes = accountTypes };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new GetInvestmentAccountsResult();
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetInvestmentAccounts handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetInvestmentAccountsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} investment accounts for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetInvestmentAccountsHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting investment accounts for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetInvestmentAccountsHandler> logger, string Exception, long ScenarioId);
}

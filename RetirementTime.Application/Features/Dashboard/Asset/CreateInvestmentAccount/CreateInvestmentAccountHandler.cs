using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.CreateInvestmentAccount;

public partial class CreateInvestmentAccountHandler(
    IAssetsInvestmentAccountRepository repository,
    ILogger<CreateInvestmentAccountHandler> logger) : IRequestHandler<CreateInvestmentAccountCommand, CreateInvestmentAccountResult>
{
    public async Task<CreateInvestmentAccountResult> Handle(CreateInvestmentAccountCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var account = new AssetsInvestmentAccount
            {
                ScenarioId = request.ScenarioId,
                AccountName = string.Empty,
                AccountTypeId = (long)InvestmentAccountType.RRSP
            };
            var created = await repository.CreateAsync(account);

            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreateInvestmentAccountResult { Success = true, AccountId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreateInvestmentAccountResult { Success = false, ErrorMessage = "An error occurred while adding the account. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateInvestmentAccount handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreateInvestmentAccountHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created investment account with ID: {AccountId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateInvestmentAccountHandler> logger, long AccountId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating investment account for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateInvestmentAccountHandler> logger, string Exception, long ScenarioId);
}

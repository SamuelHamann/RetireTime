using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.UpdateInvestmentAccount;

public partial class UpdateInvestmentAccountHandler(
    IAssetsInvestmentAccountRepository repository,
    ILogger<UpdateInvestmentAccountHandler> logger) : IRequestHandler<UpdateInvestmentAccountCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateInvestmentAccountCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var account = new AssetsInvestmentAccount
            {
                Id = request.Id,
                AccountName = request.AccountName,
                AccountTypeId = request.AccountTypeId,
                AdjustedCostBasis = request.AdjustedCostBasis,
                CurrentTotalValue = request.CurrentTotalValue,
                UseIndividualHoldings = request.UseIndividualHoldings
            };
            var success = await repository.UpdateAsync(account);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Account record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while saving the account. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdateInvestmentAccount handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdateInvestmentAccountHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Investment account not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<UpdateInvestmentAccountHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated investment account with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateInvestmentAccountHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating investment account with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateInvestmentAccountHandler> logger, string Exception, long Id);
}

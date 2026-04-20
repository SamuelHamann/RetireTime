using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.CreateHolding;

public partial class CreateHoldingHandler(
    IAssetsInvestmentAccountRepository repository,
    ILogger<CreateHoldingHandler> logger) : IRequestHandler<CreateHoldingCommand, CreateHoldingResult>
{
    public async Task<CreateHoldingResult> Handle(CreateHoldingCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.AccountId);

        try
        {
            var holding = new AssetsHolding
            {
                InvestmentAccountId = request.AccountId,
                AssetName = string.Empty,
                TickerSymbol = string.Empty
            };
            var created = await repository.CreateHoldingAsync(holding);

            LogSuccessfullyCompleted(logger, created.Id, request.AccountId);
            return new CreateHoldingResult { Success = true, HoldingId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.AccountId);
            return new CreateHoldingResult { Success = false, ErrorMessage = "An error occurred while adding the holding. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateHolding handler for AccountId: {AccountId}")]
    static partial void LogStartingHandler(ILogger<CreateHoldingHandler> logger, long AccountId);

    [LoggerMessage(LogLevel.Information, "Successfully created holding with ID: {HoldingId} for AccountId: {AccountId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateHoldingHandler> logger, long HoldingId, long AccountId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating holding for AccountId: {AccountId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateHoldingHandler> logger, string Exception, long AccountId);
}

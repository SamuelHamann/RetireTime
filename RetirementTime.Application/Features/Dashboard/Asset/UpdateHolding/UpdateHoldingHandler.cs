using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.UpdateHolding;

public partial class UpdateHoldingHandler(
    IAssetsInvestmentAccountRepository repository,
    ILogger<UpdateHoldingHandler> logger) : IRequestHandler<UpdateHoldingCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateHoldingCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var holding = new AssetsHolding
            {
                Id = request.Id,
                AssetName = request.AssetName,
                IsPubliclyTraded = request.IsPubliclyTraded,
                CurrentValue = request.CurrentValue,
                TickerSymbol = request.TickerSymbol,
                AdjustedCostBasis = request.AdjustedCostBasis
            };
            var success = await repository.UpdateHoldingAsync(holding);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Holding record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while saving the holding. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdateHolding handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdateHoldingHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Holding not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<UpdateHoldingHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated holding with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdateHoldingHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating holding with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdateHoldingHandler> logger, string Exception, long Id);
}

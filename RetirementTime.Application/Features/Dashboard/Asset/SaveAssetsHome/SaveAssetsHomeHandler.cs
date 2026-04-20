using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.SaveAssetsHome;

public partial class SaveAssetsHomeHandler(
    IAssetsHomeRepository repository,
    ILogger<SaveAssetsHomeHandler> logger) : IRequestHandler<SaveAssetsHomeCommand, BaseResult>
{
    public async Task<BaseResult> Handle(SaveAssetsHomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var home = new AssetsHome
            {
                ScenarioId = request.ScenarioId,
                PurchaseDate = request.PurchaseDate,
                HomeValue = request.HomeValue,
                PurchasePrice = request.PurchasePrice
            };

            await repository.UpsertAsync(home);

            LogSuccessfullyCompleted(logger, request.ScenarioId);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while saving home data. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting SaveAssetsHome handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<SaveAssetsHomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully saved assets home for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<SaveAssetsHomeHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while saving assets home for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<SaveAssetsHomeHandler> logger, string Exception, long ScenarioId);
}

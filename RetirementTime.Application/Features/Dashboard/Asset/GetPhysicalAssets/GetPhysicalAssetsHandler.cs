using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.GetPhysicalAssets;

public partial class GetPhysicalAssetsHandler(
    IAssetsPhysicalAssetRepository repository,
    ILogger<GetPhysicalAssetsHandler> logger) : IRequestHandler<GetPhysicalAssetsQuery, GetPhysicalAssetsResult>
{
    public async Task<GetPhysicalAssetsResult> Handle(GetPhysicalAssetsQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var assets = await repository.GetByScenarioIdAsync(request.ScenarioId);
            var assetTypes = await repository.GetAssetTypesAsync();

            LogSuccessfullyCompleted(logger, assets.Count, request.ScenarioId);
            return new GetPhysicalAssetsResult { Assets = assets, AssetTypes = assetTypes };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new GetPhysicalAssetsResult();
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetPhysicalAssets handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetPhysicalAssetsHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} physical assets for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetPhysicalAssetsHandler> logger, int Count, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting physical assets for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetPhysicalAssetsHandler> logger, string Exception, long ScenarioId);
}

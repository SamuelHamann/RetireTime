using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.CreatePhysicalAsset;

public partial class CreatePhysicalAssetHandler(
    IAssetsPhysicalAssetRepository repository,
    ILogger<CreatePhysicalAssetHandler> logger) : IRequestHandler<CreatePhysicalAssetCommand, CreatePhysicalAssetResult>
{
    public async Task<CreatePhysicalAssetResult> Handle(CreatePhysicalAssetCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var asset = new AssetsPhysicalAsset
            {
                ScenarioId = request.ScenarioId,
                AssetTypeId = (long)AssetTypeEnum.Other,
                Name = string.Empty
            };
            var created = await repository.CreateAsync(asset);

            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreatePhysicalAssetResult { Success = true, AssetId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreatePhysicalAssetResult { Success = false, ErrorMessage = "An error occurred while adding the asset. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreatePhysicalAsset handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreatePhysicalAssetHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully created physical asset with ID: {AssetId} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreatePhysicalAssetHandler> logger, long AssetId, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while creating physical asset for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreatePhysicalAssetHandler> logger, string Exception, long ScenarioId);
}

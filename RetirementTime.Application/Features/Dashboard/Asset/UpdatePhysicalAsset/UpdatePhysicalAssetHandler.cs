using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.UpdatePhysicalAsset;

public partial class UpdatePhysicalAssetHandler(
    IAssetsPhysicalAssetRepository repository,
    ILogger<UpdatePhysicalAssetHandler> logger) : IRequestHandler<UpdatePhysicalAssetCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdatePhysicalAssetCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var asset = new AssetsPhysicalAsset
            {
                Id = request.Id,
                AssetTypeId = request.AssetTypeId,
                Name = request.Name,
                EstimatedValue = request.EstimatedValue,
                AdjustedCostBasis = request.AdjustedCostBasis,
                IsConsideredPersonalProperty = request.IsConsideredPersonalProperty,
                IsConsideredAsARetirementAsset = request.IsConsideredAsARetirementAsset
            };
            var success = await repository.UpdateAsync(asset);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Asset record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while saving the asset. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpdatePhysicalAsset handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<UpdatePhysicalAssetHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Physical asset not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<UpdatePhysicalAssetHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully updated physical asset with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpdatePhysicalAssetHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while updating physical asset with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpdatePhysicalAssetHandler> logger, string Exception, long Id);
}

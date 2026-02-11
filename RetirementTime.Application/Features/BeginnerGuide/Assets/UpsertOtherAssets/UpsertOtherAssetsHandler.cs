using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.UpsertOtherAssets;

public partial class UpsertOtherAssetsHandler(
    IOtherAssetRepository otherAssetRepository,
    ILogger<UpsertOtherAssetsHandler> logger) : IRequestHandler<UpsertOtherAssetsCommand, UpsertOtherAssetsResult>
{
    public async Task<UpsertOtherAssetsResult> Handle(UpsertOtherAssetsCommand request, CancellationToken cancellationToken)
    {
        LogStartingUpsert(logger, request.UserId, request.HasOtherAssets, request.Assets.Count);

        try
        {
            // If user doesn't have other assets, delete all existing assets
            if (!request.HasOtherAssets)
            {
                await otherAssetRepository.DeleteByUserIdAsync(request.UserId);
                LogDeletedAllAssets(logger, request.UserId);
                
                return new UpsertOtherAssetsResult
                {
                    Success = true,
                    AssetIds = []
                };
            }

            // Validate all assets
            foreach (var assetDto in request.Assets)
            {
                if (string.IsNullOrWhiteSpace(assetDto.Name))
                {
                    LogValidationFailed(logger, "Asset name is required");
                    return new UpsertOtherAssetsResult
                    {
                        Success = false,
                        ErrorMessage = "Please enter a name for all assets."
                    };
                }

                if (assetDto.AssetTypeId <= 0)
                {
                    LogValidationFailed(logger, "Invalid asset type");
                    return new UpsertOtherAssetsResult
                    {
                        Success = false,
                        ErrorMessage = "Please select a valid type for all assets."
                    };
                }

                if (assetDto.CurrentValue <= 0)
                {
                    LogValidationFailed(logger, "Invalid current value");
                    return new UpsertOtherAssetsResult
                    {
                        Success = false,
                        ErrorMessage = "Please enter a valid current value for all assets."
                    };
                }

                if (assetDto.PurchasePrice.HasValue && assetDto.PurchasePrice.Value < 0)
                {
                    LogValidationFailed(logger, "Invalid purchase price");
                    return new UpsertOtherAssetsResult
                    {
                        Success = false,
                        ErrorMessage = "Purchase price cannot be negative."
                    };
                }
            }
            
            // Prepare all assets for bulk insert
            var assets = request.Assets.Select(assetDto => new OtherAsset
            {
                UserId = request.UserId,
                Name = assetDto.Name,
                AssetTypeId = assetDto.AssetTypeId,
                CurrentValue = assetDto.CurrentValue,
                PurchasePrice = assetDto.PurchasePrice,
                User = null!,
                AssetType = null!
            }).ToList();

            // Execute delete and inserts in a single transaction
            var (_, assetIds) = await otherAssetRepository.UpsertAssetsAsync(request.UserId, assets);

            LogUpsertSuccessful(logger, request.UserId, assetIds.Count);

            return new UpsertOtherAssetsResult
            {
                Success = true,
                AssetIds = assetIds
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new UpsertOtherAssetsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving your assets. Please try again."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting upsert for UserId: {UserId}, HasOtherAssets: {HasOtherAssets}, AssetCount: {AssetCount}")]
    static partial void LogStartingUpsert(ILogger logger, long userId, bool hasOtherAssets, int assetCount);

    [LoggerMessage(LogLevel.Information, "Deleted all assets for UserId: {UserId}")]
    static partial void LogDeletedAllAssets(ILogger logger, long userId);

    [LoggerMessage(LogLevel.Warning, "Validation failed: {Reason}")]
    static partial void LogValidationFailed(ILogger logger, string reason);

    [LoggerMessage(LogLevel.Information, "Successfully upserted other assets for UserId: {UserId}, Count: {Count}")]
    static partial void LogUpsertSuccessful(ILogger logger, long userId, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger logger, string exception, long userId);
}


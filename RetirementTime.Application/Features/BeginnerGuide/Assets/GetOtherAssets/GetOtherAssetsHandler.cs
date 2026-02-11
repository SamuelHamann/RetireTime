using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.GetOtherAssets;

public partial class GetOtherAssetsHandler(
    IOtherAssetRepository otherAssetRepository,
    ILogger<GetOtherAssetsHandler> logger) : IRequestHandler<GetOtherAssetsQuery, List<OtherAssetDto>>
{
    public async Task<List<OtherAssetDto>> Handle(GetOtherAssetsQuery request, CancellationToken cancellationToken)
    {
        LogStartingQuery(logger, request.UserId);

        try
        {
            var assets = await otherAssetRepository.GetByUserIdAsync(request.UserId);
            
            var result = assets.Select(a => new OtherAssetDto
            {
                Id = a.Id,
                Name = a.Name,
                AssetTypeId = a.AssetTypeId,
                AssetTypeName = a.AssetType.Name,
                CurrentValue = a.CurrentValue,
                PurchasePrice = a.PurchasePrice
            }).ToList();

            LogQuerySuccessful(logger, request.UserId, result.Count);
            
            return result;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return [];
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetOtherAssets query for UserId: {UserId}")]
    static partial void LogStartingQuery(ILogger<GetOtherAssetsHandler> logger, long userId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} assets for UserId: {UserId}")]
    static partial void LogQuerySuccessful(ILogger<GetOtherAssetsHandler> logger, long userId, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred while retrieving assets for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetOtherAssetsHandler> logger, string exception, long userId);
}


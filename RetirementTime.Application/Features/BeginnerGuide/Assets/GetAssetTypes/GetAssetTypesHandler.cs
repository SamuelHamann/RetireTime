using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.GetAssetTypes;

public partial class GetAssetTypesHandler(
    IAssetTypeRepository assetTypeRepository,
    ILogger<GetAssetTypesHandler> logger) : IRequestHandler<GetAssetTypesQuery, List<AssetTypeDto>>
{
    public async Task<List<AssetTypeDto>> Handle(GetAssetTypesQuery request, CancellationToken cancellationToken)
    {
        LogStartingQuery(logger);

        try
        {
            var assetTypes = await assetTypeRepository.GetAllAsync();
            
            var result = assetTypes.Select(at => new AssetTypeDto
            {
                Id = at.Id,
                Name = at.Name,
                Description = at.Description
            }).ToList();

            LogQuerySuccessful(logger, result.Count);
            
            return result;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message);
            return [];
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetAssetTypes query")]
    static partial void LogStartingQuery(ILogger<GetAssetTypesHandler> logger);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} asset types")]
    static partial void LogQuerySuccessful(ILogger<GetAssetTypesHandler> logger, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred while retrieving asset types | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetAssetTypesHandler> logger, string exception);
}


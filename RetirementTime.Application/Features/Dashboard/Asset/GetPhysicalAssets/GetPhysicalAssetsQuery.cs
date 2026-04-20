using MediatR;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;

namespace RetirementTime.Application.Features.Dashboard.Asset.GetPhysicalAssets;

public record GetPhysicalAssetsQuery(long ScenarioId) : IRequest<GetPhysicalAssetsResult>;

public record GetPhysicalAssetsResult
{
    public List<AssetsPhysicalAsset> Assets { get; init; } = [];
    public List<PhysicalAssetType> AssetTypes { get; init; } = [];
}

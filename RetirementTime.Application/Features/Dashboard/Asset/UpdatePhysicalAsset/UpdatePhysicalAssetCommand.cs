using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Asset.UpdatePhysicalAsset;

public record UpdatePhysicalAssetCommand : IRequest<BaseResult>
{
    public required long Id { get; init; }
    public long AssetTypeId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal? EstimatedValue { get; init; }
    public decimal? AdjustedCostBasis { get; init; }
    public bool IsConsideredPersonalProperty { get; init; }
    public bool IsConsideredAsARetirementAsset { get; init; }
}

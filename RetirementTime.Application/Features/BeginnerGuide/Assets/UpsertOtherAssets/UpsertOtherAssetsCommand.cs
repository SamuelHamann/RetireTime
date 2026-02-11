using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.UpsertOtherAssets;

public class UpsertOtherAssetsCommand : IRequest<UpsertOtherAssetsResult>
{
    public long UserId { get; set; }
    public bool HasOtherAssets { get; set; }
    public List<OtherAssetInputDto> Assets { get; set; } = new();
}

public class OtherAssetInputDto
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AssetTypeId { get; set; }
    public decimal CurrentValue { get; set; }
    public decimal? PurchasePrice { get; set; }
}

public record UpsertOtherAssetsResult : BaseResult
{
    public List<int> AssetIds { get; init; } = new();
}


using MediatR;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.GetOtherAssets;

public class GetOtherAssetsQuery : IRequest<List<OtherAssetDto>>
{
    public long UserId { get; set; }
}

public class OtherAssetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AssetTypeId { get; set; }
    public string AssetTypeName { get; set; } = string.Empty;
    public decimal CurrentValue { get; set; }
    public decimal? PurchasePrice { get; set; }
}


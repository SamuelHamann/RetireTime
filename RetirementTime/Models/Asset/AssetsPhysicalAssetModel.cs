namespace RetirementTime.Models.Asset;

public class AssetsPhysicalAssetItemModel
{
    public long Id { get; set; }
    public long AssetTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? EstimatedValue { get; set; }
    public decimal? AdjustedCostBasis { get; set; }
    public bool IsConsideredPersonalProperty { get; set; }
    public bool IsConsideredAsARetirementAsset { get; set; }
}

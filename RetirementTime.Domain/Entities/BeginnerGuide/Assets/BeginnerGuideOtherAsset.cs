namespace RetirementTime.Domain.Entities.BeginnerGuide.Assets;

public class BeginnerGuideOtherAsset
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AssetTypeId { get; set; }
    public decimal CurrentValue { get; set; }
    public decimal? PurchasePrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public User? User { get; set; }
    public required BeginnerGuideAssetType AssetType { get; set; }
}


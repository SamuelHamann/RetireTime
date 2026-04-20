using System.ComponentModel;

namespace RetirementTime.Domain.Entities.Common;

public class PhysicalAssetType
{
    public long Id { get; set; }
    public required string Name { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum AssetTypeEnum
{
    [Description("Vehicule")]
    Vehicle = 1,
    
    [Description("Collectible")]
    Collectible = 2,
    
    [Description("Jewelry")]
    Jewelry = 3,
    
    [Description("Precious Metals")]
    PreciousMetals = 4,
    
    [Description("Other")]
    Other = 5
}
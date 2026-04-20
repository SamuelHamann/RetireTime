using System.ComponentModel;
using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Entities.Dashboard.Asset;

public class AssetsPhysicalAsset
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public long AssetTypeId { get; set; }
    public required string Name { get; set; }
    public decimal? EstimatedValue { get; set; }
    public decimal? AdjustedCostBasis { get; set; }
    public bool IsConsideredPersonalProperty { get; set; }
    public bool IsConsideredAsARetirementAsset { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public PhysicalAssetType PhysicalAssetType { get; set; } = null!;
}


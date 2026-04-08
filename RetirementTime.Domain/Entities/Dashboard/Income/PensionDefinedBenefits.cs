using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Entities.Dashboard.Income;

public class PensionDefinedBenefits
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StartAge { get; set; }
    public decimal? BenefitsPre65 { get; set; }
    public int BenefitsPre65FrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public decimal? BenefitsPost65 { get; set; }
    public int BenefitsPost65FrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public int PercentIndexedToInflation { get; set; }
    public int PercentSurvivorBenefits { get; set; }
    public decimal? RrspAdjustment { get; set; }
    public int RrspAdjustmentFrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public Frequency BenefitsPre65Frequency { get; set; } = null!;
    public Frequency BenefitsPost65Frequency { get; set; } = null!;
    public Frequency RrspAdjustmentFrequency { get; set; } = null!;
}
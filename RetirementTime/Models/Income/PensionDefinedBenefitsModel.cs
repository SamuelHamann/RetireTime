namespace RetirementTime.Models.Income;

public class PensionDefinedBenefitsItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StartAge { get; set; }
    public decimal? BenefitsPre65 { get; set; }
    public decimal? BenefitsPost65 { get; set; }
    public int PercentIndexedToInflation { get; set; }
    public int PercentSurvivorBenefits { get; set; }
    public decimal? RrspAdjustment { get; set; }
}

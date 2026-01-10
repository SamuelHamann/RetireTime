namespace RetirementTime.Domain.Entities.RealEstate;

public class BuyOrRent
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required double PercentDifferenceReinvested { get; set; }
    public required double PercentDifferenceReinvestedGrowthRate { get; set; }
    public required int ComparisonTimeframeInYears { get; set; }
    
    public required Rent Rent { get; set; }
    public required RealEstate RealEstate { get; set; }
}
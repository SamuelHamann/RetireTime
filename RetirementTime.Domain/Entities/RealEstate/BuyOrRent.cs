namespace RetirementTime.Domain.Entities.RealEstate;

public class BuyOrRent
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required double PercentDifferenceReinvested { get; set; }
    public required double PercentDifferenceReinvestedGrowthRate { get; set; }
    public required int ComparisonTimeframeInYears { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    
    public int RentId { get; set; }
    public int MortgageId { get; set; }
    public required Rent Rent { get; set; }
    public required Mortgage Mortgage { get; set; }
    
}
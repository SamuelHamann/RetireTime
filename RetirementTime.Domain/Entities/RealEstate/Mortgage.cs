namespace RetirementTime.Domain.Entities.RealEstate;

public class Mortgage
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required double InterestRate { get; set; }
    public required int TermInYears { get; set; }
    public required double DownPayment { get; set; }
    public double MonthlyMortgageInsuranceCosts { get; set; }
    public double ClosingCosts { get; set; }
    public double AdditionalCosts { get; set; }
    public double AdditionalMonthlyCosts { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    
    public int RealEstateId { get; set; }
    public required RealEstate RealEstate { get; set; }
}
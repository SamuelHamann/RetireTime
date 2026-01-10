namespace RetirementTime.Domain.Entities.RealEstate;

public class RealEstate
{
    public int Id { get; set; }
    public required string Type { get; set; }
    public required string Name { get; set; }
    public required double Price { get; set; }
    public required double MonthlyElectricityCosts { get; set; }
    public required double MonthlyInsuranceCosts { get; set; }
    public required double PercentYearlyExpenses { get; set; }
    public required double YearlyTaxesPercent { get; set; }
    public required double YearlyAppreciation { get; set; }
    public double YearlyHoaCosts { get; set; }

    
}
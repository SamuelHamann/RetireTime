using RetirementTime.Domain.Entities.Location;

namespace RetirementTime.Domain.Entities.RealEstate;

public class Rent
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required double MonthlyRent { get; set; }
    public required double YearlyRentIncrease { get; set; }
    public double MonthlyElectricityCosts { get; set; }
    public double MonthlyInsuranceCosts { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required  DateTime UpdatedAt { get; set; }
}
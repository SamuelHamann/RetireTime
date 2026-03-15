namespace RetirementTime.Domain.Entities.BeginnerGuide.Income;

public class BeginnerGuideSelfEmployment
{
    public long Id { get; set; }
    public required long UserId { get; set; }
    public required string BusinessName { get; set; }
    public required decimal AnnualSalary { get; set; }
    public required decimal AverageAnnualRevenueIncrease { get; set; }
    public required decimal MonthlyDividends { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<BeginnerGuideSelfEmploymentAdditionalCompensation> AdditionalCompensations { get; set; } = new List<BeginnerGuideSelfEmploymentAdditionalCompensation>();
}

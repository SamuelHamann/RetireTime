namespace RetirementTime.Domain.Entities.BeginnerGuide.Income;

public class BeginnerGuideEmployment
{
    public long Id { get; set; }
    public required long UserId { get; set; }
    public required string EmployerName { get; set; }
    public required decimal AnnualSalary { get; set; }
    public required decimal AverageAnnualWageIncrease { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<BeginnerGuideAdditionalCompensation> AdditionalCompensations { get; set; } = new List<BeginnerGuideAdditionalCompensation>();
}

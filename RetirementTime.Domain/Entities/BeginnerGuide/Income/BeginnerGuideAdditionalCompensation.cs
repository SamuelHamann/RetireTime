using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Entities.BeginnerGuide.Income;

public class BeginnerGuideAdditionalCompensation
{
    public long Id { get; set; }
    public required long EmploymentId { get; set; }
    public required string Name { get; set; }
    public required decimal Amount { get; set; }
    public required int FrequencyId { get; set; } = 7; // Default to Annually
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public BeginnerGuideEmployment? Employment { get; set; }
    public Frequency? Frequency { get; set; }
}

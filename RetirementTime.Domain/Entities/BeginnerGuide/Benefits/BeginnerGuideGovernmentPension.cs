namespace RetirementTime.Domain.Entities.BeginnerGuide.Benefits;

public class BeginnerGuideGovernmentPension
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public int YearsWorked { get; set; }
    public bool HasSpecializedPublicSectorPension { get; set; }
    public string? SpecializedPensionName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User? User { get; set; }
}


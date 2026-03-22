namespace RetirementTime.Domain.Entities.Onboarding;

public class OnboardingPersonalInfo
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public required string CitizenshipStatus { get; set; }
    public required string MaritalStatus { get; set; }
    public bool HasCurrentChildren { get; set; }
    public bool PlansFutureChildren { get; set; }
    public bool IncludePartner { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public required User User { get; set; }
}

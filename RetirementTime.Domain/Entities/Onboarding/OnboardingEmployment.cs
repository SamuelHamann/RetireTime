namespace RetirementTime.Domain.Entities.Onboarding;

public class OnboardingEmployment
{
    public long Id { get; set; }
    public long UserId { get; set; }
    
    public bool IsEmployed { get; set; }
    public bool IsSelfEmployed { get; set; }
    public int? PlannedRetirementAge { get; set; }
    public int? CppContributionYears { get; set; }
    
    public bool HasRoyalties { get; set; }
    public bool HasDividends { get; set; }
    public bool HasRentalIncome { get; set; }
    public bool HasOtherIncome { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public required User User { get; set; }
}

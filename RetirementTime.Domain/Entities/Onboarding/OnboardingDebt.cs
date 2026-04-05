namespace RetirementTime.Domain.Entities.Onboarding;

public class OnboardingDebt
{
    public long Id { get; set; }
    public long UserId { get; set; }
    
    public bool HasPrimaryMortgage { get; set; }
    public bool HasInvestmentPropertyMortgage { get; set; }
    public bool HasCarPayments { get; set; }
    public bool HasStudentLoans { get; set; }
    public bool HasCreditCardDebt { get; set; }
    public bool HasPersonalLoans { get; set; }
    public bool HasBusinessLoans { get; set; }
    public bool HasTaxDebt { get; set; }
    public bool HasMedicalDebt { get; set; }
    public bool HasInformalDebt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public required User User { get; set; }
}

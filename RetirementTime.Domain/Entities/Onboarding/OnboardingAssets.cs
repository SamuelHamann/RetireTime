namespace RetirementTime.Domain.Entities.Onboarding;

public class OnboardingAssets
{
    public long Id { get; set; }
    public long UserId { get; set; }
    
    // Financial Assets
    public bool HasSavingsAccount { get; set; }
    public bool HasTFSA { get; set; }
    public bool HasRRSP { get; set; }
    public bool HasRRIF { get; set; }
    public bool HasFHSA { get; set; }
    public bool HasRESP { get; set; }
    public bool HasRDSP { get; set; }
    public bool HasNonRegistered { get; set; }
    public bool HasPension { get; set; }
    
    // Physical Assets
    public bool HasPrincipalResidence { get; set; }
    public bool HasCar { get; set; }
    public bool HasInvestmentProperty { get; set; }
    public bool HasBusiness { get; set; }
    public bool HasIncorporation { get; set; }
    public bool HasPreciousMetals { get; set; }
    public bool HasOtherHardAssets { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public required User User { get; set; }
}

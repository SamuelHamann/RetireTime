namespace RetirementTime.Domain.Entities.Common;

public class AccountType
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum InvestmentAccountType
{
    RRSP = 1,           // Registered Retirement Savings Plan
    RRIF = 2,           // Registered Retirement Income Fund
    TFSA = 3,           // Tax-Free Savings Account
    RESP = 4,           // Registered Education Savings Plan
    RDSP = 5,           // Registered Disability Savings Plan
    FHSA = 6,           // First Home Savings Account
    NonRegistered = 7,  // Non-Registered (Taxable) Account
    LIRA = 8,           // Locked-In Retirement Account
    LIF = 9,            // Life Income Fund
    PRIF = 10,          // Prescribed Retirement Income Fund
    RLSP = 11,          // Restricted Locked-In Savings Plan (Quebec)
    RLIF = 12,          // Restricted Life Income Fund (Quebec)
}
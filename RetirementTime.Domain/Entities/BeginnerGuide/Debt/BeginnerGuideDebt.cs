namespace RetirementTime.Domain.Entities.BeginnerGuide.Debt;

public class BeginnerGuideDebt
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public BeginnerGuideDebtType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal MonthlyPayment { get; set; }
    public decimal InterestRate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User? User { get; set; }
}


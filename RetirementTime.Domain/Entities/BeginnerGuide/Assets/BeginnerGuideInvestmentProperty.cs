namespace RetirementTime.Domain.Entities.BeginnerGuide.Assets;

public class BeginnerGuideInvestmentProperty
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public required string Name { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal MonthlyMortgagePayments { get; set; }
    public decimal MortgageLeft { get; set; }
    public decimal YearlyInsurance { get; set; }
    public decimal? MonthlyElectricityCosts { get; set; }
    public int MortgageDuration { get; set; }
    public DateTime MortgageStartDate { get; set; }
    public decimal? EstimatedValue { get; set; }
    public decimal MonthlyCost { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    
    public virtual User? User { get; set; }
}


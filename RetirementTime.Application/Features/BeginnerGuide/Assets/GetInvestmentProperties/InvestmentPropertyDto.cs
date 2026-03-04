namespace RetirementTime.Application.Features.BeginnerGuide.Assets.GetInvestmentProperties;

public class InvestmentPropertyDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
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
}


namespace RetirementTime.Models.BeginnerGuide.Assets;

public class PropertyData
{
    public decimal PurchasePrice { get; set; }
    public decimal MonthlyMortgagePayments { get; set; }
    public decimal MortgageLeft { get; set; }
    public decimal YearlyInsurance { get; set; }
    public decimal? MonthlyElectricityCosts { get; set; }
    public int MortgageDuration { get; set; }
    public System.DateTime? MortgageStartDate { get; set; }
    public decimal? EstimatedValue { get; set; }
}


using System.ComponentModel.DataAnnotations;

namespace RetirementTime.Models.BeginnerGuide.InvestmentProperties;

public class InvestmentPropertyFormModel
{
    public bool HasInvestmentProperties { get; set; }
    public List<InvestmentPropertyItemModel> Properties { get; set; } = new();
}

public class InvestmentPropertyItemModel
{
    public long? Id { get; set; }
    
    [Required(ErrorMessage = "Property name is required")]
    [StringLength(200, ErrorMessage = "Property name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Purchase price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0")]
    public decimal PurchasePrice { get; set; }
    
    [Required(ErrorMessage = "Monthly mortgage payment is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Monthly mortgage payment cannot be negative")]
    public decimal MonthlyMortgagePayments { get; set; }
    
    [Required(ErrorMessage = "Mortgage left is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Mortgage left cannot be negative")]
    public decimal MortgageLeft { get; set; }
    
    [Required(ErrorMessage = "Yearly insurance is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Yearly insurance cannot be negative")]
    public decimal YearlyInsurance { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Monthly electricity costs cannot be negative")]
    public decimal? MonthlyElectricityCosts { get; set; }
    
    [Required(ErrorMessage = "Mortgage duration is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Mortgage duration must be at least 1 year")]
    public int MortgageDuration { get; set; }
    
    [Required(ErrorMessage = "Mortgage start date is required")]
    public DateTime MortgageStartDate { get; set; } = DateTime.Today;
    
    [Range(0, double.MaxValue, ErrorMessage = "Estimated value cannot be negative")]
    public decimal? EstimatedValue { get; set; }
    
    [Required(ErrorMessage = "Monthly cost is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Monthly cost cannot be negative")]
    public decimal MonthlyCost { get; set; }
    
    [Required(ErrorMessage = "Monthly revenue is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Monthly revenue cannot be negative")]
    public decimal MonthlyRevenue { get; set; }
}


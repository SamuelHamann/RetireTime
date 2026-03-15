using System.ComponentModel.DataAnnotations;

namespace RetirementTime.Models.BeginnerGuide.Income;

public class SelfEmploymentFormModel
{
    public bool IsSelfEmployed { get; set; }
    public List<SelfEmploymentItemModel> SelfEmployments { get; set; } = new();
}

public class SelfEmploymentItemModel
{
    [Required(ErrorMessage = "Business name is required")]
    [MaxLength(200, ErrorMessage = "Business name cannot exceed 200 characters")]
    public string BusinessName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Annual salary is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Annual salary must be a positive number")]
    public decimal AnnualSalary { get; set; }

    [Required(ErrorMessage = "Average annual revenue increase is required")]
    [Range(-100, 100, ErrorMessage = "Average annual revenue increase must be between -100% and 100%")]
    public decimal AverageAnnualRevenueIncrease { get; set; } = 2.0m;

    [Required(ErrorMessage = "Monthly dividends is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Monthly dividends must be a positive number")]
    public decimal MonthlyDividends { get; set; }

    public List<SelfEmploymentAdditionalCompensationItemModel> AdditionalCompensations { get; set; } = new();
}

public class SelfEmploymentAdditionalCompensationItemModel
{
    [Required(ErrorMessage = "Compensation name is required")]
    [MaxLength(200, ErrorMessage = "Compensation name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Compensation amount is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Frequency is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a frequency")]
    public int FrequencyId { get; set; } = 7; // Default to Annually
}

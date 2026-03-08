using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RetirementTime.Models.BeginnerGuide.Income;

public class EmploymentFormModel
{
    public bool IsEmployed { get; set; }
    public List<EmploymentItemModel> Employments { get; set; } = new();
}

public class EmploymentItemModel
{
    public long? Id { get; set; }

    [Required(ErrorMessage = "Employer name is required")]
    [MaxLength(200, ErrorMessage = "Employer name cannot exceed 200 characters")]
    public string EmployerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Annual salary is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Annual salary must be greater than 0")]
    public decimal AnnualSalary { get; set; }

    [Required(ErrorMessage = "Average annual wage increase is required")]
    [Range(0, 100, ErrorMessage = "Average annual wage increase must be between 0 and 100")]
    public decimal AverageAnnualWageIncrease { get; set; } = 2m;

    public List<AdditionalCompensationItemModel> AdditionalCompensations { get; set; } = new();
}

public class AdditionalCompensationItemModel
{
    public long? Id { get; set; }

    [Required(ErrorMessage = "Compensation name is required")]
    [MaxLength(200, ErrorMessage = "Compensation name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Compensation amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Compensation amount must be greater than 0")]
    public decimal Amount { get; set; }
}


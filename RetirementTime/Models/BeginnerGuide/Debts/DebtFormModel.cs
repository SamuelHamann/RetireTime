using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RetirementTime.Domain.Entities.BeginnerGuide.Debt;

namespace RetirementTime.Models.BeginnerGuide.Debts;

public class DebtFormModel
{
    public bool HasDebts { get; set; }
    public List<DebtItemModel> Debts { get; set; } = new();
}

public class DebtItemModel
{
    public long? Id { get; set; }

    [Required(ErrorMessage = "Debt type is required")]
    public DebtType Type { get; set; }

    [Required(ErrorMessage = "Debt amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Debt amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Monthly payment is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Monthly payment cannot be negative")]
    public decimal MonthlyPayment { get; set; }

    [Required(ErrorMessage = "Interest rate is required")]
    [Range(0, 100, ErrorMessage = "Interest rate must be between 0 and 100")]
    public decimal InterestRate { get; set; }
}



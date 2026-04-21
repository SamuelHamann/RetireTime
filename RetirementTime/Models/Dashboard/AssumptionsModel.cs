using System.ComponentModel.DataAnnotations;

namespace RetirementTime.Models.Dashboard;

public class AssumptionsModel
{
    // General
    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    public decimal YearlyInflationRate { get; set; } = 2.0m;

    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    public decimal YearlyPropertyAppreciation { get; set; } = 3.0m;

    // Stocks
    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    public decimal StockAllocation { get; set; } = 60.0m;

    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    public decimal StockYearlyReturn { get; set; } = 7.0m;

    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    public decimal StockYearlyDividend { get; set; } = 2.0m;

    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    public decimal StockCanadianAllocation { get; set; } = 30.0m;

    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    public decimal StockForeignAllocation { get; set; } = 70.0m;

    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    public decimal StockFees { get; set; } = 0.2m;

    // Bonds
    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    public decimal BondAllocation { get; set; } = 30.0m;

    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    public decimal BondYearlyReturn { get; set; } = 3.0m;

    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    public decimal BondFees { get; set; } = 0.1m;

    // Cash
    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    public decimal CashAllocation { get; set; } = 10.0m;

    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    public decimal CashYearlyReturn { get; set; } = 1.5m;
}


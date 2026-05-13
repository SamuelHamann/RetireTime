namespace RetirementTime.Domain.Entities.Dashboard;

public class DashboardAssumptions
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public required DashboardScenario Scenario { get; set; }

    // General
    public decimal YearlyInflationRate { get; set; }
    public decimal YearlyPropertyAppreciation { get; set; }
    public decimal YearlyHouseMaintenance { get; set; }
    public decimal AnnualSalaryRaise { get; set; }
    public int LifeExpectancy { get; set; }

    // Stocks
    public decimal StockAllocation { get; set; }
    public decimal StockYearlyReturn { get; set; }
    public decimal StockYearlyDividend { get; set; }
    public decimal StockCanadianAllocation { get; set; }
    public decimal StockForeignAllocation { get; set; }
    public decimal StockFees { get; set; }

    // Bonds
    public decimal BondAllocation { get; set; }
    public decimal BondYearlyReturn { get; set; }
    public decimal BondFees { get; set; }

    // Cash
    public decimal CashAllocation { get; set; }
    public decimal CashYearlyReturn { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}


using MediatR;

namespace RetirementTime.Application.Features.Dashboard.SaveAssumptions;

public record SaveAssumptionsCommand : IRequest<SaveAssumptionsResult>
{
    public required long ScenarioId { get; init; }
    public required decimal YearlyInflationRate { get; init; }
    public required decimal YearlyPropertyAppreciation { get; init; }
    public required decimal YearlyHouseMaintenance { get; init; }
    public required decimal AnnualSalaryRaise { get; init; }
    public required int LifeExpectancy { get; init; }
    public required decimal StockAllocation { get; init; }
    public required decimal StockYearlyReturn { get; init; }
    public required decimal StockYearlyDividend { get; init; }
    public required decimal StockCanadianAllocation { get; init; }
    public required decimal StockForeignAllocation { get; init; }
    public required decimal StockFees { get; init; }
    public required decimal BondAllocation { get; init; }
    public required decimal BondYearlyReturn { get; init; }
    public required decimal BondFees { get; init; }
    public required decimal CashAllocation { get; init; }
    public required decimal CashYearlyReturn { get; init; }
}

public record SaveAssumptionsResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
}


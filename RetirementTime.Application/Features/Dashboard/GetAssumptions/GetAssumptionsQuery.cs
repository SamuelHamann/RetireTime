using MediatR;

namespace RetirementTime.Application.Features.Dashboard.GetAssumptions;

public record GetAssumptionsQuery : IRequest<GetAssumptionsResult>
{
    public required long ScenarioId { get; init; }
}

public record GetAssumptionsResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public AssumptionsDto? Assumptions { get; init; }
}

public record AssumptionsDto
{
    public long Id { get; init; }
    public decimal YearlyInflationRate { get; init; }
    public decimal YearlyPropertyAppreciation { get; init; }
    public decimal StockAllocation { get; init; }
    public decimal StockYearlyReturn { get; init; }
    public decimal StockYearlyDividend { get; init; }
    public decimal StockCanadianAllocation { get; init; }
    public decimal StockForeignAllocation { get; init; }
    public decimal StockFees { get; init; }
    public decimal BondAllocation { get; init; }
    public decimal BondYearlyReturn { get; init; }
    public decimal BondFees { get; init; }
    public decimal CashAllocation { get; init; }
    public decimal CashYearlyReturn { get; init; }
}


using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdateSelfEmploymentIncome;

public record UpdateSelfEmploymentIncomeCommand : IRequest<BaseResult>
{
    public required long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal? GrossSalary { get; init; }
    public int GrossSalaryFrequencyId { get; init; }
    public decimal? NetSalary { get; init; }
    public int NetSalaryFrequencyId { get; init; }
    public decimal? GrossDividends { get; init; }
    public int GrossDividendsFrequencyId { get; init; }
    public decimal? NetDividends { get; init; }
    public int NetDividendsFrequencyId { get; init; }
}

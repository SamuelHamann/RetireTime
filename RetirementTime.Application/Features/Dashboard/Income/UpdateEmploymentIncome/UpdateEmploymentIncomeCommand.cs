using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdateEmploymentIncome;

public record UpdateEmploymentIncomeCommand : IRequest<BaseResult>
{
    public required long Id { get; init; }
    public string EmployerName { get; init; } = string.Empty;
    public decimal? GrossSalary { get; init; }
    public int GrossSalaryFrequencyId { get; init; }
    public decimal? NetSalary { get; init; }
    public int NetSalaryFrequencyId { get; init; }
    public decimal? GrossCommissions { get; init; }
    public int GrossCommissionsFrequencyId { get; init; }
    public decimal? NetCommissions { get; init; }
    public int NetCommissionsFrequencyId { get; init; }
    public decimal? GrossBonus { get; init; }
    public int GrossBonusFrequencyId { get; init; }
    public decimal? NetBonus { get; init; }
    public int NetBonusFrequencyId { get; init; }
}

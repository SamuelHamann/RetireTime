using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdateEmploymentIncome;

public record UpdateEmploymentIncomeCommand : IRequest<BaseResult>
{
    public required long Id { get; init; }
    public string EmployerName { get; init; } = string.Empty;
    public decimal? GrossSalary { get; init; }
    public decimal? NetSalary { get; init; }
    public decimal? GrossCommissions { get; init; }
    public decimal? NetCommissions { get; init; }
    public decimal? GrossBonus { get; init; }
    public decimal? NetBonus { get; init; }
}

using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.UpdateSharePurchasePlan;

public record UpdateSharePurchasePlanCommand : IRequest<BaseResult>
{
    public required long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal? PercentOfSalaryEmployee { get; init; }
    public decimal? PercentOfSalaryEmployer { get; init; }
    public bool UseFlatAmountInsteadOfPercent { get; init; }
}

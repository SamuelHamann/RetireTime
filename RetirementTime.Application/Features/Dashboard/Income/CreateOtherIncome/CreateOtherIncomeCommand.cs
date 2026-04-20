using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateOtherIncome;

public record CreateOtherIncomeCommand(long EmploymentIncomeId) : IRequest<CreateOtherIncomeResult>;

public record CreateOtherIncomeResult : BaseResult
{
    public long OtherIncomeId { get; init; }
}

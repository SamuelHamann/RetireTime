using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateEmploymentIncome;

public record CreateEmploymentIncomeCommand(long ScenarioId, long UserId) : IRequest<CreateEmploymentIncomeResult>;

public record CreateEmploymentIncomeResult : BaseResult
{
    public long EmploymentIncomeId { get; init; }
}

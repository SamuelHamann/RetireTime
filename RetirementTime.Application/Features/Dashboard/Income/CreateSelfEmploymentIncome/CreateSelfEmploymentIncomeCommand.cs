using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateSelfEmploymentIncome;

public record CreateSelfEmploymentIncomeCommand(long ScenarioId, long TimelineId) : IRequest<CreateSelfEmploymentIncomeResult>;

public record CreateSelfEmploymentIncomeResult : BaseResult
{
    public long SelfEmploymentIncomeId { get; init; }
}

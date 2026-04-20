using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateSharePurchasePlan;

public record CreateSharePurchasePlanCommand(long ScenarioId) : IRequest<CreateSharePurchasePlanResult>;

public record CreateSharePurchasePlanResult : BaseResult
{
    public long PlanId { get; init; }
}

using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateGroupRrsp;

public record CreateGroupRrspCommand(long ScenarioId, long TimelineId) : IRequest<CreateGroupRrspResult>;

public record CreateGroupRrspResult : BaseResult
{
    public long PlanId { get; init; }
}

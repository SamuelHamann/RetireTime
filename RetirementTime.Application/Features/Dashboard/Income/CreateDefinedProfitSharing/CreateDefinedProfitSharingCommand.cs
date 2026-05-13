using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.CreateDefinedProfitSharing;

public record CreateDefinedProfitSharingCommand(long ScenarioId, long TimelineId) : IRequest<CreateDefinedProfitSharingResult>;

public record CreateDefinedProfitSharingResult : BaseResult
{
    public long PlanId { get; init; }
}

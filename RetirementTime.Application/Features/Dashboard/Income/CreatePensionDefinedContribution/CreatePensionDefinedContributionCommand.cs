using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.CreatePensionDefinedContribution;

public record CreatePensionDefinedContributionCommand(long ScenarioId, long TimelineId) : IRequest<CreatePensionDefinedContributionResult>;

public record CreatePensionDefinedContributionResult : BaseResult
{
    public long PlanId { get; init; }
}

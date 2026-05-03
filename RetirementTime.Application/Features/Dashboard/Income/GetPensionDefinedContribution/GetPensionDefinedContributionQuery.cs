using MediatR;
using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Application.Features.Dashboard.Income.GetPensionDefinedContribution;

public record GetPensionDefinedContributionQuery(long ScenarioId, long TimelineId) : IRequest<List<PensionDefinedContribution>>;

using MediatR;
using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Application.Features.Dashboard.Income.GetDefinedProfitSharing;

public record GetDefinedProfitSharingQuery(long ScenarioId, long TimelineId) : IRequest<List<DefinedProfitSharing>>;

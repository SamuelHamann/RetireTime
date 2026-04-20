using MediatR;
using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Application.Features.Dashboard.Income.GetGroupRrsp;

public record GetGroupRrspQuery(long ScenarioId) : IRequest<List<GroupRrsp>>;

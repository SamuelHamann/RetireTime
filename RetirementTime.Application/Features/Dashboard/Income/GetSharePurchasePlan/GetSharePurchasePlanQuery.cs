using MediatR;
using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Application.Features.Dashboard.Income.GetSharePurchasePlan;

public record GetSharePurchasePlanQuery(long ScenarioId) : IRequest<List<SharePurchasePlan>>;

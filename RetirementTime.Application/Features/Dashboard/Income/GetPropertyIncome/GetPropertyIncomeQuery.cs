using MediatR;
using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Application.Features.Dashboard.Income.GetPropertyIncome;

public record GetPropertyIncomeQuery(long ScenarioId, long TimelineId) : IRequest<List<PropertyIncome>>;


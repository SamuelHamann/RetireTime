using MediatR;
using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Application.Features.Dashboard.Income.GetSelfEmploymentIncomes;

public record GetSelfEmploymentIncomesQuery(long ScenarioId, long TimelineId) : IRequest<List<SelfEmploymentIncome>>;

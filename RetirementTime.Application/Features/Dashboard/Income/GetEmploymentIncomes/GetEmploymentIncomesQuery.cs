using MediatR;
using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Application.Features.Dashboard.Income.GetEmploymentIncomes;

public record GetEmploymentIncomesQuery(long ScenarioId, long TimelineId) : IRequest<List<EmploymentIncome>>;

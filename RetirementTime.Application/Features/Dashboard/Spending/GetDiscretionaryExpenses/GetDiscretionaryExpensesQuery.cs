using MediatR;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Application.Features.Dashboard.Spending.GetDiscretionaryExpenses;

public record GetDiscretionaryExpensesQuery(long ScenarioId, long TimelineId) : IRequest<GetDiscretionaryExpensesResult>;

public record GetDiscretionaryExpensesResult
{
    public SpendingDiscretionaryExpenses? Expenses { get; init; }
    public List<Frequency> Frequencies { get; init; } = [];
}

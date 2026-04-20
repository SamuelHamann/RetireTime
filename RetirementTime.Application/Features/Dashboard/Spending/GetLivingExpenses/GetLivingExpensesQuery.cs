using MediatR;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Application.Features.Dashboard.Spending.GetLivingExpenses;

public record GetLivingExpensesQuery(long ScenarioId) : IRequest<GetLivingExpensesResult>;

public record GetLivingExpensesResult
{
    public SpendingLivingExpenses? Expenses { get; init; }
    public List<Frequency> Frequencies { get; init; } = [];
}

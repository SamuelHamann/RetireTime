using MediatR;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Application.Features.Dashboard.Spending.GetOtherExpenses;

public record GetOtherExpensesQuery(long ScenarioId, long TimelineId) : IRequest<GetOtherExpensesResult>;
public record GetOtherExpensesResult
{
    public List<SpendingOtherExpense> Expenses { get; init; } = [];
    public List<Frequency> Frequencies { get; init; } = [];
}

public record CreateOtherExpenseCommand(long ScenarioId, long TimelineId) : IRequest<CreateSpendingItemResult>;

public record UpdateOtherExpenseCommand : IRequest<BaseResult>
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal? Amount { get; init; }
    public int FrequencyId { get; init; }
}

public record DeleteOtherExpenseCommand(long Id) : IRequest<BaseResult>;

public record CreateSpendingItemResult : BaseResult
{
    public long ItemId { get; init; }
}

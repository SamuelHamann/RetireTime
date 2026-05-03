using MediatR;
using RetirementTime.Application.Common;
using RetirementTime.Application.Features.Dashboard.Spending.GetOtherExpenses;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Application.Features.Dashboard.Spending.GetInvestmentExpenses;

// ── Query / Commands ──────────────────────────────────────────────────────────

public record GetInvestmentExpensesQuery(long ScenarioId, long TimelineId) : IRequest<GetInvestmentExpensesResult>;
public record GetInvestmentExpensesResult
{
    public List<SpendingInvestmentExpense> Expenses { get; init; } = [];
    public List<Frequency> Frequencies { get; init; } = [];
    public List<AssetsInvestmentAccount> InvestmentAccounts { get; init; } = [];
}

public record CreateInvestmentExpenseCommand(long ScenarioId, long TimelineId) : IRequest<CreateSpendingItemResult>;

public record UpdateInvestmentExpenseCommand : IRequest<BaseResult>
{
    public long Id { get; init; }
    public decimal? Amount { get; init; }
    public int FrequencyId { get; init; }
    public long? InvestmentAccountId { get; init; }
    public long? RetirementTimelineId { get; init; }
}

public record DeleteInvestmentExpenseCommand(long Id) : IRequest<BaseResult>;


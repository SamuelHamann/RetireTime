using MediatR;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Application.Features.Dashboard.Spending.GetDebtRepayments;

public record GetDebtRepaymentsQuery(long ScenarioId) : IRequest<GetDebtRepaymentsResult>;

public record GetDebtRepaymentsResult
{
    public List<SpendingDebtRepayment> Repayments { get; init; } = [];
    public List<GenericDebt> Debts { get; init; } = [];
    public List<Frequency> Frequencies { get; init; } = [];
}

public record CreateDebtRepaymentCommand(long ScenarioId, long? GenericDebtId = null) : IRequest<CreateSpendingItemResult>;

public record UpdateDebtRepaymentCommand : IRequest<BaseResult>
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal? Amount { get; init; }
    public int FrequencyId { get; init; }
    public long? GenericDebtId { get; init; }
}

public record DeleteDebtRepaymentCommand(long Id) : IRequest<BaseResult>;

public record CreateSpendingItemResult : BaseResult
{
    public long ItemId { get; init; }
}

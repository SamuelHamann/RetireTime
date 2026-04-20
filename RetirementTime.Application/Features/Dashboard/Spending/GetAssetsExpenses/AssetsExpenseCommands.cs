using MediatR;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Application.Features.Dashboard.Spending.GetAssetsExpenses;

public record GetAssetsExpensesQuery(long ScenarioId) : IRequest<GetAssetsExpensesResult>;

public record GetAssetsExpensesResult
{
    public List<SpendingAssetsExpense> Expenses { get; init; } = [];
    public AssetsHome? Home { get; init; }
    public List<AssetsInvestmentProperty> InvestmentProperties { get; init; } = [];
    public List<AssetsInvestmentAccount> InvestmentAccounts { get; init; } = [];
    public List<AssetsPhysicalAsset> PhysicalAssets { get; init; } = [];
    public List<Frequency> Frequencies { get; init; } = [];
}

public record CreateAssetsExpenseCommand(
    long ScenarioId,
    long? AssetsHomeId = null,
    long? AssetsInvestmentPropertyId = null,
    long? AssetsInvestmentAccountId = null,
    long? AssetsPhysicalAssetId = null) : IRequest<CreateSpendingItemResult>;

public record UpdateAssetsExpenseCommand : IRequest<BaseResult>
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal? Expense { get; init; }
    public int FrequencyId { get; init; }
    public long? AssetsHomeId { get; init; }
    public long? AssetsInvestmentPropertyId { get; init; }
    public long? AssetsInvestmentAccountId { get; init; }
    public long? AssetsPhysicalAssetId { get; init; }
}

public record DeleteAssetsExpenseCommand(long Id) : IRequest<BaseResult>;

public record CreateSpendingItemResult : BaseResult
{
    public long ItemId { get; init; }
}

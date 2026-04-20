using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Models.Spending;

public class SpendingDebtRepaymentItemModel
{
    public long Id { get; set; }
    public long? GenericDebtId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public int FrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
}

public class SpendingAssetsExpenseItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? Expense { get; set; }
    public int FrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public long? AssetsHomeId { get; set; }
    public long? AssetsInvestmentPropertyId { get; set; }
    public long? AssetsInvestmentAccountId { get; set; }
    public long? AssetsPhysicalAssetId { get; set; }
}

public class SpendingOtherExpenseItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public int FrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
}

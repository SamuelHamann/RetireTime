using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Models.Spending;

public class SpendingInvestmentExpenseItemModel
{
    public long Id { get; set; }
    public decimal? Amount { get; set; }
    public int FrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public long? InvestmentAccountId { get; set; }
}


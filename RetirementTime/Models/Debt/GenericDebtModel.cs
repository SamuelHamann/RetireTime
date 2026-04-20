using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Models.Debt;

public class HomeMortgageModel
{
    public decimal? Balance { get; set; }
    public decimal? InterestRate { get; set; }
    public int FrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public int? TermInYears { get; set; }
}

public class GenericDebtItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long DebtTypeId { get; set; }
    public decimal? Balance { get; set; }
    public decimal? InterestRate { get; set; }
    public int FrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public int? TermInYears { get; set; }
    public long? LinkedAssetId { get; set; }
    public string? LinkedAssetName { get; set; }
}

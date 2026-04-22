using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Models.Income;

public class OtherPersistingIncomeItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public int FrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public bool Taxable { get; set; } = true;
}

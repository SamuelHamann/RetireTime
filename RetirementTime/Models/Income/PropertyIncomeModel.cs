namespace RetirementTime.Models.Income;

public class PropertyIncomeItemModel
{
    public long Id { get; set; }
    public long? InvestmentPropertyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public int FrequencyId { get; set; }
}


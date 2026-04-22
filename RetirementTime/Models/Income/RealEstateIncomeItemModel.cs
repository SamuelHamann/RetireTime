namespace RetirementTime.Models.Income;

public class RealEstateIncomeItemModel
{
    public long Id { get; set; }
    public long? InvestmentPropertyId { get; set; }
    public string PropertyName { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public int? FrequencyId { get; set; }
}


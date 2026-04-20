namespace RetirementTime.Models.Asset;

public class AssetsInvestmentPropertyItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? PurchaseDate { get; set; }
    public decimal? PropertyValue { get; set; }
    public decimal? PurchasePrice { get; set; }
}

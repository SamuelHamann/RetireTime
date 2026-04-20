namespace RetirementTime.Models.Income;

public class SelfEmploymentItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? GrossSalary { get; set; }
    public decimal? NetSalary { get; set; }
    public decimal? GrossDividends { get; set; }
    public decimal? NetDividends { get; set; }
}

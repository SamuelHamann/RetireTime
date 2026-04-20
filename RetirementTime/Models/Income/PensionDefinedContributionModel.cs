namespace RetirementTime.Models.Income;

public class PensionDefinedContributionItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? PercentOfSalaryEmployee { get; set; }
    public decimal? PercentOfSalaryEmployer { get; set; }
}

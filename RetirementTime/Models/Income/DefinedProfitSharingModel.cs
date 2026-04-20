namespace RetirementTime.Models.Income;

public class DefinedProfitSharingItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? PercentOfSalaryEmployer { get; set; }
}

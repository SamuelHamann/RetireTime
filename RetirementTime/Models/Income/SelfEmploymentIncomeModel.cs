using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Models.Income;

public class SelfEmploymentItemModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? GrossSalary { get; set; }
    public int GrossSalaryFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? NetSalary { get; set; }
    public int NetSalaryFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? GrossDividends { get; set; }
    public int GrossDividendsFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? NetDividends { get; set; }
    public int NetDividendsFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
}

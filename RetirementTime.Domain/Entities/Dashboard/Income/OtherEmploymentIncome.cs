using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Entities.Dashboard.Income;

public class OtherEmploymentIncome
{
    public long Id { get; set; }
    public long EmploymentIncomeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? Gross { get; set; }
    public int GrossFrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public decimal? Net { get; set; }
    public int NetFrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public required EmploymentIncome EmploymentIncome { get; set; }
    public Frequency GrossFrequency { get; set; } = null!;
    public Frequency NetFrequency { get; set; } = null!;
}

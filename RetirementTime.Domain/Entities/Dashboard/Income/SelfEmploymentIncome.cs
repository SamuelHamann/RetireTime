using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Domain.Entities.Dashboard.Income;

public class SelfEmploymentIncome
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public long? RetirementTimelineId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? NetSalary { get; set; }
    public int NetSalaryFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? GrossSalary { get; set; }
    public int GrossSalaryFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? GrossDividends { get; set; }
    public int GrossDividendsFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? NetDividends { get; set; }
    public int NetDividendsFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public RetirementTimeline? RetirementTimeline { get; set; }
    public Frequency NetSalaryFrequency { get; set; } = null!;
    public Frequency GrossSalaryFrequency { get; set; } = null!;
    public Frequency GrossDividendsFrequency { get; set; } = null!;
    public Frequency NetDividendsFrequency { get; set; } = null!;
}
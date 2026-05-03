using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Domain.Entities.Dashboard.Income;

public class SharePurchasePlan
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public long? RetirementTimelineId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? PercentOfSalaryEmployee { get; set; } = 0;
    public int PurchaseFrequencyId { get; set; } = (int)FrequencyEnum.BiWeekly;
    public decimal? PercentOfSalaryEmployer { get; set; } = 0;
    public int EmployerMatchFrequencyId { get; set; } = (int)FrequencyEnum.BiWeekly;
    public bool UseFlatAmountInsteadOfPercent { get; set; } = false;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public RetirementTimeline? RetirementTimeline { get; set; }
    public Frequency PurchaseFrequency { get; set; } = null!;
    public Frequency EmployerMatchFrequency { get; set; } = null!;

}
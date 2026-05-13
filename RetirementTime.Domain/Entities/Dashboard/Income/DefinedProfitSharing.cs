using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Domain.Entities.Dashboard.Income;

public class DefinedProfitSharing
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public long? RetirementTimelineId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? PercentOfSalaryEmployer { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public RetirementTimeline? RetirementTimeline { get; set; }
}
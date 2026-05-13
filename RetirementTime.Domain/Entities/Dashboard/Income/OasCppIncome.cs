using RetirementTime.Domain.Entities.Dashboard.Spending;

namespace RetirementTime.Domain.Entities.Dashboard.Income;

public class OasCppIncome
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public long? RetirementTimelineId { get; set; }
    public decimal? IncomeLastYear { get; set; }
    public decimal? Income2YearsAgo { get; set; }
    public decimal? Income3YearsAgo { get; set; }
    public decimal? Income4YearsAgo { get; set; }
    public decimal? Income5YearsAgo { get; set; }
    public int? YearsSpentInCanada { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public RetirementTimeline? RetirementTimeline { get; set; }
}
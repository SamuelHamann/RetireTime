namespace RetirementTime.Domain.Entities.Dashboard;

public class DashboardScenario
{
    public long ScenarioId { get; set; }
    public required string ScenarioName { get; set; }
    public long UserId { get; set; }
    public bool ScenarioFullyCreated { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

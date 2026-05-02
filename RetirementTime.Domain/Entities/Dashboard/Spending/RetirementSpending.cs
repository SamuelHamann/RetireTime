namespace RetirementTime.Domain.Entities.Dashboard.Spending;

public class RetirementSpending
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public bool IsFullyCreated { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
}


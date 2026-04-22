using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Entities.Dashboard.PersistingIncome;

public class OtherPersistingIncome
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public int FrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    public bool Taxable { get; set; } = true;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public Frequency Frequency { get; set; } = null!;
}
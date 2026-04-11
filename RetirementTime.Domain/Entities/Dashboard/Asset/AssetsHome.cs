namespace RetirementTime.Domain.Entities.Dashboard.Asset;

public class AssetsHome
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal? HomeValue { get; set; }
    public decimal? PurchasePrice { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
}
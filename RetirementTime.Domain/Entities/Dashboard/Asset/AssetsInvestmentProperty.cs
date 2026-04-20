namespace RetirementTime.Domain.Entities.Dashboard.Asset;

public class AssetsInvestmentProperty
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal? PropertyValue { get; set; }
    public decimal? PurchasePrice { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
}
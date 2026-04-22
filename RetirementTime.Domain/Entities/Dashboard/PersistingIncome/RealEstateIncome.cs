using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;

namespace RetirementTime.Domain.Entities.Dashboard.PersistingIncome;

public class RealEstateIncome
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public long? InvestmentPropertyId { get; set; }
    public string PropertyName { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public int? FrequencyId { get; set; } = (int)FrequencyEnum.Annually;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public Frequency? Frequency { get; set; }
    public AssetsInvestmentProperty? InvestmentProperty { get; set; }
}
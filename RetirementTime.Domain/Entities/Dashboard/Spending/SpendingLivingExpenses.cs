using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Entities.Dashboard.Spending;

public class SpendingLivingExpenses
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }

    public decimal? RentOrMortgage { get; set; }
    public int RentOrMortgageFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public decimal? Food { get; set; }
    public int FoodFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public decimal? Utilities { get; set; }
    public int UtilitiesFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public decimal? Insurance { get; set; }
    public int InsuranceFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public decimal? Gas { get; set; }
    public int GasFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public decimal? HomeMaintenance { get; set; }
    public int HomeMaintenanceFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public decimal? PropertyTax { get; set; }
    public int PropertyTaxFrequencyId { get; set; } = (int)FrequencyEnum.Annually;

    public decimal? Cellphone { get; set; }
    public int CellphoneFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public decimal? HealthSpendings { get; set; }
    public int HealthSpendingsFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public decimal? OtherLivingExpenses { get; set; }
    public int OtherLivingExpensesFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public Frequency RentOrMortgageFrequency { get; set; } = null!;
    public Frequency FoodFrequency { get; set; } = null!;
    public Frequency UtilitiesFrequency { get; set; } = null!;
    public Frequency InsuranceFrequency { get; set; } = null!;
    public Frequency GasFrequency { get; set; } = null!;
    public Frequency HomeMaintenanceFrequency { get; set; } = null!;
    public Frequency PropertyTaxFrequency { get; set; } = null!;
    public Frequency CellphoneFrequency { get; set; } = null!;
    public Frequency HealthSpendingsFrequency { get; set; } = null!;
    public Frequency OtherLivingExpensesFrequency { get; set; } = null!;
}

using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Entities.Dashboard.Spending;

public class SpendingDiscretionaryExpenses
{
    public long Id { get; set; }
    public long ScenarioId { get; set; }
    public long? RetirementSpendingId { get; set; }

    public decimal? GymMembership { get; set; }
    public int GymMembershipFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public decimal? Subscriptions { get; set; }
    public int SubscriptionsFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public decimal? EatingOut { get; set; }
    public int EatingOutFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public decimal? Entertainment { get; set; }
    public int EntertainmentFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public decimal? Travel { get; set; }
    public int TravelFrequencyId { get; set; } = (int)FrequencyEnum.Annually;

    public decimal? CharitableDonations { get; set; }
    public int CharitableDonationsFrequencyId { get; set; } = (int)FrequencyEnum.Annually;

    public decimal? OtherDiscretionaryExpenses { get; set; }
    public int OtherDiscretionaryExpensesFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    // Grouped entry mode
    public bool UseGroupedEntry { get; set; }
    public decimal? GroupedAmount { get; set; }
    public int GroupedFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public DashboardScenario Scenario { get; set; } = null!;
    public RetirementSpending? RetirementSpending { get; set; }
    public Frequency GymMembershipFrequency { get; set; } = null!;
    public Frequency SubscriptionsFrequency { get; set; } = null!;
    public Frequency EatingOutFrequency { get; set; } = null!;
    public Frequency EntertainmentFrequency { get; set; } = null!;
    public Frequency TravelFrequency { get; set; } = null!;
    public Frequency CharitableDonationsFrequency { get; set; } = null!;
    public Frequency OtherDiscretionaryExpensesFrequency { get; set; } = null!;
    public Frequency GroupedFrequency { get; set; } = null!;
}

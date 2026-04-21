using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Models.Spending;

public class SpendingDiscretionaryExpensesModel
{
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
}

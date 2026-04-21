using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Spending.SaveDiscretionaryExpenses;

public record SaveDiscretionaryExpensesCommand : IRequest<BaseResult>
{
    public long ScenarioId { get; init; }

    public decimal? GymMembership { get; init; }
    public int GymMembershipFrequencyId { get; init; }
    public decimal? Subscriptions { get; init; }
    public int SubscriptionsFrequencyId { get; init; }
    public decimal? EatingOut { get; init; }
    public int EatingOutFrequencyId { get; init; }
    public decimal? Entertainment { get; init; }
    public int EntertainmentFrequencyId { get; init; }
    public decimal? Travel { get; init; }
    public int TravelFrequencyId { get; init; }
    public decimal? CharitableDonations { get; init; }
    public int CharitableDonationsFrequencyId { get; init; }
    public decimal? OtherDiscretionaryExpenses { get; init; }
    public int OtherDiscretionaryExpensesFrequencyId { get; init; }

    public bool UseGroupedEntry { get; init; }
    public decimal? GroupedAmount { get; init; }
    public int GroupedFrequencyId { get; init; }
}

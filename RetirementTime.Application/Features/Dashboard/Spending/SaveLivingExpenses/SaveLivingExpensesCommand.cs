using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Spending.SaveLivingExpenses;

public record SaveLivingExpensesCommand : IRequest<BaseResult>
{
    public long ScenarioId { get; init; }

    public decimal? RentOrMortgage { get; init; }
    public int RentOrMortgageFrequencyId { get; init; }
    public decimal? Food { get; init; }
    public int FoodFrequencyId { get; init; }
    public decimal? Utilities { get; init; }
    public int UtilitiesFrequencyId { get; init; }
    public decimal? Insurance { get; init; }
    public int InsuranceFrequencyId { get; init; }
    public decimal? Gas { get; init; }
    public int GasFrequencyId { get; init; }
    public decimal? HomeMaintenance { get; init; }
    public int HomeMaintenanceFrequencyId { get; init; }
    public decimal? Cellphone { get; init; }
    public int CellphoneFrequencyId { get; init; }
    public decimal? HealthSpendings { get; init; }
    public int HealthSpendingsFrequencyId { get; init; }
    public decimal? OtherLivingExpenses { get; init; }
    public int OtherLivingExpensesFrequencyId { get; init; }
}

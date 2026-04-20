using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Models.Spending;

public class SpendingLivingExpensesModel
{
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
    public decimal? Cellphone { get; set; }
    public int CellphoneFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? HealthSpendings { get; set; }
    public int HealthSpendingsFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
    public decimal? OtherLivingExpenses { get; set; }
    public int OtherLivingExpensesFrequencyId { get; set; } = (int)FrequencyEnum.Monthly;
}

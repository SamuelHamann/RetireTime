using RetirementTime.Domain.Entities.Dashboard.Income;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IEmploymentIncomeRepository
{
    Task<List<EmploymentIncome>> GetByScenarioIdAsync(long scenarioId);
    Task<EmploymentIncome> CreateAsync(EmploymentIncome employmentIncome);
    Task<bool> UpdateAsync(EmploymentIncome employmentIncome);
    Task<bool> DeleteAsync(long employmentIncomeId);
    Task<OtherEmploymentIncome> CreateOtherIncomeAsync(OtherEmploymentIncome otherIncome);
    Task<bool> UpdateOtherIncomeAsync(OtherEmploymentIncome otherIncome);
    Task<bool> DeleteOtherIncomeAsync(long otherIncomeId);
}

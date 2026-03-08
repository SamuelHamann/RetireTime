using RetirementTime.Domain.Entities.BeginnerGuide.Debt;

namespace RetirementTime.Domain.Interfaces.Repositories;

public interface IBeginnerGuideDebtRepository
{
    Task<List<BeginnerGuideDebt>> GetByUserIdAsync(long userId);
    Task<List<BeginnerGuideDebt>> UpsertDebtsAsync(long userId, List<BeginnerGuideDebt> debts);
    Task DeleteByUserIdAsync(long userId);
}


using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Infrastructure.Repositories;

public class RetirementTimelineRepository(ApplicationDbContext context) : IRetirementTimelineRepository
{
    public async Task<List<RetirementTimeline>> GetByScenarioIdAsync(long scenarioId) =>
        await context.RetirementTimelines
            .Where(e => e.ScenarioId == scenarioId)
            .OrderBy(e => e.AgeFrom)
            .ToListAsync();

    public async Task<RetirementTimeline?> GetByIdAsync(long id) =>
        await context.RetirementTimelines.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<RetirementTimeline> CreateAsync(RetirementTimeline entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        context.RetirementTimelines.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(RetirementTimeline entity)
    {
        var existing = await context.RetirementTimelines.FirstOrDefaultAsync(e => e.Id == entity.Id);
        if (existing is null) return;

        existing.Name           = entity.Name;
        existing.AgeFrom        = entity.AgeFrom;
        existing.AgeTo          = entity.AgeTo;
        existing.TimelineType   = entity.TimelineType;
        existing.IsFullyCreated = entity.IsFullyCreated;
        existing.UpdatedAt      = DateTime.UtcNow;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var existing = await context.RetirementTimelines.FirstOrDefaultAsync(e => e.Id == id);
        if (existing is null) return;
        context.RetirementTimelines.Remove(existing);
        await context.SaveChangesAsync();
    }

    public async Task CloneIncomeFromTimelineAsync(long scenarioId, long sourceTimelineId, long targetTimelineId)
    {
        var now = DateTime.UtcNow;

        // Employment incomes
        var empList = await context.EmploymentIncomes.AsNoTracking()
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId)
            .ToListAsync();
        foreach (var s in empList)
            context.EmploymentIncomes.Add(new RetirementTime.Domain.Entities.Dashboard.Income.EmploymentIncome
            {
                ScenarioId = scenarioId, UserId = s.UserId, RetirementTimelineId = targetTimelineId,
                EmployerName = s.EmployerName,
                GrossSalary = s.GrossSalary, GrossSalaryFrequencyId = s.GrossSalaryFrequencyId,
                NetSalary = s.NetSalary, NetSalaryFrequencyId = s.NetSalaryFrequencyId,
                GrossCommissions = s.GrossCommissions, GrossCommissionsFrequencyId = s.GrossCommissionsFrequencyId,
                NetCommissions = s.NetCommissions, NetCommissionsFrequencyId = s.NetCommissionsFrequencyId,
                GrossBonus = s.GrossBonus, GrossBonusFrequencyId = s.GrossBonusFrequencyId,
                NetBonus = s.NetBonus, NetBonusFrequencyId = s.NetBonusFrequencyId,
                PensionContributions = s.PensionContributions, PensionContributionFrequencyId = s.PensionContributionFrequencyId,
                TaxDeductions = s.TaxDeductions, TaxDeductionFrequencyId = s.TaxDeductionFrequencyId,
                CppDeductions = s.CppDeductions, CppDeductionFrequencyId = s.CppDeductionFrequencyId,
                OtherDeductions = s.OtherDeductions, OtherDeductionFrequencyId = s.OtherDeductionFrequencyId,
                CreatedAt = now, UpdatedAt = now
            });

        // Self employment incomes
        var selfList = await context.SelfEmploymentIncomes.AsNoTracking()
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId)
            .ToListAsync();
        foreach (var s in selfList)
            context.SelfEmploymentIncomes.Add(new RetirementTime.Domain.Entities.Dashboard.Income.SelfEmploymentIncome
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                Name = s.Name,
                GrossSalary = s.GrossSalary, GrossSalaryFrequencyId = s.GrossSalaryFrequencyId,
                NetSalary = s.NetSalary, NetSalaryFrequencyId = s.NetSalaryFrequencyId,
                GrossDividends = s.GrossDividends, GrossDividendsFrequencyId = s.GrossDividendsFrequencyId,
                NetDividends = s.NetDividends, NetDividendsFrequencyId = s.NetDividendsFrequencyId,
                CreatedAt = now, UpdatedAt = now
            });

        // Pension defined benefits
        var pdbList = await context.PensionDefinedBenefits.AsNoTracking()
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId)
            .ToListAsync();
        foreach (var s in pdbList)
            context.PensionDefinedBenefits.Add(new RetirementTime.Domain.Entities.Dashboard.Income.PensionDefinedBenefits
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                Name = s.Name, StartAge = s.StartAge,
                BenefitsPre65 = s.BenefitsPre65, BenefitsPre65FrequencyId = s.BenefitsPre65FrequencyId,
                BenefitsPost65 = s.BenefitsPost65, BenefitsPost65FrequencyId = s.BenefitsPost65FrequencyId,
                PercentIndexedToInflation = s.PercentIndexedToInflation,
                PercentSurvivorBenefits = s.PercentSurvivorBenefits,
                RrspAdjustment = s.RrspAdjustment, RrspAdjustmentFrequencyId = s.RrspAdjustmentFrequencyId,
                CreatedAt = now, UpdatedAt = now
            });

        // Pension defined contribution
        var pdcList = await context.PensionDefinedContributions.AsNoTracking()
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId)
            .ToListAsync();
        foreach (var s in pdcList)
            context.PensionDefinedContributions.Add(new RetirementTime.Domain.Entities.Dashboard.Income.PensionDefinedContribution
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                Name = s.Name,
                PercentOfSalaryEmployee = s.PercentOfSalaryEmployee,
                PercentOfSalaryEmployer = s.PercentOfSalaryEmployer,
                CreatedAt = now, UpdatedAt = now
            });

        // Group RRSP
        var rrspList = await context.GroupRrsps.AsNoTracking()
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId)
            .ToListAsync();
        foreach (var s in rrspList)
            context.GroupRrsps.Add(new RetirementTime.Domain.Entities.Dashboard.Income.GroupRrsp
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                Name = s.Name,
                PercentOfSalaryEmployee = s.PercentOfSalaryEmployee,
                PercentOfSalaryEmployer = s.PercentOfSalaryEmployer,
                CreatedAt = now, UpdatedAt = now
            });

        // Defined profit sharing
        var dpsList = await context.DefinedProfitSharings.AsNoTracking()
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId)
            .ToListAsync();
        foreach (var s in dpsList)
            context.DefinedProfitSharings.Add(new RetirementTime.Domain.Entities.Dashboard.Income.DefinedProfitSharing
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                Name = s.Name,
                PercentOfSalaryEmployer = s.PercentOfSalaryEmployer,
                CreatedAt = now, UpdatedAt = now
            });

        // Share purchase plans
        var sppList = await context.SharePurchasePlans.AsNoTracking()
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId)
            .ToListAsync();
        foreach (var s in sppList)
            context.SharePurchasePlans.Add(new RetirementTime.Domain.Entities.Dashboard.Income.SharePurchasePlan
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                Name = s.Name,
                PercentOfSalaryEmployee = s.PercentOfSalaryEmployee, PurchaseFrequencyId = s.PurchaseFrequencyId,
                PercentOfSalaryEmployer = s.PercentOfSalaryEmployer, EmployerMatchFrequencyId = s.EmployerMatchFrequencyId,
                UseFlatAmountInsteadOfPercent = s.UseFlatAmountInsteadOfPercent,
                CreatedAt = now, UpdatedAt = now
            });

        // OAS/CPP income
        var oasList = await context.OasCppIncomes.AsNoTracking()
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId)
            .ToListAsync();
        foreach (var s in oasList)
            context.OasCppIncomes.Add(new RetirementTime.Domain.Entities.Dashboard.Income.OasCppIncome
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                IncomeLastYear = s.IncomeLastYear, Income2YearsAgo = s.Income2YearsAgo,
                Income3YearsAgo = s.Income3YearsAgo, Income4YearsAgo = s.Income4YearsAgo,
                Income5YearsAgo = s.Income5YearsAgo, YearsSpentInCanada = s.YearsSpentInCanada,
                CreatedAt = now, UpdatedAt = now
            });

        // Other income or benefits
        var otherList = await context.OtherIncomeOrBenefits.AsNoTracking()
            .Where(e => e.ScenarioId == scenarioId && e.RetirementTimelineId == sourceTimelineId)
            .ToListAsync();
        foreach (var s in otherList)
            context.OtherIncomeOrBenefits.Add(new RetirementTime.Domain.Entities.Dashboard.Income.OtherIncomeOrBenefits
            {
                ScenarioId = scenarioId, RetirementTimelineId = targetTimelineId,
                Name = s.Name, Amount = s.Amount, FrequencyId = s.FrequencyId,
                CreatedAt = now, UpdatedAt = now
            });

        await context.SaveChangesAsync();
    }
}


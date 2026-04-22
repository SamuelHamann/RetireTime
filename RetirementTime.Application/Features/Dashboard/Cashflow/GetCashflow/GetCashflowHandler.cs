using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Cashflow.GetCashflow;

public partial class GetCashflowHandler(
    IEmploymentIncomeRepository employmentIncomeRepository,
    ISelfEmploymentIncomeRepository selfEmploymentIncomeRepository,
    IPensionDefinedBenefitsRepository pensionDefinedBenefitsRepository,
    IOtherIncomeOrBenefitsRepository otherIncomeOrBenefitsRepository,
    ISpendingRepository spendingRepository,
    IOnboardingPersonalInfoRepository onboardingPersonalInfoRepository,
    IOnboardingEmploymentRepository onboardingEmploymentRepository,
    IDashboardScenarioRepository scenarioRepository,
    IDashboardAssumptionsRepository assumptionsRepository,
    ILogger<GetCashflowHandler> logger) : IRequestHandler<GetCashflowQuery, GetCashflowResult>
{
    public async Task<GetCashflowResult> Handle(GetCashflowQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var scenario = await scenarioRepository.GetByIdAsync(request.ScenarioId);
            var personalInfo = await onboardingPersonalInfoRepository.GetByUserId(scenario.UserId);
            var onboardingEmployment = await onboardingEmploymentRepository.GetByUserId(scenario.UserId);
            
            var frequencies = await spendingRepository.GetFrequenciesAsync();

            // ── Income ───────────────────────────────────────────────────────

            var employments     = await employmentIncomeRepository.GetByScenarioIdAsync(request.ScenarioId);
            var selfEmployments = await selfEmploymentIncomeRepository.GetByScenarioIdAsync(request.ScenarioId);
            var pensions        = await pensionDefinedBenefitsRepository.GetByScenarioIdAsync(request.ScenarioId);
            var otherIncomes    = await otherIncomeOrBenefitsRepository.GetByScenarioIdAsync(request.ScenarioId);

            decimal employmentTotal = employments.Sum(e =>
                ToAnnual(e.NetSalary, e.NetSalaryFrequencyId, frequencies) +
                ToAnnual(e.NetCommissions, e.NetCommissionsFrequencyId, frequencies) +
                ToAnnual(e.NetBonus, e.NetBonusFrequencyId, frequencies));

            decimal selfEmploymentTotal = selfEmployments.Sum(e =>
                ToAnnual(e.NetSalary, e.NetSalaryFrequencyId, frequencies) +
                ToAnnual(e.NetDividends, e.NetDividendsFrequencyId, frequencies));

            decimal pensionTotal = pensions.Sum(e =>
                ToAnnual(e.BenefitsPre65, e.BenefitsPre65FrequencyId, frequencies));

            decimal otherIncomeTotal = otherIncomes.Sum(e =>
                ToAnnual(e.Amount, e.FrequencyId ?? (int)FrequencyEnum.Annually, frequencies));

            decimal totalIncome = employmentTotal + selfEmploymentTotal + pensionTotal + otherIncomeTotal;

            // ── Expenses ─────────────────────────────────────────────────────

            var living       = await spendingRepository.GetLivingExpensesAsync(request.ScenarioId);
            var discretionary = await spendingRepository.GetDiscretionaryExpensesAsync(request.ScenarioId);
            var debtRepayments = await spendingRepository.GetDebtRepaymentsAsync(request.ScenarioId);
            var assetsExpenses = await spendingRepository.GetAssetsExpensesAsync(request.ScenarioId);
            var otherExpenses  = await spendingRepository.GetOtherExpensesAsync(request.ScenarioId);

            decimal livingTotal = living == null ? 0m :
                ToAnnual(living.RentOrMortgage, living.RentOrMortgageFrequencyId, frequencies) +
                ToAnnual(living.Food, living.FoodFrequencyId, frequencies) +
                ToAnnual(living.Utilities, living.UtilitiesFrequencyId, frequencies) +
                ToAnnual(living.Insurance, living.InsuranceFrequencyId, frequencies) +
                ToAnnual(living.Gas, living.GasFrequencyId, frequencies) +
                ToAnnual(living.HomeMaintenance, living.HomeMaintenanceFrequencyId, frequencies) +
                ToAnnual(living.Cellphone, living.CellphoneFrequencyId, frequencies) +
                ToAnnual(living.HealthSpendings, living.HealthSpendingsFrequencyId, frequencies) +
                ToAnnual(living.OtherLivingExpenses, living.OtherLivingExpensesFrequencyId, frequencies);

            decimal discretionaryTotal = discretionary == null ? 0m :
                discretionary.UseGroupedEntry
                    ? ToAnnual(discretionary.GroupedAmount, discretionary.GroupedFrequencyId, frequencies)
                    : ToAnnual(discretionary.GymMembership, discretionary.GymMembershipFrequencyId, frequencies) +
                      ToAnnual(discretionary.Subscriptions, discretionary.SubscriptionsFrequencyId, frequencies) +
                      ToAnnual(discretionary.EatingOut, discretionary.EatingOutFrequencyId, frequencies) +
                      ToAnnual(discretionary.Entertainment, discretionary.EntertainmentFrequencyId, frequencies) +
                      ToAnnual(discretionary.Travel, discretionary.TravelFrequencyId, frequencies) +
                      ToAnnual(discretionary.CharitableDonations, discretionary.CharitableDonationsFrequencyId, frequencies) +
                      ToAnnual(discretionary.OtherDiscretionaryExpenses, discretionary.OtherDiscretionaryExpensesFrequencyId, frequencies);

            decimal debtTotal     = debtRepayments.Sum(e => ToAnnual(e.Amount, e.FrequencyId, frequencies));
            decimal assetsTotal   = assetsExpenses.Sum(e => ToAnnual(e.Expense, e.FrequencyId, frequencies));
            decimal otherExpTotal = otherExpenses.Sum(e => ToAnnual(e.Amount, e.FrequencyId, frequencies));

            decimal totalExpenses = livingTotal + discretionaryTotal + debtTotal + assetsTotal + otherExpTotal;
            decimal savings = Math.Max(0m, totalIncome - totalExpenses);

            // ── Build Sankey ─────────────────────────────────────────────────

            var nodes = new List<CashflowNodeDto>();
            var links = new List<CashflowLinkDto>();

            // Only include nodes/links with non-zero values
            void AddIncomeSource(string label, decimal amount)
            {
                if (amount <= 0) return;
                nodes.Add(new CashflowNodeDto { Name = label, Category = "income" });
                links.Add(new CashflowLinkDto { Source = label, Target = request.Label_TotalIncome, Value = amount });
            }

            void AddExpenseCategory(string label, decimal amount)
            {
                if (amount <= 0) return;
                nodes.Add(new CashflowNodeDto { Name = label, Category = "expenses" });
                links.Add(new CashflowLinkDto { Source = request.Label_TotalExpenses, Target = label, Value = amount });
            }

            AddIncomeSource(request.Label_Employment,     employmentTotal);
            AddIncomeSource(request.Label_SelfEmployment, selfEmploymentTotal);
            AddIncomeSource(request.Label_DefinedBenefits, pensionTotal);
            AddIncomeSource(request.Label_OtherIncome,    otherIncomeTotal);

            nodes.Add(new CashflowNodeDto { Name = request.Label_TotalIncome, Category = "income" });

            if (totalExpenses > 0)
            {
                nodes.Add(new CashflowNodeDto { Name = request.Label_TotalExpenses, Category = "expenses" });
                links.Add(new CashflowLinkDto { Source = request.Label_TotalIncome, Target = request.Label_TotalExpenses, Value = totalExpenses });
            }

            if (savings > 0)
            {
                nodes.Add(new CashflowNodeDto { Name = request.Label_Savings, Category = "savings" });
                links.Add(new CashflowLinkDto { Source = request.Label_TotalIncome, Target = request.Label_Savings, Value = savings });
            }

            AddExpenseCategory(request.Label_LivingExpenses,      livingTotal);
            AddExpenseCategory(request.Label_DiscretionaryExpenses, discretionaryTotal);
            AddExpenseCategory(request.Label_DebtRepayments,       debtTotal);
            AddExpenseCategory(request.Label_AssetsExpenses,       assetsTotal);
            AddExpenseCategory(request.Label_OtherExpenses,        otherExpTotal);

            var assumptions = await assumptionsRepository.GetByScenarioIdAsync(request.ScenarioId);

            var mortgageExpense = living == null ? 0m : ToAnnual(living.RentOrMortgage, living.RentOrMortgageFrequencyId, frequencies);
            var currentSalary = employmentTotal;
            var currentOtherIncome = totalIncome - currentSalary;
            // Non-mortgage, non-debt expenses that grow with inflation
            var inflationExpenses = livingTotal - mortgageExpense + discretionaryTotal + assetsTotal + otherExpTotal;
            // Debt repayments stay fixed
            var fixedDebtExpenses = debtTotal;

            var inflationRate = assumptions != null ? assumptions.YearlyInflationRate / 100m : 0.02m;
            var salaryRaiseRate = assumptions != null ? assumptions.AnnualSalaryRaise / 100m : 0.0275m;
            var lifeExpectancy = assumptions != null ? assumptions.LifeExpectancy : 90;
            var retirementAge = onboardingEmployment?.PlannedRetirementAge;

            var currentAge = CalculateAge(personalInfo.DateOfBirth);
            var yearlyCashFlows = new List<YearlyCashFlowDto>();

            var salary = currentSalary;
            var otherIncome = currentOtherIncome;
            var varExpenses = inflationExpenses;

            for (var age = currentAge; age <= lifeExpectancy; age++)
            {
                var isRetired = retirementAge.HasValue && age >= retirementAge.Value;
                var incomeThisYear = (isRetired ? 0m : salary) + otherIncome;

                yearlyCashFlows.Add(new YearlyCashFlowDto
                {
                    Year = age,
                    TotalIncome = incomeThisYear,
                    TotalExpenses = varExpenses + mortgageExpense + fixedDebtExpenses
                });

                if (!isRetired)
                    salary *= (1m + salaryRaiseRate - inflationRate);
            }
            
            LogSuccessfullyCompleted(logger, request.ScenarioId);

            return new GetCashflowResult
            {
                Success = true,
                Data = new CashflowSankeyDto { Nodes = nodes, Links = links },
                YearlyCashFlows = yearlyCashFlows
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new GetCashflowResult
            {
                Success = false,
                ErrorMessage = "An error occurred while loading cashflow data. Please try again later."
            };
        }
    }

    private static decimal ToAnnual(decimal? amount, int frequencyId, List<Frequency> frequencies)
    {
        if (amount is null or <= 0) return 0m;
        var freq = frequencies.FirstOrDefault(f => f.Id == frequencyId);
        return amount.Value * (freq?.FrequencyPerYear ?? 1);
    }

    private static int CalculateAge(DateOnly birthDate)
    {
        return (int)(DateTime.Today - birthDate.ToDateTime(TimeOnly.MinValue)).TotalDays / 365;
    }

    [LoggerMessage(LogLevel.Information, "Starting GetCashflow handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetCashflowHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully built cashflow Sankey for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetCashflowHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting cashflow for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetCashflowHandler> logger, string Exception, long ScenarioId);
}


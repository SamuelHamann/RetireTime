using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Interfaces.Repositories;
using RetirementTime.Domain.Interfaces.Services;

namespace RetirementTime.Application.Features.Dashboard.Cashflow.GetCashflow;

public partial class GetCashflowHandler(
    IEmploymentIncomeRepository employmentIncomeRepository,
    ISelfEmploymentIncomeRepository selfEmploymentIncomeRepository,
    IPensionDefinedBenefitsRepository pensionDefinedBenefitsRepository,
    IOtherIncomeOrBenefitsRepository otherIncomeOrBenefitsRepository,
    ISpendingRepository spendingRepository,
    IRetirementTimelineRepository timelineRepository,
    IOnboardingPersonalInfoRepository onboardingPersonalInfoRepository,
    IOnboardingEmploymentRepository onboardingEmploymentRepository,
    IDashboardScenarioRepository scenarioRepository,
    IDashboardAssumptionsRepository assumptionsRepository,
    IGenericDebtRepository debtRepository,
    ICashflowCalculationService cashflowCalculationService,
    ILogger<GetCashflowHandler> logger) : IRequestHandler<GetCashflowQuery, GetCashflowResult>
{
    public async Task<GetCashflowResult> Handle(GetCashflowQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            // ── 1. Scenario → UserId ──────────────────────────────────────────
            var scenario = await scenarioRepository.GetByIdAsync(request.ScenarioId);
            if (scenario is null)
                return new GetCashflowResult { Success = false, ErrorMessage = "Scenario not found." };

            var userId = scenario.UserId;

            // ── 2. Age / Retirement Age / Life Expectancy ─────────────────────
            var personalInfo = await onboardingPersonalInfoRepository.GetByUserId(userId);
            var employment   = await onboardingEmploymentRepository.GetByUserId(userId);
            var assumptions  = await assumptionsRepository.GetByScenarioIdAsync(request.ScenarioId);

            var today        = DateOnly.FromDateTime(DateTime.UtcNow);
            var currentAge   = personalInfo is not null
                ? CalculateAge(personalInfo.DateOfBirth, today)
                : (int?)null;

            var retirementAge  = employment?.PlannedRetirementAge;
            var lifeExpectancy = assumptions?.LifeExpectancy;

            // ── 3. All timelines for this scenario ────────────────────────────
            var allTimelines = await timelineRepository.GetByScenarioIdAsync(request.ScenarioId);
            var incomeTimelines  = allTimelines.Where(t => t.TimelineType == RetirementTimelineTypeEnum.Income).ToList();
            var expenseTimelines = allTimelines.Where(t => t.TimelineType == RetirementTimelineTypeEnum.Expenses).ToList();

            // ── 4. Frequencies (needed for ToAnnual conversions later) ────────
            var frequencies = await spendingRepository.GetFrequenciesAsync();

            // ── 5. Incomes – per income timeline ──────────────────────────────
            var incomeTimelineData = new List<CashflowTimelineData>();
            foreach (var timeline in incomeTimelines)
            {
                incomeTimelineData.Add(new CashflowTimelineData
                {
                    Timeline             = timeline,
                    EmploymentIncomes    = await employmentIncomeRepository.GetByScenarioIdAsync(request.ScenarioId, timeline.Id),
                    SelfEmploymentIncomes = await selfEmploymentIncomeRepository.GetByScenarioIdAsync(request.ScenarioId, timeline.Id),
                    PensionDefinedBenefits = await pensionDefinedBenefitsRepository.GetByScenarioIdAsync(request.ScenarioId, timeline.Id),
                    OtherIncomes         = await otherIncomeOrBenefitsRepository.GetByScenarioIdAsync(request.ScenarioId, timeline.Id),
                });
            }

            // ── 6. Expenses – per expense timeline ────────────────────────────
            var expenseTimelineData = new List<CashflowTimelineData>();
            foreach (var timeline in expenseTimelines)
            {
                expenseTimelineData.Add(new CashflowTimelineData
                {
                    Timeline              = timeline,
                    LivingExpenses        = await spendingRepository.GetLivingExpensesAsync(request.ScenarioId, timeline.Id),
                    DiscretionaryExpenses = await spendingRepository.GetDiscretionaryExpensesAsync(request.ScenarioId, timeline.Id),
                    DebtRepayments        = await spendingRepository.GetDebtRepaymentsAsync(request.ScenarioId, timeline.Id),
                    AssetsExpenses        = await spendingRepository.GetAssetsExpensesAsync(request.ScenarioId, timeline.Id),
                    OtherExpenses         = await spendingRepository.GetOtherExpensesAsync(request.ScenarioId, timeline.Id),
                    InvestmentExpenses    = await spendingRepository.GetInvestmentExpensesAsync(request.ScenarioId, timeline.Id),
                });
            }

            // ── 7. Debts linked to spending repayments ────────────────────────
            var debts = await debtRepository.GetAllByScenarioIdAsync(request.ScenarioId);

            // ── 8. Delegate calculations to the Domain service ────────────────
            var timelineTotals = cashflowCalculationService.CalculateTimelineTotals(
                incomeTimelineData,
                expenseTimelineData,
                frequencies,
                assumptions!,
                debts,
                currentAge ?? 0,
                retirementAge ?? 0,
                lifeExpectancy ?? 0);

            LogSuccessfullyCompleted(logger, request.ScenarioId);

            // Calculations done – Sankey/chart mapping will be done in subsequent steps.
            return new GetCashflowResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new GetCashflowResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    private static int CalculateAge(DateOnly dateOfBirth, DateOnly today)
    {
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age)) age--;
        return age;
    }


    [LoggerMessage(LogLevel.Information, "Starting GetCashflow handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetCashflowHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully built cashflow Sankey for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetCashflowHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting cashflow for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetCashflowHandler> logger, string Exception, long ScenarioId);
}

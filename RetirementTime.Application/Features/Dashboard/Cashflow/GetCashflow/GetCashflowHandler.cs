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
        return new GetCashflowResult();
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

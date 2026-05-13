using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Helpers;

public static class FinancialMathHelper
{
    /// <summary>
    /// Calculates the net annual OAS benefit for a given recipient, applying residency
    /// proportion, deferral enhancement, the age-75+ top-up rate, and the income-based
    /// clawback (recovery tax).
    /// </summary>
    /// <remarks>
    /// <para><b>Residency proportion:</b> A full pension requires <see cref="OasConstants.FullResidencyYears"/>
    /// years of Canadian residency after age 18. Fewer years yield
    /// <c>yearsInCanada / FullResidencyYears</c> of the maximum. A minimum of
    /// <see cref="OasConstants.MinResidencyYears"/> years is required to receive any benefit.</para>
    /// <para><b>Deferral enhancement:</b> Starting OAS after <see cref="OasConstants.StandardStartAge"/>
    /// increases the monthly amount by <see cref="OasConstants.DeferralRatePerMonth"/> per month of
    /// delay, up to a maximum of 36% at <see cref="OasConstants.MaxDeferralAge"/>. The start age is
    /// clamped to [StandardStartAge, MaxDeferralAge] for this calculation.</para>
    /// <para><b>Age-75+ rate:</b> Recipients aged 75 or older receive
    /// <see cref="OasConstants.MonthlyRate75Plus"/> instead of
    /// <see cref="OasConstants.MonthlyRate65To74"/>.</para>
    /// <para><b>Clawback:</b> If net world income exceeds <see cref="OasConstants.ClawbackThreshold"/>,
    /// 15% of the excess is repaid. The benefit is fully eliminated at
    /// <see cref="OasConstants.EliminationThreshold65To74"/> (current age 65–74) or
    /// <see cref="OasConstants.EliminationThreshold75Plus"/> (current age 75+). Note that
    /// the elimination threshold is based on the recipient's <b>current age</b> in the tax
    /// year, not the age at which they started collecting OAS.</para>
    /// </remarks>
    /// <param name="oasConstants">Current government-published OAS rates and thresholds.</param>
    /// <param name="yearsInCanadaAfter18">Number of years the recipient resided in Canada
    /// after their 18th birthday.</param>
    /// <param name="oasStartAge">The age at which the recipient begins collecting OAS.</param>
    /// <param name="currentAge">The recipient's current age in the tax year, used to select
    /// the correct clawback elimination threshold (65–74 vs 75+).</param>
    /// <param name="netWorldIncome">The recipient's annual net world income used to
    /// determine whether the clawback applies.</param>
    /// <returns>An <see cref="OasResult"/> containing the gross annual benefit, annual
    /// clawback amount, and net annual benefit after clawback.</returns>
    public static OasResult CalculateOasAnnualBenefit(
        OasConstants oasConstants,
        int          yearsInCanadaAfter18,
        int          oasStartAge,
        int          currentAge,
        decimal      netWorldIncome)
    {
        // Must meet minimum residency requirement
        if (yearsInCanadaAfter18 < oasConstants.MinResidencyYears)
            return new OasResult(0m, 0m, 0m);

        // ── 1. Residency proportion ───────────────────────────────────────────
        var residencyYears      = Math.Min(yearsInCanadaAfter18, oasConstants.FullResidencyYears);
        var residencyProportion = residencyYears / (decimal)oasConstants.FullResidencyYears;

        // ── 2. Base monthly rate (age-75+ top-up if applicable) ───────────────
        var baseMonthly = oasStartAge >= 75
            ? oasConstants.MonthlyRate75Plus
            : oasConstants.MonthlyRate65To74;

        // ── 3. Deferral enhancement (e.g. 0.6% per month deferred past 65, max 36% at 70) ──
        var clampedStartAge    = Math.Clamp(oasStartAge, oasConstants.StandardStartAge, oasConstants.MaxDeferralAge);
        var monthsDeferred     = (clampedStartAge - oasConstants.StandardStartAge) * 12;
        var deferralMultiplier = 1m + monthsDeferred * oasConstants.DeferralRatePerMonth;

        // ── 4. Gross annual benefit ───────────────────────────────────────────
        var grossMonthly = baseMonthly * residencyProportion * deferralMultiplier;
        var grossAnnual  = grossMonthly * 12m;

        // ── 5. Clawback (recovery tax) ────────────────────────────────────────
        // The elimination threshold is determined by the recipient's *current* age,
        // not their OAS start age, as CRA bases the threshold on age in the tax year.
        var eliminationThreshold = currentAge >= 75
            ? oasConstants.EliminationThreshold75Plus
            : oasConstants.EliminationThreshold65To74;

        decimal clawback;
        if (netWorldIncome >= eliminationThreshold)
        {
            clawback = grossAnnual;
        }
        else if (netWorldIncome > oasConstants.ClawbackThreshold)
        {
            var excess = netWorldIncome - oasConstants.ClawbackThreshold;
            clawback   = Math.Min(excess * 0.15m, grossAnnual);
        }
        else
        {
            clawback = 0m;
        }

        return new OasResult(grossAnnual, clawback, grossAnnual - clawback);
    }


    /// <param name="nominalRate">Nominal rate of return (e.g. 0.07 for 7%)</param>
    /// <param name="inflationRate">Inflation rate (e.g. 0.02 for 2%)</param>
    /// <returns>Real rate of return</returns>
    public static decimal RealRateOfReturn(decimal nominalRate, decimal inflationRate)
    {
        return (1 + nominalRate) / (1 + inflationRate) - 1;
    }

    /// <summary>
    /// Calculates the future value using compound interest, with optional periodic contributions.
    /// Formula: FV = P(1 + r/n)^(n*t) + C * [((1 + r/n)^(n*t) - 1) / (r/n)]
    /// </summary>
    /// <param name="principal">Initial principal amount</param>
    /// <param name="annualRate">Annual rate of return (e.g. 0.07 for 7%)</param>
    /// <param name="compoundingPeriodsPerYear">Number of compounding periods per year (e.g. 12 for monthly)</param>
    /// <param name="years">Number of years</param>
    /// <param name="contributionAmount">Optional periodic contribution amount (default 0)</param>
    /// <param name="contributionsPerYear">Optional number of contributions per year (default 0, meaning no contributions)</param>
    /// <returns>Future value after compound growth and contributions</returns>
    public static decimal CompoundInterest(
        decimal principal,
        decimal annualRate,
        int compoundingPeriodsPerYear,
        int years,
        decimal contributionAmount = 0,
        int contributionsPerYear = 0)
    {
        if (compoundingPeriodsPerYear <= 0) compoundingPeriodsPerYear = 1;

        var n = (decimal)compoundingPeriodsPerYear;
        var t = (decimal)years;
        var r = annualRate;

        var growthFactor = (decimal)Math.Pow((double)(1 + r / n), (double)(n * t));
        var fvPrincipal  = principal * growthFactor;

        var fvContributions = 0m;
        if (contributionAmount != 0 && contributionsPerYear > 0)
        {
            var nc = (decimal)contributionsPerYear;
            var contributionGrowthFactor = (decimal)Math.Pow((double)(1 + r / nc), (double)(nc * t));

            fvContributions = r == 0
                ? contributionAmount * nc * t
                : contributionAmount * ((contributionGrowthFactor - 1) / (r / nc));
        }

        return fvPrincipal + fvContributions;
    }
}

/// <summary>
/// Holds the result of an OAS benefit calculation.
/// </summary>
/// <param name="GrossAnnual">Annual OAS benefit before the clawback is applied.</param>
/// <param name="AnnualClawback">Amount repaid via the OAS recovery tax (15% of income
/// above the threshold, capped at the gross benefit).</param>
/// <param name="NetAnnual">Net annual benefit the recipient actually receives after
/// the clawback.</param>
public record OasResult(decimal GrossAnnual, decimal AnnualClawback, decimal NetAnnual);

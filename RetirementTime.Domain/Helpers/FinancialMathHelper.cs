namespace RetirementTime.Domain.Helpers;

public static class FinancialMathHelper
{
    /// <summary>
    /// Calculates the real rate of return, adjusting a nominal rate for inflation
    /// using the Fisher equation: (1 + r) / (1 + i) - 1
    /// </summary>
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

        // Future value of the principal
        var growthFactor = (decimal)Math.Pow((double)(1 + r / n), (double)(n * t));
        var fvPrincipal  = principal * growthFactor;

        // Future value of contributions (annuity formula)
        var fvContributions = 0m;
        if (contributionAmount != 0 && contributionsPerYear > 0)
        {
            var nc = (decimal)contributionsPerYear;
            var contributionGrowthFactor = (decimal)Math.Pow((double)(1 + r / nc), (double)(nc * t));

            // If rate is effectively zero, contributions simply accumulate
            fvContributions = r == 0
                ? contributionAmount * nc * t
                : contributionAmount * ((contributionGrowthFactor - 1) / (r / nc));
        }

        return fvPrincipal + fvContributions;
    }
}



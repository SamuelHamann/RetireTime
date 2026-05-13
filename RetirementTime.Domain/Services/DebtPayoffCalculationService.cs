using RetirementTime.Domain.Helpers;
using RetirementTime.Domain.Interfaces.Services;

namespace RetirementTime.Domain.Services;

/// <inheritdoc cref="IDebtPayoffCalculationService"/>
public class DebtPayoffCalculationService : IDebtPayoffCalculationService
{
    /// <inheritdoc/>
    public List<DebtYearlyBalance> CalculateYearlyBalances(
        decimal principal,
        decimal annualInterestRate,
        IEnumerable<DebtPaymentSegment> paymentSegments)
    {
        var result   = new List<DebtYearlyBalance>();
        if (principal <= 0) return result;

        var segments = paymentSegments.ToList();
        if (segments.Count == 0) return result;

        var rate    = annualInterestRate / 100m;
        var balance = principal;

        foreach (var segment in segments.OrderBy(s => s.AgeFrom))
        {
            // r = monthly rate, P = monthly payment equivalent
            var rMonthly       = rate / 12m;
            var monthlyPayment = segment.AnnualPayment / 12m;

            // A resets to the balance carried in from the previous segment
            var segmentPrincipal = balance;
            var paymentsMade     = 0;

            for (var age = segment.AgeFrom; age < segment.AgeTo; age++)
            {
                // Advance 12 monthly payments within this segment
                paymentsMade += 12;

                balance = FinancialMathHelper.RemainingLoanBalance(
                    originalPrincipal: segmentPrincipal,
                    ratePerPeriod:     rMonthly,
                    paymentsMade:      paymentsMade,
                    periodicPayment:   monthlyPayment);

                result.Add(new DebtYearlyBalance(age, balance));

                if (balance <= 0) return result;
            }
        }

        return result;
    }
}

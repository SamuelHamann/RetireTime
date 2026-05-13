using RetirementTime.Domain.Services;

namespace RetirementTime.Domain.Interfaces.Services;

/// <summary>
/// Calculates how a debt balance evolves year-by-year across multiple payment segments.
/// </summary>
public interface IDebtPayoffCalculationService
{
    /// <summary>
    /// Returns the yearly balance schedule for a debt across all timeline segments.
    /// Each entry represents one year: interest is compounded first, then the annual
    /// payment is deducted. Once the balance reaches zero the list stops.
    /// An empty list is returned when principal is zero or no segments are provided.
    /// </summary>
    List<DebtYearlyBalance> CalculateYearlyBalances(
        decimal principal,
        decimal annualInterestRate,
        IEnumerable<DebtPaymentSegment> paymentSegments);
}

namespace RetirementTime.Domain.Services;

/// <summary>
/// Represents a timeline segment during which a fixed annual payment is made toward a debt.
/// </summary>
/// <param name="AgeFrom">The age at which this segment starts.</param>
/// <param name="AgeTo">The age at which this segment ends (exclusive).</param>
/// <param name="AnnualPayment">The total amount paid toward the debt per year in this segment.</param>
public record DebtPaymentSegment(int AgeFrom, int AgeTo, decimal AnnualPayment);

/// <summary>The remaining debt balance at the end of a given age year.</summary>
/// <param name="Age">The age year that has just completed.</param>
/// <param name="RemainingBalance">Remaining balance after interest and payment for that year (floored at zero).</param>
public record DebtYearlyBalance(int Age, decimal RemainingBalance);

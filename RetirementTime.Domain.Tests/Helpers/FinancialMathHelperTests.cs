using RetirementTime.Domain.Helpers;

namespace RetirementTime.Domain.Tests.Helpers;

[TestFixture]
public class FinancialMathHelperTests
{
    // ── Guard clauses ────────────────────────────────────────────────────────

    [Test]
    public void RemainingLoanBalance_ZeroPrincipal_ReturnsZero()
    {
        var result = FinancialMathHelper.RemainingLoanBalance(0m, 0.005m, 10, 500m);

        Assert.That(result, Is.EqualTo(0m));
    }

    [Test]
    public void RemainingLoanBalance_NegativePrincipal_ReturnsZero()
    {
        var result = FinancialMathHelper.RemainingLoanBalance(-1_000m, 0.005m, 10, 500m);

        Assert.That(result, Is.EqualTo(0m));
    }

    [Test]
    public void RemainingLoanBalance_ZeroPaymentsMade_ReturnsOriginalPrincipal()
    {
        var result = FinancialMathHelper.RemainingLoanBalance(10_000m, 0.005m, 0, 500m);

        Assert.That(result, Is.EqualTo(10_000m));
    }

    [Test]
    public void RemainingLoanBalance_NegativePaymentsMade_ReturnsOriginalPrincipal()
    {
        var result = FinancialMathHelper.RemainingLoanBalance(10_000m, 0.005m, -5, 500m);

        Assert.That(result, Is.EqualTo(10_000m));
    }

    // ── Zero interest rate ───────────────────────────────────────────────────

    [Test]
    public void RemainingLoanBalance_ZeroRate_SubtractsPaymentsFromPrincipal()
    {
        // 10 000 – 12 × 500 = 4 000
        var result = FinancialMathHelper.RemainingLoanBalance(10_000m, 0m, 12, 500m);

        Assert.That(result, Is.EqualTo(4_000m));
    }

    [Test]
    public void RemainingLoanBalance_ZeroRate_ExactPayoff_ReturnsZero()
    {
        // 6 000 – 12 × 500 = 0
        var result = FinancialMathHelper.RemainingLoanBalance(6_000m, 0m, 12, 500m);

        Assert.That(result, Is.EqualTo(0m));
    }

    [Test]
    public void RemainingLoanBalance_ZeroRate_OverPayment_FlooredAtZero()
    {
        // 1 000 – 24 × 500 would be negative → floor to 0
        var result = FinancialMathHelper.RemainingLoanBalance(1_000m, 0m, 24, 500m);

        Assert.That(result, Is.EqualTo(0m));
    }

    [Test]
    public void RemainingLoanBalance_ZeroRate_LargePaymentsMade_ReturnsZero()
    {
        var result = FinancialMathHelper.RemainingLoanBalance(5_000m, 0m, 10_000, 500m);

        Assert.That(result, Is.EqualTo(0m));
    }

    [Test]
    public void RemainingLoanBalance_ZeroRate_ZeroPeriodicPayment_ReturnsPrincipalUnchanged()
    {
        var result = FinancialMathHelper.RemainingLoanBalance(10_000m, 0m, 12, 0m);

        Assert.That(result, Is.EqualTo(10_000m));
    }

    // ── Non-zero interest rate ───────────────────────────────────────────────

    [Test]
    public void RemainingLoanBalance_StandardAmortization_MatchesFormula()
    {
        // 12% annual / 12 = 1% per month; 12 payments of ~888.49 on 10 000 principal
        // After 12 payments the balance should be approximately 5 654 (halfway on ~24-month loan)
        // Use the exact PMT for a 24-month loan so balance is predictable.
        // PMT = P * r / (1 - (1+r)^-n) with P=10000, r=0.01, n=24
        // PMT ≈ 470.73
        const decimal principal = 10_000m;
        const decimal ratePerPeriod = 0.01m;   // 1% / month
        const decimal payment = 470.73m;        // approximate monthly PMT for 24-month loan

        var result = FinancialMathHelper.RemainingLoanBalance(principal, ratePerPeriod, 12, payment);

        // After 12 payments on a ~24-month amortization the balance should be roughly 5 100–5 400
        Assert.That(result, Is.InRange(5_000m, 5_500m));
    }

    [Test]
    public void RemainingLoanBalance_FullAmortization_ReturnsZeroAfterAllPayments()
    {
        // P = 1 000, r = 0.01/month, n = 12 payments
        // PMT = P * r / (1 - (1+r)^-n) = 1000 * 0.01 / (1 - 1.01^-12) ≈ 88.85
        const decimal principal = 1_000m;
        const decimal ratePerPeriod = 0.01m;
        const decimal payment = 88.85m;   // approximation of exact PMT

        var result = FinancialMathHelper.RemainingLoanBalance(principal, ratePerPeriod, 12, payment);

        // Should be very close to zero (within rounding tolerance of approximate PMT)
        Assert.That(result, Is.InRange(0m, 2m));
    }

    [Test]
    public void RemainingLoanBalance_PaymentSmallerThanInterestOnly_BalanceGrows()
    {
        // Interest-only on 10 000 at 1%/month = 100/month
        // Paying only 50/month means balance grows
        const decimal principal = 10_000m;
        const decimal ratePerPeriod = 0.01m;
        const decimal payment = 50m;   // less than 100 interest-only

        var result = FinancialMathHelper.RemainingLoanBalance(principal, ratePerPeriod, 12, payment);

        Assert.That(result, Is.GreaterThan(principal));
    }

    [Test]
    public void RemainingLoanBalance_LargePaymentsMade_FlooredAtZero()
    {
        // After far more payments than needed the balance should be clamped to zero
        const decimal principal = 5_000m;
        const decimal ratePerPeriod = 0.005m;   // 0.5% / month
        const decimal payment = 500m;

        var result = FinancialMathHelper.RemainingLoanBalance(principal, ratePerPeriod, 10_000, payment);

        Assert.That(result, Is.EqualTo(0m));
    }

    [Test]
    public void RemainingLoanBalance_OnePayment_ReducesPrincipalByPaymentMinusInterest()
    {
        // After 1 month: balance = 10 000 * 1.01 - 200 = 10 100 - 200 = 9 900
        const decimal principal = 10_000m;
        const decimal ratePerPeriod = 0.01m;
        const decimal payment = 200m;
        const decimal expected = 9_900m;

        var result = FinancialMathHelper.RemainingLoanBalance(principal, ratePerPeriod, 1, payment);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void RemainingLoanBalance_ZeroPeriodicPayment_GrowsByInterestOnly()
    {
        // With no payments, balance compounds: 10 000 * 1.01^12 ≈ 11 268.25
        const decimal principal = 10_000m;
        const decimal ratePerPeriod = 0.01m;
        var expected = principal * (decimal)Math.Pow(1.01, 12);

        var result = FinancialMathHelper.RemainingLoanBalance(principal, ratePerPeriod, 12, 0m);

        Assert.That(result, Is.EqualTo(expected).Within(0.01m));
    }
}

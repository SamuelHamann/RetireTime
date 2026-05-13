using RetirementTime.Domain.Services;

namespace RetirementTime.Domain.Tests.Services;

[TestFixture]
public class DebtPayoffCalculationServiceTests
{
    private DebtPayoffCalculationService _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _sut = new DebtPayoffCalculationService();
    }

    // ── Guard clauses ────────────────────────────────────────────────────────

    [Test]
    public void CalculateYearlyBalances_ZeroPrincipal_ReturnsEmptyList()
    {
        var segments = new[] { new DebtPaymentSegment(30, 35, 1_200m) };

        var result = _sut.CalculateYearlyBalances(0m, 5m, segments);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void CalculateYearlyBalances_NegativePrincipal_ReturnsEmptyList()
    {
        var segments = new[] { new DebtPaymentSegment(30, 35, 1_200m) };

        var result = _sut.CalculateYearlyBalances(-1_000m, 5m, segments);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void CalculateYearlyBalances_NoSegments_ReturnsEmptyList()
    {
        var result = _sut.CalculateYearlyBalances(10_000m, 5m, []);

        Assert.That(result, Is.Empty);
    }

    // ── Zero-interest, single segment ───────────────────────────────────────

    [Test]
    public void CalculateYearlyBalances_ZeroRate_SingleSegment_ProducesOneEntryPerYear()
    {
        // 1 200 principal, 240/year → pays off in exactly 5 years (ages 30-34)
        var segments = new[] { new DebtPaymentSegment(30, 36, 240m) };

        var result = _sut.CalculateYearlyBalances(1_200m, 0m, segments);

        // Should exit early when balance hits 0 at age 34 (5 entries total)
        Assert.That(result, Has.Count.EqualTo(5));
    }

    [Test]
    public void CalculateYearlyBalances_ZeroRate_SingleSegment_BalancesDecreaseByAnnualPayment()
    {
        // principal = 1 200, annualPayment = 240, rate = 0
        // End-of-year balances: 960, 720, 480, 240, 0
        var segments = new[] { new DebtPaymentSegment(30, 36, 240m) };

        var result = _sut.CalculateYearlyBalances(1_200m, 0m, segments);

        Assert.Multiple(() =>
        {
            Assert.That(result[0].Age,              Is.EqualTo(30));
            Assert.That(result[0].RemainingBalance, Is.EqualTo(960m));
            Assert.That(result[1].Age,              Is.EqualTo(31));
            Assert.That(result[1].RemainingBalance, Is.EqualTo(720m));
            Assert.That(result[2].Age,              Is.EqualTo(32));
            Assert.That(result[2].RemainingBalance, Is.EqualTo(480m));
            Assert.That(result[3].Age,              Is.EqualTo(33));
            Assert.That(result[3].RemainingBalance, Is.EqualTo(240m));
            Assert.That(result[4].Age,              Is.EqualTo(34));
            Assert.That(result[4].RemainingBalance, Is.EqualTo(0m));
        });
    }

    [Test]
    public void CalculateYearlyBalances_ZeroRate_ExitsEarlyWhenBalanceReachesZero()
    {
        // principal = 600, annualPayment = 300, segment covers ages 30-40
        // Debt paid off at age 31 → only 2 entries should be returned
        var segments = new[] { new DebtPaymentSegment(30, 40, 300m) };

        var result = _sut.CalculateYearlyBalances(600m, 0m, segments);

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].RemainingBalance, Is.EqualTo(300m));
            Assert.That(result[1].RemainingBalance, Is.EqualTo(0m));
        });
    }

    [Test]
    public void CalculateYearlyBalances_ZeroRate_SegmentEndsBeforePayoff_AllAgesRecorded()
    {
        // principal = 10 000, annualPayment = 1 000, segment covers only 3 years → never hits 0
        var segments = new[] { new DebtPaymentSegment(30, 33, 1_000m) };

        var result = _sut.CalculateYearlyBalances(10_000m, 0m, segments);

        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[2].Age,              Is.EqualTo(32));
            Assert.That(result[2].RemainingBalance, Is.EqualTo(7_000m).Within(0.01m));
        });
    }

    // ── Zero-width segment ───────────────────────────────────────────────────

    [Test]
    public void CalculateYearlyBalances_ZeroWidthSegment_ProducesNoEntries()
    {
        // AgeFrom == AgeTo → the for-loop body never executes
        var segments = new[] { new DebtPaymentSegment(30, 30, 1_200m) };

        var result = _sut.CalculateYearlyBalances(5_000m, 5m, segments);

        Assert.That(result, Is.Empty);
    }

    // ── Non-zero interest rate, single segment ───────────────────────────────

    [Test]
    public void CalculateYearlyBalances_NonZeroRate_BalanceHigherThanZeroRateEquivalent()
    {
        // With interest the remaining balance should always be higher than without
        var segments = new[] { new DebtPaymentSegment(30, 35, 1_200m) };

        var withInterest    = _sut.CalculateYearlyBalances(5_000m, 12m, segments);
        var withoutInterest = _sut.CalculateYearlyBalances(5_000m,  0m, segments);

        Assert.That(withInterest[0].RemainingBalance,
                    Is.GreaterThan(withoutInterest[0].RemainingBalance));
    }

    [Test]
    public void CalculateYearlyBalances_NonZeroRate_OneYearBalance_MatchesFormula()
    {
        // principal = 12 000, annual rate = 12% → monthly rate = 1%
        // monthly payment = 200 (annual 2 400)
        // Expected end-of-year balance after 12 payments:
        //   B = 12000 * 1.01^12 - 200 * (1.01^12 - 1) / 0.01
        //     ≈ 13521.90 - 2536.50 ≈ 10985.40
        var segments = new[] { new DebtPaymentSegment(30, 31, 2_400m) };

        var result = _sut.CalculateYearlyBalances(12_000m, 12m, segments);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].RemainingBalance, Is.InRange(10_900m, 11_100m));
    }

    [Test]
    public void CalculateYearlyBalances_NonZeroRate_PaymentUnderInterestOnly_BalanceGrows()
    {
        // 12 000 at 12% → interest-only monthly = 120
        // Annual payment = 600 (monthly = 50) < 120 interest-only → balance grows
        var segments = new[] { new DebtPaymentSegment(30, 32, 600m) };

        var result = _sut.CalculateYearlyBalances(12_000m, 12m, segments);

        Assert.That(result[0].RemainingBalance, Is.GreaterThan(12_000m));
    }

    [Test]
    public void CalculateYearlyBalances_NonZeroRate_ExitsEarlyOnPayoff()
    {
        // Very large annual payment on a small principal → paid off well before segment ends
        var segments = new[] { new DebtPaymentSegment(30, 50, 5_000m) };

        var result = _sut.CalculateYearlyBalances(1_000m, 5m, segments);

        // Should stop as soon as balance hits 0, not produce 20 entries
        Assert.That(result.Count, Is.LessThan(20));
        Assert.That(result.Last().RemainingBalance, Is.EqualTo(0m));
    }

    // ── Multiple segments ────────────────────────────────────────────────────

    [Test]
    public void CalculateYearlyBalances_TwoSegments_BalanceCarriesForwardCorrectly()
    {
        // Segment 1: ages 30-32, annual payment 600 → monthly 50, rate 0
        //   age 30: 2400 - 600 = 1800; age 31: 2400 - 1200 = 1200  → carried forward = 1200
        // Segment 2: ages 32-34, annual payment 1200 → monthly 100, rate 0
        //   segmentPrincipal = 1200; age 32: 1200 - 1200 = 0 → stop
        var segments = new[]
        {
            new DebtPaymentSegment(30, 32,   600m),
            new DebtPaymentSegment(32, 34, 1_200m),
        };

        var result = _sut.CalculateYearlyBalances(2_400m, 0m, segments);

        Assert.Multiple(() =>
        {
            Assert.That(result[0], Is.EqualTo(new DebtYearlyBalance(30, 1_800m)));
            Assert.That(result[1], Is.EqualTo(new DebtYearlyBalance(31, 1_200m)));
            Assert.That(result[2], Is.EqualTo(new DebtYearlyBalance(32,      0m)));
            Assert.That(result, Has.Count.EqualTo(3));
        });
    }

    [Test]
    public void CalculateYearlyBalances_TwoSegments_FirstSegmentEndsWithPositiveBalance()
    {
        // Segment 1 makes small payments; segment 2 clears the debt
        // principal = 3 000, rate = 0
        // Seg 1 (30-32): annual = 600 → age 30 → 2400, age 31 → 1800
        // Seg 2 (32-35): annual = 1800 → age 32 → 0 (1800 - 1800 = 0) → stop
        var segments = new[]
        {
            new DebtPaymentSegment(30, 32,   600m),
            new DebtPaymentSegment(32, 35, 1_800m),
        };

        var result = _sut.CalculateYearlyBalances(3_000m, 0m, segments);

        Assert.Multiple(() =>
        {
            Assert.That(result[0].RemainingBalance, Is.EqualTo(2_400m));
            Assert.That(result[1].RemainingBalance, Is.EqualTo(1_800m));
            Assert.That(result[2].RemainingBalance, Is.EqualTo(0m));
        });
    }

    [Test]
    public void CalculateYearlyBalances_SegmentsOutOfOrder_AreProcessedByAgeAscending()
    {
        // Provide segments in reverse order; service should sort by AgeFrom
        // Same params as TwoSegments_BalanceCarriesForwardCorrectly above
        var segments = new[]
        {
            new DebtPaymentSegment(32, 34, 1_200m),   // second segment listed first
            new DebtPaymentSegment(30, 32,   600m),   // first segment listed second
        };

        var result = _sut.CalculateYearlyBalances(2_400m, 0m, segments);

        // If segments are sorted correctly, first entry is for age 30
        Assert.That(result[0].Age, Is.EqualTo(30));
    }

    [Test]
    public void CalculateYearlyBalances_TwoSegments_DebtNeverPaidOff_AllAgesPresent()
    {
        // Very small payments relative to principal — debt never reaches zero
        var segments = new[]
        {
            new DebtPaymentSegment(30, 35, 100m),
            new DebtPaymentSegment(35, 40, 100m),
        };

        var result = _sut.CalculateYearlyBalances(50_000m, 0m, segments);

        // 10 age-years total (30-34 and 35-39), none should have zero balance
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(10));
            Assert.That(result.All(b => b.RemainingBalance > 0), Is.True);
        });
    }

    // ── Age values stored correctly ──────────────────────────────────────────

    [Test]
    public void CalculateYearlyBalances_AgeValuesMatchLoopCounter()
    {
        // 5-year segment starting at age 40; verify age field is 40, 41, 42, 43, 44
        var segments = new[] { new DebtPaymentSegment(40, 45, 500m) };

        var result = _sut.CalculateYearlyBalances(50_000m, 0m, segments);

        var ages = result.Select(r => r.Age).ToList();
        Assert.That(ages, Is.EqualTo(new[] { 40, 41, 42, 43, 44 }));
    }
}

using System.Text.Json;
using RetirementTime.Domain.Entities.Dashboard;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Domain.Services;

namespace RetirementTime.Domain.Tests.Services;

[TestFixture]
public class NetWorthCalculationServiceTests
{
    private NetWorthCalculationService _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _sut = new NetWorthCalculationService();
    }

    // ── Baseline ────────────────────────────────────────────────────────────

    [Test]
    public void Calculate_AllInputsEmpty_ReturnsZeroTotalsAndEmptyJsonArrays()
    {
        var result = Calculate();

        Assert.Multiple(() =>
        {
            Assert.That(result.Asset, Is.EqualTo(0));
            Assert.That(result.Debt,  Is.EqualTo(0));
            Assert.That(result.Assets, Is.EqualTo("[]"));
            Assert.That(result.Debts,  Is.EqualTo("[]"));
        });
    }

    [Test]
    public void Calculate_AllInputsEmpty_SetsCorrectScenarioIdAndSnapshotDate()
    {
        var before = DateTime.UtcNow;
        var result = Calculate(scenarioId: 42);
        var after  = DateTime.UtcNow;

        Assert.Multiple(() =>
        {
            Assert.That(result.ScenarioId, Is.EqualTo(42));
            Assert.That(result.DateOfSnapShot, Is.InRange(before, after));
        });
    }

    // ── Home ────────────────────────────────────────────────────────────────

    [Test]
    public void Calculate_WithHome_IncludesHomeValueInTotalAssets()
    {
        var home = new AssetsHome { ScenarioId = 1, HomeValue = 500_000m };

        var result = Calculate(home: home);

        Assert.That(result.Asset, Is.EqualTo(500_000m));
    }

    [Test]
    public void Calculate_WithNullHome_DoesNotContributeToAssets()
    {
        var result = Calculate(home: null);

        Assert.That(result.Asset, Is.EqualTo(0));
    }

    [Test]
    public void Calculate_WithHomeNullValue_TreatsAsZero()
    {
        var home = new AssetsHome { ScenarioId = 1, HomeValue = null };

        var result = Calculate(home: home);

        Assert.That(result.Asset, Is.EqualTo(0));
    }

    [Test]
    public void Calculate_WithHome_SerializesOneAssetItemWithCorrectFields()
    {
        var home = new AssetsHome { ScenarioId = 1, HomeValue = 350_000m };

        var result   = Calculate(home: home);
        var items    = DeserializeAssets(result.Assets);

        Assert.That(items, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(items[0].AssetType, Is.EqualTo("Home"));
            Assert.That(items[0].Amount,    Is.EqualTo(350_000m));
        });
    }

    // ── Investment properties ────────────────────────────────────────────────

    [Test]
    public void Calculate_WithMultipleInvestmentProperties_SumsTotalCorrectly()
    {
        var properties = new[]
        {
            new AssetsInvestmentProperty { ScenarioId = 1, Name = "Cottage",   PropertyValue = 200_000m },
            new AssetsInvestmentProperty { ScenarioId = 1, Name = "Condo",     PropertyValue = 300_000m },
        };

        var result = Calculate(investmentProperties: properties);

        Assert.That(result.Asset, Is.EqualTo(500_000m));
    }

    [Test]
    public void Calculate_InvestmentPropertyWithNullValue_TreatsAsZero()
    {
        var properties = new[]
        {
            new AssetsInvestmentProperty { ScenarioId = 1, Name = "Vacant lot", PropertyValue = null }
        };

        var result = Calculate(investmentProperties: properties);

        Assert.That(result.Asset, Is.EqualTo(0));
    }

    [Test]
    public void Calculate_WithInvestmentProperties_SerializesCorrectAssetType()
    {
        var properties = new[]
        {
            new AssetsInvestmentProperty { ScenarioId = 1, Name = "Cottage", PropertyValue = 100_000m }
        };

        var result = Calculate(investmentProperties: properties);
        var items  = DeserializeAssets(result.Assets);

        Assert.That(items[0].AssetType, Is.EqualTo("Investment Property"));
    }

    // ── Investment accounts ──────────────────────────────────────────────────

    [Test]
    public void Calculate_AccountNotUsingHoldings_UsesCurrentTotalValue()
    {
        var accounts = new[]
        {
            new AssetsInvestmentAccount
            {
                ScenarioId        = 1,
                AccountName       = "TFSA",
                UseIndividualHoldings = false,
                CurrentTotalValue = 80_000m,
                Holdings          = []
            }
        };

        var result = Calculate(investmentAccounts: accounts);

        Assert.That(result.Asset, Is.EqualTo(80_000m));
    }

    [Test]
    public void Calculate_AccountUsingHoldings_SumsHoldingValues()
    {
        var accounts = new[]
        {
            new AssetsInvestmentAccount
            {
                ScenarioId            = 1,
                AccountName           = "RRSP",
                UseIndividualHoldings = true,
                CurrentTotalValue     = 999_999m,   // should be ignored
                Holdings =
                [
                    new AssetsHolding { CurrentValue = 10_000m },
                    new AssetsHolding { CurrentValue = 25_000m },
                ]
            }
        };

        var result = Calculate(investmentAccounts: accounts);

        Assert.That(result.Asset, Is.EqualTo(35_000m));
    }

    [Test]
    public void Calculate_AccountUsingHoldingsWithNullValues_TreatsNullHoldingsAsZero()
    {
        var accounts = new[]
        {
            new AssetsInvestmentAccount
            {
                ScenarioId            = 1,
                AccountName           = "LIRA",
                UseIndividualHoldings = true,
                Holdings =
                [
                    new AssetsHolding { CurrentValue = null },
                    new AssetsHolding { CurrentValue = 5_000m },
                ]
            }
        };

        var result = Calculate(investmentAccounts: accounts);

        Assert.That(result.Asset, Is.EqualTo(5_000m));
    }

    [Test]
    public void Calculate_AccountNotUsingHoldingsWithNullTotal_TreatsAsZero()
    {
        var accounts = new[]
        {
            new AssetsInvestmentAccount
            {
                ScenarioId            = 1,
                AccountName           = "RESP",
                UseIndividualHoldings = false,
                CurrentTotalValue     = null,
                Holdings              = []
            }
        };

        var result = Calculate(investmentAccounts: accounts);

        Assert.That(result.Asset, Is.EqualTo(0));
    }

    // ── Physical assets ──────────────────────────────────────────────────────

    [Test]
    public void Calculate_WithPhysicalAssets_IncludesEstimatedValueInTotal()
    {
        var assets = new[]
        {
            new AssetsPhysicalAsset { ScenarioId = 1, Name = "Car",   EstimatedValue = 15_000m },
            new AssetsPhysicalAsset { ScenarioId = 1, Name = "Boat",  EstimatedValue = 5_000m  },
        };

        var result = Calculate(physicalAssets: assets);

        Assert.That(result.Asset, Is.EqualTo(20_000m));
    }

    [Test]
    public void Calculate_PhysicalAssetWithNullValue_TreatsAsZero()
    {
        var assets = new[]
        {
            new AssetsPhysicalAsset { ScenarioId = 1, Name = "Jewelry", EstimatedValue = null }
        };

        var result = Calculate(physicalAssets: assets);

        Assert.That(result.Asset, Is.EqualTo(0));
    }

    // ── Debts ────────────────────────────────────────────────────────────────

    [Test]
    public void Calculate_WithMultipleDebts_SumsTotalDebtCorrectly()
    {
        var debts = new[]
        {
            BuildDebt("Car Loan",     "Car Loan",      10_000m),
            BuildDebt("Credit Card",  "Credit Card",   2_500m),
            BuildDebt("Student Loan", "Student Loan",  20_000m),
        };

        var result = Calculate(debts: debts);

        Assert.That(result.Debt, Is.EqualTo(32_500m));
    }

    [Test]
    public void Calculate_DebtWithNullBalance_TreatsAsZero()
    {
        var debts = new[] { BuildDebt("Personal Loan", "Personal Loan", null) };

        var result = Calculate(debts: debts);

        Assert.That(result.Debt, Is.EqualTo(0));
    }

    [Test]
    public void Calculate_DebtWithNullDebtType_UsesEmptyStringForDebtType()
    {
        var debt = new GenericDebt
        {
            ScenarioId = 1,
            Name       = "Mystery Debt",
            Balance    = 1_000m,
            DebtType   = null!
        };

        var result = Calculate(debts: [debt]);
        var items  = DeserializeDebts(result.Debts);

        Assert.That(items[0].DebtType, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Calculate_WithDebts_SerializesCorrectDebtItems()
    {
        var debts = new[] { BuildDebt("My Mortgage", "Mortgage", 400_000m) };

        var result = Calculate(debts: debts);
        var items  = DeserializeDebts(result.Debts);

        Assert.That(items, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(items[0].Name,     Is.EqualTo("My Mortgage"));
            Assert.That(items[0].DebtType, Is.EqualTo("Mortgage"));
            Assert.That(items[0].Amount,   Is.EqualTo(400_000m));
        });
    }

    // ── Combined ─────────────────────────────────────────────────────────────

    [Test]
    public void Calculate_MixedAssetsAndDebts_ComputesTotalsIndependently()
    {
        var home = new AssetsHome { ScenarioId = 1, HomeValue = 600_000m };
        var debts = new[]
        {
            BuildDebt("Mortgage", "Mortgage", 400_000m),
            BuildDebt("Car Loan", "Car Loan",  15_000m),
        };

        var result = Calculate(home: home, debts: debts);

        Assert.Multiple(() =>
        {
            Assert.That(result.Asset, Is.EqualTo(600_000m));
            Assert.That(result.Debt,  Is.EqualTo(415_000m));
        });
    }

    [Test]
    public void Calculate_AllAssetTypes_SumsAllContributions()
    {
        var home = new AssetsHome { ScenarioId = 1, HomeValue = 500_000m };

        var investmentProperties = new[]
        {
            new AssetsInvestmentProperty { ScenarioId = 1, Name = "Rental", PropertyValue = 300_000m }
        };

        var investmentAccounts = new[]
        {
            new AssetsInvestmentAccount
            {
                ScenarioId = 1, AccountName = "TFSA",
                UseIndividualHoldings = false,
                CurrentTotalValue = 100_000m,
                Holdings = []
            }
        };

        var physicalAssets = new[]
        {
            new AssetsPhysicalAsset { ScenarioId = 1, Name = "Car", EstimatedValue = 20_000m }
        };

        var result = Calculate(
            home: home,
            investmentProperties: investmentProperties,
            investmentAccounts: investmentAccounts,
            physicalAssets: physicalAssets);

        Assert.That(result.Asset, Is.EqualTo(920_000m));
    }

    [Test]
    public void Calculate_AllAssetTypes_SerializesAllAssetItems()
    {
        var home = new AssetsHome { ScenarioId = 1, HomeValue = 1m };
        var investmentProperties = new[] { new AssetsInvestmentProperty { ScenarioId = 1, Name = "P1", PropertyValue = 1m } };
        var investmentAccounts   = new[]
        {
            new AssetsInvestmentAccount
            {
                ScenarioId = 1, AccountName = "A1",
                UseIndividualHoldings = false,
                CurrentTotalValue = 1m,
                Holdings = []
            }
        };
        var physicalAssets = new[] { new AssetsPhysicalAsset { ScenarioId = 1, Name = "PA1", EstimatedValue = 1m } };

        var result = Calculate(
            home: home,
            investmentProperties: investmentProperties,
            investmentAccounts: investmentAccounts,
            physicalAssets: physicalAssets);

        var items = DeserializeAssets(result.Assets);
        Assert.That(items, Has.Count.EqualTo(4));
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private NetWorthHistory Calculate(
        long scenarioId = 1,
        AssetsHome? home = null,
        IEnumerable<AssetsInvestmentProperty>? investmentProperties = null,
        IEnumerable<AssetsInvestmentAccount>?  investmentAccounts   = null,
        IEnumerable<AssetsPhysicalAsset>?      physicalAssets       = null,
        IEnumerable<GenericDebt>?              debts                = null)
    {
        return _sut.Calculate(
            scenarioId,
            home,
            investmentProperties ?? [],
            investmentAccounts   ?? [],
            physicalAssets       ?? [],
            debts                ?? []);
    }

    private static GenericDebt BuildDebt(string name, string debtTypeName, decimal? balance) =>
        new()
        {
            ScenarioId = 1,
            Name       = name,
            Balance    = balance,
            DebtType   = new DebtType { Name = debtTypeName }
        };

    private static List<NetWorthAssetItem> DeserializeAssets(string json) =>
        JsonSerializer.Deserialize<List<NetWorthAssetItem>>(json)!;

    private static List<NetWorthDebtItem> DeserializeDebts(string json) =>
        JsonSerializer.Deserialize<List<NetWorthDebtItem>>(json)!;
}

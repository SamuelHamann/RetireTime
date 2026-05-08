using System.Text.Json;
using RetirementTime.Domain.Entities.Dashboard;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Domain.Interfaces.Services;

namespace RetirementTime.Domain.Services;

public class NetWorthCalculationService : INetWorthCalculationService
{
    /// <summary>
    /// Calculates a net worth snapshot for the given scenario by aggregating all asset
    /// and debt values into a <see cref="NetWorthHistory"/> record. Assets are broken down
    /// into: primary residence, investment properties, investment accounts (individual
    /// holdings or total value), and physical assets. Debts are taken directly from the
    /// provided <see cref="GenericDebt"/> list. The resulting record stores serialised
    /// JSON breakdowns alongside the total asset and total debt figures.
    /// </summary>
    public NetWorthHistory Calculate(
        long scenarioId,
        AssetsHome? home,
        IEnumerable<AssetsInvestmentProperty> investmentProperties,
        IEnumerable<AssetsInvestmentAccount> investmentAccounts,
        IEnumerable<AssetsPhysicalAsset> physicalAssets,
        IEnumerable<GenericDebt> debts)
    {
        var assetItems = new List<NetWorthAssetItem>();

        if (home is not null)
        {
            assetItems.Add(new NetWorthAssetItem
            {
                Name = "Primary Residence",
                AssetType = "Home",
                Amount = home.HomeValue
            });
        }

        foreach (var prop in investmentProperties)
        {
            assetItems.Add(new NetWorthAssetItem
            {
                Name = prop.Name,
                AssetType = "Investment Property",
                Amount = prop.PropertyValue
            });
        }

        foreach (var account in investmentAccounts)
        {
            var value = account.UseIndividualHoldings
                ? account.Holdings.Sum(h => h.CurrentValue ?? 0)
                : account.CurrentTotalValue;

            assetItems.Add(new NetWorthAssetItem
            {
                Name = account.AccountName,
                AssetType = "Investment Account",
                Amount = value
            });
        }

        foreach (var asset in physicalAssets)
        {
            assetItems.Add(new NetWorthAssetItem
            {
                Name = asset.Name,
                AssetType = "Physical Asset",
                Amount = asset.EstimatedValue
            });
        }

        var debtItems = debts.Select(d => new NetWorthDebtItem
        {
            Name = d.Name,
            DebtType = d.DebtType?.Name ?? string.Empty,
            Amount = d.Balance
        }).ToList();

        var totalAssets = assetItems.Sum(a => a.Amount ?? 0);
        var totalDebts = debtItems.Sum(d => d.Amount ?? 0);

        return new NetWorthHistory
        {
            ScenarioId = scenarioId,
            DateOfSnapShot = DateTime.UtcNow,
            Asset = totalAssets,
            Debt = totalDebts,
            Assets = JsonSerializer.Serialize(assetItems),
            Debts = JsonSerializer.Serialize(debtItems)
        };
    }
}

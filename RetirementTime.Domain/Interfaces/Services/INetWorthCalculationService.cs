using RetirementTime.Domain.Entities.Dashboard;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Entities.Dashboard.Debt;

namespace RetirementTime.Domain.Interfaces.Services;

public interface INetWorthCalculationService
{
    NetWorthHistory Calculate(
        long scenarioId,
        AssetsHome? home,
        IEnumerable<AssetsInvestmentProperty> investmentProperties,
        IEnumerable<AssetsInvestmentAccount> investmentAccounts,
        IEnumerable<AssetsPhysicalAsset> physicalAssets,
        IEnumerable<GenericDebt> debts);
}

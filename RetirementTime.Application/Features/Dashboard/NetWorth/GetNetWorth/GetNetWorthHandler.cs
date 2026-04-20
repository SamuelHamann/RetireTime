using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.NetWorth.GetNetWorth;

public partial class GetNetWorthHandler(
    IAssetsHomeRepository homeRepo,
    IAssetsInvestmentPropertyRepository investmentPropertyRepo,
    IAssetsInvestmentAccountRepository investmentAccountRepo,
    IAssetsPhysicalAssetRepository physicalAssetRepo,
    IGenericDebtRepository debtRepo,
    ILogger<GetNetWorthHandler> logger) : IRequestHandler<GetNetWorthQuery, GetNetWorthResult>
{
    public async Task<GetNetWorthResult> Handle(GetNetWorthQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var assets = new List<NetWorthAssetModel>();
            var debts  = new List<NetWorthDebtModel>();

            // Assets
            var home                 = await homeRepo.GetByScenarioIdAsync(request.ScenarioId);
            var investmentProperties = await investmentPropertyRepo.GetByScenarioIdAsync(request.ScenarioId);
            var investmentAccounts   = await investmentAccountRepo.GetByScenarioIdAsync(request.ScenarioId);
            var physicalAssets       = await physicalAssetRepo.GetByScenarioIdAsync(request.ScenarioId);

            if (home != null)
                assets.Add(new NetWorthAssetModel
                {
                    AssetId   = home.Id,
                    AssetType = "Home",
                    Name      = "Primary Residence",
                    Category  = "Real Estate",
                    Value     = home.HomeValue ?? 0m
                });

            foreach (var p in investmentProperties)
                assets.Add(new NetWorthAssetModel
                {
                    AssetId   = p.Id,
                    AssetType = "InvestmentProperty",
                    Name      = string.IsNullOrWhiteSpace(p.Name) ? "Investment Property" : p.Name,
                    Category  = "Real Estate",
                    Value     = p.PropertyValue ?? 0m
                });

            foreach (var acct in investmentAccounts)
            {
                var value = acct.UseIndividualHoldings
                    ? acct.Holdings.Sum(h => h.CurrentValue ?? 0m)
                    : acct.CurrentTotalValue ?? 0m;

                assets.Add(new NetWorthAssetModel
                {
                    AssetId   = acct.Id,
                    AssetType = "InvestmentAccount",
                    Name      = string.IsNullOrWhiteSpace(acct.AccountName) ? "Investment Account" : acct.AccountName,
                    Category  = "Investment Accounts",
                    Value     = value
                });
            }

            foreach (var pa in physicalAssets)
                assets.Add(new NetWorthAssetModel
                {
                    AssetId   = pa.Id,
                    AssetType = "PhysicalAsset",
                    Name      = string.IsNullOrWhiteSpace(pa.Name) ? "Physical Asset" : pa.Name,
                    Category  = "Physical Assets",
                    Value     = pa.EstimatedValue ?? 0m
                });

            // Debts
            // Asset IDs are scoped to their own table, so the same numeric ID can appear
            // in multiple asset tables. Build per-type sets and use the debt type to
            // resolve the correct asset table when linking debts to assets.
            var allDebts = await debtRepo.GetAllByScenarioIdAsync(request.ScenarioId);

            var homeAssetIds               = home != null ? new HashSet<long> { home.Id } : [];
            var investmentPropertyAssetIds = investmentProperties.Select(p => p.Id).ToHashSet();
            var investmentAccountAssetIds  = investmentAccounts.Select(a => a.Id).ToHashSet();
            var physicalAssetIds           = physicalAssets.Select(pa => pa.Id).ToHashSet();

            foreach (var debt in allDebts.Where(d => d.Balance is > 0))
            {
                string? linkedAssetType = null;

                if (debt.DebtAgainstAssetId.HasValue)
                {
                    var aid = debt.DebtAgainstAssetId.Value;
                    linkedAssetType = (DebtTypeEnum)debt.DebtTypeId switch
                    {
                        DebtTypeEnum.Mortgage when homeAssetIds.Contains(aid)               => "Home",
                        DebtTypeEnum.Mortgage when investmentPropertyAssetIds.Contains(aid) => "InvestmentProperty",
                        DebtTypeEnum.CarLoan  when physicalAssetIds.Contains(aid)           => "PhysicalAsset",
                        _                                                                    => null
                    };
                }

                debts.Add(new NetWorthDebtModel
                {
                    Name             = string.IsNullOrWhiteSpace(debt.Name) ? debt.DebtType?.Name ?? "Debt" : debt.Name,
                    Category         = debt.DebtType?.Name ?? "Other",
                    Balance          = debt.Balance ?? 0m,
                    AgainstAssetId   = debt.DebtAgainstAssetId,
                    AgainstAssetType = linkedAssetType
                });
            }

            LogSuccessfullyCompleted(logger, assets.Sum(a => a.Value), debts.Sum(d => d.Balance), request.ScenarioId);
            return new GetNetWorthResult { Success = true, Assets = assets, Debts = debts };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new GetNetWorthResult { Success = false, ErrorMessage = "An error occurred while calculating net worth. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetNetWorth handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<GetNetWorthHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Net worth calculated for ScenarioId: {ScenarioId} — Assets: {TotalAssets}, Debts: {TotalDebts}")]
    static partial void LogSuccessfullyCompleted(ILogger<GetNetWorthHandler> logger, decimal TotalAssets, decimal TotalDebts, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error occurred while calculating net worth for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetNetWorthHandler> logger, string Exception, long ScenarioId);
}

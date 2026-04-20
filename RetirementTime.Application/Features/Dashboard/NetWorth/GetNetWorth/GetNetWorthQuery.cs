using MediatR;

namespace RetirementTime.Application.Features.Dashboard.NetWorth.GetNetWorth;

public record GetNetWorthQuery : IRequest<GetNetWorthResult>
{
    public required long ScenarioId { get; init; }
}

public record GetNetWorthResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public decimal TotalAssets => Assets.Sum(a => a.Value);
    public decimal TotalDebts  => Debts.Sum(d => d.Balance);
    public decimal TotalNetWorth => TotalAssets - TotalDebts;
    public List<NetWorthAssetModel> Assets { get; init; } = [];
    public List<NetWorthDebtModel>  Debts  { get; init; } = [];
}

/// <summary>A single asset line in the net-worth snapshot.</summary>
public record NetWorthAssetModel
{
    /// <summary>Database ID of the underlying asset record.</summary>
    public long   AssetId   { get; init; }
    /// <summary>Discriminator so callers know which table the ID belongs to.</summary>
    public string AssetType { get; init; } = string.Empty; // "Home" | "InvestmentProperty" | "InvestmentAccount" | "PhysicalAsset"
    public string Name      { get; init; } = string.Empty;
    public string Category  { get; init; } = string.Empty;
    public decimal Value    { get; init; }
}

/// <summary>A single liability line in the net-worth snapshot.</summary>
public record NetWorthDebtModel
{
    public string  Name     { get; init; } = string.Empty;
    public string  Category { get; init; } = string.Empty;
    public decimal Balance  { get; init; }

    /// <summary>
    /// Matches <see cref="NetWorthAssetModel.AssetId"/> when this debt is secured
    /// against a specific asset (e.g. a mortgage against a property).
    /// Null when the debt is not linked to any particular asset.
    /// </summary>
    public long?   AgainstAssetId   { get; init; }

    /// <summary>Matches <see cref="NetWorthAssetModel.AssetType"/> for the linked asset.</summary>
    public string? AgainstAssetType { get; init; }
}


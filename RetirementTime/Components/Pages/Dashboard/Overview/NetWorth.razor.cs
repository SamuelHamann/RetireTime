using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.NetWorth.GetNetWorth;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.Overview;

public partial class NetWorth : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private GetNetWorthResult? _netWorth;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        _netWorth = await Mediator.Send(new GetNetWorthQuery { ScenarioId = ScenarioId });

        _isLoading = false;
        StateHasChanged();
    }

    private static string FormatCurrency(decimal value) =>
        value.ToString("C0", System.Globalization.CultureInfo.GetCultureInfo("en-CA"));

    private decimal GetCategoryTotal(string category) =>
        _netWorth?.Assets.Where(a => a.Category == category).Sum(a => a.Value) ?? 0m;

    private string GetCategoryAllocation(string category)
    {
        var total = _netWorth?.TotalAssets ?? 0m;
        if (total == 0m) return "0%";
        var pct = GetCategoryTotal(category) / total * 100m;
        return $"{pct:F0}% Allocation";
    }

    private string GetDebtRatio()
    {
        var total = _netWorth?.TotalAssets ?? 0m;
        if (total == 0m) return "0% Ratio";
        var pct = _netWorth!.TotalDebts / total * 100m;
        return $"{pct:F0}% Ratio";
    }

    private NetWorthDebtModel? GetDebtFor(long assetId, string assetType) =>
        _netWorth?.Debts.FirstOrDefault(d => d.AgainstAssetId == assetId && d.AgainstAssetType == assetType);

    private static readonly (string Month, int Pct)[] ChartBars =
    [
        ("JAN", 40), ("FEB", 45), ("MAR", 42), ("APR", 55), ("MAY", 65),
        ("JUN", 72), ("JUL", 85), ("AUG", 80), ("SEP", 88), ("OCT", 92),
        ("NOV", 95), ("DEC", 100)
    ];

    private static (string Icon, string BadgeClass) GetAssetIconInfo(string assetType) => assetType switch
    {
        "Home"               => ("home",             "nw-icon-badge--home"),
        "InvestmentProperty" => ("apartment",         "nw-icon-badge--property"),
        "InvestmentAccount"  => ("candlestick_chart", "nw-icon-badge--investment"),
        "PhysicalAsset"      => ("directions_car",    "nw-icon-badge--physical"),
        _                    => ("category",          "")
    };

    private static (string Icon, string BadgeClass) GetDebtIconInfo(string category) => category switch
    {
        "Mortgage"                          => ("home",                   "nw-icon-badge--debt"),
        "Car Loan"                          => ("directions_car",         "nw-icon-badge--debt"),
        "Student Loan"                      => ("school",                 "nw-icon-badge--debt"),
        "Credit Card"                       => ("credit_card",            "nw-icon-badge--debt"),
        "Medical"                           => ("medical_services",       "nw-icon-badge--debt"),
        "Personal Loan" or "Line of Credit" => ("payments",               "nw-icon-badge--debt"),
        _                                   => ("account_balance_wallet", "nw-icon-badge--debt")
    };
}

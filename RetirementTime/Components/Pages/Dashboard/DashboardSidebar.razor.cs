using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.CreateScenario;
using RetirementTime.Application.Features.Dashboard.GetScenarios;
using RetirementTime.Resources.Common;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard;

public partial class DashboardSidebar : ComponentBase, IDisposable
{
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private IStringLocalizer<CommonResources> Localizer { get; set; } = null!;
    [Inject] private IStringLocalizer<DashboardResources> DashboardLocalizer { get; set; } = null!;
    [Inject] private IMediator Mediator { get; set; } = null!;
    [Inject] private IncomeNavigationService IncomeNavService { get; set; } = null!;
    [Inject] private AssetNavigationService AssetNavService { get; set; } = null!;
    [Inject] private DebtNavigationService DebtNavService { get; set; } = null!;
    [Inject] private SpendingNavigationService SpendingNavService { get; set; } = null!;

    [Parameter] public bool HasCompletedIntro { get; set; }
    [Parameter] public long UserId { get; set; }
    [Parameter] public int RefreshTrigger { get; set; }

    // Scenario state
    private List<Application.Features.Dashboard.Common.ScenarioDto> _scenarios = [];
    private HashSet<long> _expandedScenarios = [];
    private HashSet<long> _expendedIncomeAssets = [];
    private HashSet<long> _expandedIncomeSubMenu = [];
    private HashSet<long> _expandedAssetsSubMenu = [];
    private HashSet<long> _expandedDebtSubMenu = [];
    private HashSet<long> _expandedSpendingSubMenu = [];
    private HashSet<long> _expandedScenarioOverview = [];
    private HashSet<long> _expandedPersistingIncomeSubMenu = [];
    private int _lastRefreshTrigger = -1;
    private bool _isCreatingScenario;

    // Active state based on current URLß
    private string _currentUrl = string.Empty;
    private long? _activeScenarioId;
    private string _activeView = "overview";
    private string _activeIncomeSubView = string.Empty;
    private string _activeAssetsSubView = string.Empty;
    private string _activeDebtSubView = string.Empty;
    private string _activeSpendingSubView = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadScenarios();
        Navigation.LocationChanged += OnLocationChanged;
        IncomeNavService.Register(NavigateNextIncomeStep);
        AssetNavService.Register(NavigateNextAssetStep);
        DebtNavService.Register(NavigateNextDebtStep);
        SpendingNavService.Register(NavigateNextSpendingStep);
        UpdateActiveStateFromUrl();
    }

    protected override async Task OnParametersSetAsync()
    {
        // Reload scenarios when the trigger changes
        if (RefreshTrigger != _lastRefreshTrigger)
        {
            _lastRefreshTrigger = RefreshTrigger;
            await LoadScenarios();
        }

        // Update active state when URL changes
        UpdateActiveStateFromUrl();
    }

    private void OnLocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
    {
        var previousUrl = _currentUrl;
        UpdateActiveStateFromUrl();

        // Reload scenarios if navigating away from a scenario page (e.g., after deletion)
        // or navigating to /home (overview)
        var previousPath = string.IsNullOrEmpty(previousUrl) ? "" : new Uri(previousUrl).PathAndQuery;
        var currentPath = Navigation.ToBaseRelativePath(Navigation.Uri);

        // If we were on a scenario page and now we're on /home, reload scenarios
        if (previousPath.Contains("scenario/") && !currentPath.Contains("scenario/"))
        {
            _ = ReloadScenariosAsync();
        }
        // Reload when leaving settings so a newly fully-created scenario is reflected
        else if (previousPath.Contains("/settings") && currentPath.StartsWith("scenario/") && !currentPath.Contains("settings"))
        {
            _ = ReloadScenariosAsync();
        }

        InvokeAsync(StateHasChanged);
    }

    private async Task ReloadScenariosAsync()
    {
        await LoadScenarios();
        await InvokeAsync(StateHasChanged);
    }

    private void UpdateActiveStateFromUrl()
    {
        _currentUrl = Navigation.Uri;
        var relativePath = Navigation.ToBaseRelativePath(_currentUrl);

        // Parse /scenario/{id}/{view}[/{subview}]
        if (relativePath.StartsWith("scenario/"))
        {
            var parts = relativePath.Split('/');
            if (parts.Length >= 3 && long.TryParse(parts[1], out var scenarioId))
            {
                _activeScenarioId = scenarioId;
                _activeView = parts[2]; // "settings", "income", "spending", etc.

                // Auto-expand the active scenario
                _expandedScenarios.Add(scenarioId);

                // Auto-expand the Financial Profile group
                HashSet<string> financialProfileViews = ["income", "spending", "assets", "debt"];
                if (financialProfileViews.Contains(_activeView))
                    _expendedIncomeAssets.Add(scenarioId);

                // Auto-expand Scenario Overview for net-worth or cashflow view
                if (_activeView == "net-worth" || _activeView == "cashflow")
                    _expandedScenarioOverview.Add(scenarioId);
                // Handle income sub-view: /scenario/{id}/income/{subview}
                if (_activeView == "income" && parts.Length >= 4)
                {
                    _activeIncomeSubView = parts[3];
                    _expandedIncomeSubMenu.Add(scenarioId);

                    // Auto-expand persisting income sub-menu when on those sub-views
                    if (_activeIncomeSubView == "real-estate-income" || _activeIncomeSubView == "other-persisting-income")
                        _expandedPersistingIncomeSubMenu.Add(scenarioId);
                }
                else if (_activeView == "income" && parts.Length == 3)
                {
                    // Landing on /income with no sub-view — default to employment
                    _activeIncomeSubView = "employment";
                    _expandedIncomeSubMenu.Add(scenarioId);
                }
                else
                {
                    _activeIncomeSubView = string.Empty;
                }

                // Handle assets sub-view: /scenario/{id}/assets/{subview}
                if (_activeView == "assets" && parts.Length >= 4)
                {
                    _activeAssetsSubView = parts[3];
                    _expandedAssetsSubMenu.Add(scenarioId);
                }
                else if (_activeView == "assets" && parts.Length == 3)
                {
                    _activeAssetsSubView = "home";
                    _expandedAssetsSubMenu.Add(scenarioId);
                }
                else if (_activeView != "assets")
                {
                    _activeAssetsSubView = string.Empty;
                }

                // Handle debt sub-view: /scenario/{id}/debt/{subview}
                if (_activeView == "debt" && parts.Length >= 4)
                {
                    _activeDebtSubView = parts[3];
                    _expandedDebtSubMenu.Add(scenarioId);
                }
                else if (_activeView == "debt" && parts.Length == 3)
                {
                    _activeDebtSubView = "home-mortgage";
                    _expandedDebtSubMenu.Add(scenarioId);
                }
                else if (_activeView != "debt")
                {
                    _activeDebtSubView = string.Empty;
                }

                // Handle spending sub-view: /scenario/{id}/spending/{subview}
                if (_activeView == "spending" && parts.Length >= 4)
                {
                    _activeSpendingSubView = parts[3];
                    _expandedSpendingSubMenu.Add(scenarioId);
                }
                else if (_activeView == "spending" && parts.Length == 3)
                {
                    _activeSpendingSubView = "living-expenses";
                    _expandedSpendingSubMenu.Add(scenarioId);
                }
                else if (_activeView != "spending")
                {
                    _activeSpendingSubView = string.Empty;
                }
            }
        }
        // Check for /home or overview
        else if (relativePath.StartsWith("home") || string.IsNullOrEmpty(relativePath))
        {
            _activeScenarioId = null;
            _activeView = "overview";
            _activeIncomeSubView = string.Empty;
        }
    }

    private async Task LoadScenarios()
    {
        if (UserId == 0) return;

        var result = await Mediator.Send(new GetScenariosQuery { UserId = UserId });
        if (result.Success)
        {
            _scenarios = result.Scenarios;
        }
    }

    private void ToggleScenario(long scenarioId)
    {
        if (!_expandedScenarios.Add(scenarioId))
        {
            _expandedScenarios.Remove(scenarioId);
        }
    }

    private void ToggleTemp1(long scenarioId)
    {
        if (!_expendedIncomeAssets.Add(scenarioId))
            _expendedIncomeAssets.Remove(scenarioId);
    }

    private void ToggleAssetsSubMenu(long scenarioId)
    {
        if (!_expandedAssetsSubMenu.Add(scenarioId))
            _expandedAssetsSubMenu.Remove(scenarioId);
    }

    private void NavigateToAssetsSubPage(long scenarioId, string subView)
    {
        _activeAssetsSubView = subView;
        _activeView = "assets";
        _activeScenarioId = scenarioId;
        Navigation.NavigateTo($"/scenario/{scenarioId}/assets/{subView}", forceLoad: false);
        InvokeAsync(StateHasChanged);
    }

    private string IsAssetsSubActive(long scenarioId, string subView)
    {
        if (_activeScenarioId == scenarioId && _activeView == "assets" && _activeAssetsSubView == subView)
            return "active";
        return string.Empty;
    }

    private static readonly string[] AssetSubViewOrder = ["home", "investment-properties", "investment-accounts", "physical-assets"];

    public void NavigateNextAssetStep()
    {
        if (_activeScenarioId is null) return;
        var idx = Array.IndexOf(AssetSubViewOrder, _activeAssetsSubView);
        if (idx >= 0 && idx < AssetSubViewOrder.Length - 1)
            NavigateToAssetsSubPage(_activeScenarioId.Value, AssetSubViewOrder[idx + 1]);
    }

    private void ToggleDebtSubMenu(long scenarioId)
    {
        if (!_expandedDebtSubMenu.Add(scenarioId))
            _expandedDebtSubMenu.Remove(scenarioId);
    }

    private void ToggleScenarioOverview(long scenarioId)
    {
        if (!_expandedScenarioOverview.Add(scenarioId))
            _expandedScenarioOverview.Remove(scenarioId);
    }

    private void NavigateToDebtSubPage(long scenarioId, string subView)
    {
        _activeDebtSubView = subView;
        _activeView = "debt";
        _activeScenarioId = scenarioId;
        Navigation.NavigateTo($"/scenario/{scenarioId}/debt/{subView}", forceLoad: false);
        InvokeAsync(StateHasChanged);
    }

    private string IsDebtSubActive(long scenarioId, string subView)
    {
        if (_activeScenarioId == scenarioId && _activeView == "debt" && _activeDebtSubView == subView)
            return "active";
        return string.Empty;
    }

    private static readonly string[] DebtSubViewOrder =
    [
        "home-mortgage", "investment-property-mortgage", "car-loans",
        "student-loans", "medical-debt", "personal-loans", "credit-cards"
    ];

    public void NavigateNextDebtStep()
    {
        if (_activeScenarioId is null) return;
        var idx = Array.IndexOf(DebtSubViewOrder, _activeDebtSubView);
        if (idx >= 0 && idx < DebtSubViewOrder.Length - 1)
            NavigateToDebtSubPage(_activeScenarioId.Value, DebtSubViewOrder[idx + 1]);
    }

    private void ToggleIncomeSubMenu(long scenarioId)
    {
        if (!_expandedIncomeSubMenu.Add(scenarioId))
            _expandedIncomeSubMenu.Remove(scenarioId);
    }

    private void NavigateToIncomeSubPage(long scenarioId, string subView)
    {
        _activeIncomeSubView = subView;
        _activeView = "income";
        _activeScenarioId = scenarioId;
        Navigation.NavigateTo($"/scenario/{scenarioId}/income/{subView}", forceLoad: false);
        InvokeAsync(StateHasChanged);
    }

    private void ToggleSpendingSubMenu(long scenarioId)
    {
        if (!_expandedSpendingSubMenu.Add(scenarioId))
            _expandedSpendingSubMenu.Remove(scenarioId);
    }

    private void NavigateToSpendingSubPage(long scenarioId, string subView)
    {
        _activeSpendingSubView = subView;
        _activeView = "spending";
        _activeScenarioId = scenarioId;
        Navigation.NavigateTo($"/scenario/{scenarioId}/spending/{subView}", forceLoad: false);
        InvokeAsync(StateHasChanged);
    }

    private string IsSpendingSubActive(long scenarioId, string subView)
    {
        if (_activeScenarioId == scenarioId && _activeView == "spending" && _activeSpendingSubView == subView)
            return "active";
        return string.Empty;
    }

    private static readonly string[] SpendingSubViewOrder =
    [
        "living-expenses", "discretionary-expenses", "debt-repayments",
        "assets-expenses", "other-expenses"
    ];

    public void NavigateNextSpendingStep()
    {
        if (_activeScenarioId is null) return;
        var idx = Array.IndexOf(SpendingSubViewOrder, _activeSpendingSubView);
        if (idx >= 0 && idx < SpendingSubViewOrder.Length - 1)
            NavigateToSpendingSubPage(_activeScenarioId.Value, SpendingSubViewOrder[idx + 1]);
    }

    private static readonly string[] IncomeSubViewOrder =
    [
        "employment", "self-employment", "defined-benefits",
        "defined-contribution", "group-rrsp", "defined-profit-sharing",
        "share-purchase-plan", "oas-cpp", "other",
        "real-estate-income", "other-persisting-income"
    ];

    private void TogglePersistingIncomeSubMenu(long scenarioId)
    {
        if (!_expandedPersistingIncomeSubMenu.Add(scenarioId))
            _expandedPersistingIncomeSubMenu.Remove(scenarioId);
    }

    public void NavigateNextIncomeStep()
    {
        if (_activeScenarioId is null) return;

        var idx = Array.IndexOf(IncomeSubViewOrder, _activeIncomeSubView);
        if (idx >= 0 && idx < IncomeSubViewOrder.Length - 1)
            NavigateToIncomeSubPage(_activeScenarioId.Value, IncomeSubViewOrder[idx + 1]);
    }    private async Task OnNewScenarioClick()
    {
        if (_isCreatingScenario) return;

        _isCreatingScenario = true;

        try
        {
            var command = new CreateScenarioCommand
            {
                UserId = UserId,
                ScenarioName = "New Scenario"
            };

            var result = await Mediator.Send(command);

            if (result.Success)
            {
                // Reload scenarios to include the new one
                await LoadScenarios();

                // Navigate to the new scenario's settings page
                Navigation.NavigateTo($"/scenario/{result.ScenarioId}/settings", forceLoad: false);
            }
        }
        finally
        {
            _isCreatingScenario = false;
        }
    }

    private void SelectItem(string item)
    {
        // Block all nav items except introduction when intro is not complete
        if (!HasCompletedIntro && item != "onboarding") return;

        // Navigate to the appropriate URL
        if (item == "overview")
        {
            Navigation.NavigateTo("/home", forceLoad: false);
        }
    }

    private void SelectScenarioSettings(long scenarioId)
    {
        // Navigate to scenario settings page
        Navigation.NavigateTo($"/scenario/{scenarioId}/settings", forceLoad: false);
    }

    private string IsActive(string item)
    {
        // Check for scenario sub-items (settings, income, spending, etc.)
        if (item.Contains("scenario_"))
        {
            var withoutPrefix = item.Replace("scenario_", "");
            var lastUnderscore = withoutPrefix.LastIndexOf('_');
            if (lastUnderscore > 0)
            {
                var scenarioIdStr = withoutPrefix[..lastUnderscore];
                var view = withoutPrefix[(lastUnderscore + 1)..];
                if (long.TryParse(scenarioIdStr, out var scenarioId))
                {
                    if (_activeView == view && _activeScenarioId == scenarioId)
                        return "active";
                }
            }
        }

        // Check for overview
        if (item == "overview" && (_activeView == "overview" || string.IsNullOrEmpty(_activeView)))
            return "active";

        return string.Empty;
    }

    private string IsIncomeSubActive(long scenarioId, string subView)
    {
        if (_activeScenarioId == scenarioId && _activeView == "income" && _activeIncomeSubView == subView)
            return "active";

        return string.Empty;
    }

    private string IsDisabled(string item)
    {
        if (HasCompletedIntro) return string.Empty;
        return item != "introduction" ? "nav-item-disabled" : string.Empty;
    }

    public void Dispose()
    {
        Navigation.LocationChanged -= OnLocationChanged;
        IncomeNavService.Unregister(NavigateNextIncomeStep);
        AssetNavService.Unregister(NavigateNextAssetStep);
        DebtNavService.Unregister(NavigateNextDebtStep);
        SpendingNavService.Unregister(NavigateNextSpendingStep);
    }
}

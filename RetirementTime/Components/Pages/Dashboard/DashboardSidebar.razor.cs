using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.CreateScenario;
using RetirementTime.Application.Features.Dashboard.GetScenarios;
using RetirementTime.Resources.Common;
using RetirementTime.Resources.Dashboard;

namespace RetirementTime.Components.Pages.Dashboard;

public partial class DashboardSidebar : ComponentBase, IDisposable
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<CommonResources> Localizer { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> DashboardLocalizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Parameter] public bool HasCompletedIntro { get; set; }
    [Parameter] public long UserId { get; set; }
    [Parameter] public int RefreshTrigger { get; set; }

    // Scenario state
    private List<Application.Features.Dashboard.Common.ScenarioDto> _scenarios = [];
    private HashSet<long> _expandedScenarios = [];
    private HashSet<long> _expendedIncomeAssets = [];
    private int _lastRefreshTrigger = -1;
    private bool _isCreatingScenario = false;

    // Active state based on current URL
    private string _currentUrl = string.Empty;
    private long? _activeScenarioId = null;
    private string _activeView = "overview";

    protected override async Task OnInitializedAsync()
    {
        await LoadScenarios();

        // Subscribe to navigation changes
        Navigation.LocationChanged += OnLocationChanged;

        // Parse initial URL
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

        // Parse /scenario/{id}/settings
        if (relativePath.StartsWith("scenario/"))
        {
            var parts = relativePath.Split('/');
            if (parts.Length >= 3 && long.TryParse(parts[1], out var scenarioId))
            {
                _activeScenarioId = scenarioId;
                _activeView = parts[2]; // "settings", "assets", etc.

                // Auto-expand the active scenario
                if (!_expandedScenarios.Contains(scenarioId))
                {
                    _expandedScenarios.Add(scenarioId);
                }

                // Auto-expand the Financial Profile group for views nested inside it
                HashSet<string> financialProfileViews = ["income", "spending"];
                if (financialProfileViews.Contains(_activeView) && !_expendedIncomeAssets.Contains(scenarioId))
                {
                    _expendedIncomeAssets.Add(scenarioId);
                }
            }
        }
        // Check for /home or overview
        else if (relativePath.StartsWith("home") || string.IsNullOrEmpty(relativePath))
        {
            _activeScenarioId = null;
            _activeView = "overview";
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
        if (_expandedScenarios.Contains(scenarioId))
        {
            _expandedScenarios.Remove(scenarioId);
        }
        else
        {
            _expandedScenarios.Add(scenarioId);
        }
    }

    private void ToggleTemp1(long scenarioId)
    {
        if (_expendedIncomeAssets.Contains(scenarioId))
        {
            _expendedIncomeAssets.Remove(scenarioId);
        }
        else
        {
            _expendedIncomeAssets.Add(scenarioId);
        }
    }

    private async Task OnNewScenarioClick()
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
                    {
                        return "active";
                    }
                }
            }
        }

        // Check for overview
        if (item == "overview" && (_activeView == "overview" || string.IsNullOrEmpty(_activeView)))
        {
            return "active";
        }

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
    }
}

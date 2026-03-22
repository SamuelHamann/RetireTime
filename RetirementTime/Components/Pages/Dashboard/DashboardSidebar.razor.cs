using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Resources.Common;

namespace RetirementTime.Components.Pages.Dashboard;

public partial class DashboardSidebar : ComponentBase
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<CommonResources> Localizer { get; set; } = default!;

    [Parameter] public string ActiveItem { get; set; } = "overview";
    [Parameter] public EventCallback<string> ActiveItemChanged { get; set; }
    [Parameter] public bool HasCompletedIntro { get; set; } = true;

    private static readonly Dictionary<string, string> _routes = new()
    {
        ["introduction"]    = "/introduction",
        ["overview"]        = "/home",
        ["assets"]          = "/home/assets",
        ["retirement-plan"] = "/home",
        ["debts"]           = "/home",
        ["income"]          = "/home",
        ["benefits"]        = "/home",
        ["projections"]     = "/home",
        ["settings"]        = "/home/settings",
    };

    private void SelectItem(string item)
    {
        // Block all nav items except introduction when intro is not complete
        if (!HasCompletedIntro && item != "introduction") return;

        ActiveItem = item;
        ActiveItemChanged.InvokeAsync(item);

        if (_routes.TryGetValue(item, out var route))
            Navigation.NavigateTo(route);
    }

    private string IsActive(string item) => ActiveItem == item ? "active" : string.Empty;

    private string IsDisabled(string item)
    {
        if (HasCompletedIntro) return string.Empty;
        return item != "introduction" ? "nav-item-disabled" : string.Empty;
    }
}

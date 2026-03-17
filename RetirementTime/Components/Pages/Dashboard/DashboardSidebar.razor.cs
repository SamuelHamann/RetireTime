using Microsoft.AspNetCore.Components;

namespace RetirementTime.Components.Pages.Dashboard;

public partial class DashboardSidebar : ComponentBase
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    [Parameter] public string ActiveItem { get; set; } = "overview";
    [Parameter] public EventCallback<string> ActiveItemChanged { get; set; }

    private static readonly Dictionary<string, string> _routes = new()
    {
        ["overview"]        = "/home",
        ["assets"]          = "/home/assets",
        ["retirement-plan"] = "/home",
        ["debts"]           = "/home",
        ["income"]          = "/home",
        ["benefits"]        = "/home",
        ["projections"]     = "/home",
        ["settings"]        = "/home",
    };

    private void SelectItem(string item)
    {
        ActiveItem = item;
        ActiveItemChanged.InvokeAsync(item);

        if (_routes.TryGetValue(item, out var route))
            Navigation.NavigateTo(route);
    }

    private string IsActive(string item) => ActiveItem == item ? "active" : string.Empty;
}


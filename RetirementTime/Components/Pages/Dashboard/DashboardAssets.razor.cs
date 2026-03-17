using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard;

public partial class DashboardAssets
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    private string _greeting = string.Empty;
    private string _activeSection = "assets";

    protected override async Task OnInitializedAsync()
    {
        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        _greeting = authenticatedUser?.FirstName ?? string.Empty;
    }
}


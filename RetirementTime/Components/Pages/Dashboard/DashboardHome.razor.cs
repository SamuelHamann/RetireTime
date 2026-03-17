using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard;

public partial class DashboardHome
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    private string _greeting = string.Empty;
    private string _title = string.Empty;
    private string _subtitle = string.Empty;
    private string _description = string.Empty;
    private string _activeSection = "overview";

    protected override async Task OnInitializedAsync()
    {
        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);

        _greeting = string.Format(Localizer["Home_Greeting"], authenticatedUser?.FirstName ?? string.Empty);
        _title = Localizer["Home_Title"];
        _subtitle = Localizer["Home_Subtitle"];
        _description = Localizer["Home_Description"];
    }
}


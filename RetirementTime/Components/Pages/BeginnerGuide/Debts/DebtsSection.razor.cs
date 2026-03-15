using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.BeginnerGuide.Debts;

public partial class DebtsSection
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    private long _userId;
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null)
        {
            Navigation.NavigateTo("/");
            return;
        }

        _userId = authenticatedUser.UserId;
        _isLoading = false;
    }

    private void GoToPreviousSection()
    {
        Navigation.NavigateTo("/beginner-guide/assets");
    }

    private void GoToNextSection()
    {
        Navigation.NavigateTo("/beginner-guide/income");
    }
}


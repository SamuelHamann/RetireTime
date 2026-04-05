using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.SpendingAndDebt;

public partial class Spending : ComponentBase, IDisposable
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private DelayedLoadingService LoadingService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private long _userId;

    protected override async Task OnInitializedAsync()
    {
        LoadingService.StartLoading(StateHasChanged, delayMs: 500);

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null)
        {
            Navigation.NavigateTo("/");
            return;
        }

        _userId = authenticatedUser.UserId;

        LoadingService.StopLoading();
    }

    public void Dispose()
    {
        LoadingService?.Dispose();
    }
}

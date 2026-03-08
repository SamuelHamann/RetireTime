using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.BeginnerGuide.Income;

public partial class IncomeSection
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    private long _userId;
    private int _currentStep;
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

    private void SetStep(int step)
    {
        _currentStep = step;
    }

    private void GoToNextStep()
    {
        if (_currentStep < 1)
            _currentStep++;
        else
            Navigation.NavigateTo("/beginner-guide/recurring-payments");
    }

    private void GoToPreviousSection()
    {
        Navigation.NavigateTo("/beginner-guide/debts");
    }
}


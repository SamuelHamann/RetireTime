using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.BeginnerGuide.Benefits;

public partial class BenefitsSection
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

    private void SetCurrentStep(int step)
    {
        _currentStep = step;
    }

    private string GetDotActiveClass(int step)
    {
        return step == _currentStep ? "active" : "";
    }

    private void GoToNextStep()
    {
        if (_currentStep < 2)
            _currentStep++;
    }

    private void GoToPreviousStep()
    {
        if (_currentStep > 0)
            _currentStep--;
    }

    private void GoToNextSection()
    {
        // TODO: Navigate to next section when it's implemented
        Navigation.NavigateTo("/beginner-guide");
    }

    private void GoToPreviousSection()
    {
        Navigation.NavigateTo("/beginner-guide/income");
    }
}

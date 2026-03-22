using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using RetirementTime.Application.Features.Users.CompleteIntro;
using RetirementTime.Resources.Introduction;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Introduction;

public partial class Introduction
{
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private IStringLocalizer<IntroResources> Localizer { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    private const int TotalSteps = 5;
    private int _currentStep = 1;
    private string _initials = "?";

    protected override async Task OnInitializedAsync()
    {
        var user = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (user != null)
        {
            if (!string.IsNullOrEmpty(user.FirstName))
                _initials = user.FirstName.Length >= 2 ? user.FirstName[..2].ToUpper() : user.FirstName.ToUpper();
            else if (!string.IsNullOrEmpty(user.Email))
                _initials = user.Email.Length >= 2 ? user.Email[..2].ToUpper() : user.Email.ToUpper();
        }
    }

    private async Task NextStep()
    {
        if (_currentStep < TotalSteps)
        {
            _currentStep++;
            await ScrollToTop();
        }
    }

    private async Task PreviousStep()
    {
        if (_currentStep > 1)
        {
            _currentStep--;
            await ScrollToTop();
        }
    }

    private async Task GoToStep(int step)
    {
        if (step >= 1 && step <= TotalSteps)
        {
            _currentStep = step;
            await ScrollToTop();
        }
    }

    private async Task ScrollToTop()
    {
        await JS.InvokeVoidAsync("eval", "document.getElementById('intro-main')?.scrollTo({top:0,behavior:'instant'})");
    }

    private async Task Finish()
    {
        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);

        if (authenticatedUser != null)
        {
            await Mediator.Send(new CompleteIntroCommand { UserId = authenticatedUser.UserId });
        }

        Navigation.NavigateTo("/home");
    }
}

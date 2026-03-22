using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using RetirementTime.Application.Features.Onboarding.GetAssets;
using RetirementTime.Application.Features.Onboarding.GetPersonalInfo;
using RetirementTime.Application.Features.Users.CompleteIntro;
using RetirementTime.Models.Introduction;
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
    private long _userId;
    private bool _isLoading = true;

    // View model containing all onboarding data
    private OnboardingDataViewModel _onboardingData = new();

    protected override async Task OnInitializedAsync()
    {
        var user = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (user != null)
        {
            _userId = user.UserId;

            if (!string.IsNullOrEmpty(user.FirstName))
                _initials = user.FirstName.Length >= 2 ? user.FirstName[..2].ToUpper() : user.FirstName.ToUpper();
            else if (!string.IsNullOrEmpty(user.Email))
                _initials = user.Email.Length >= 2 ? user.Email[..2].ToUpper() : user.Email.ToUpper();

            // Fetch all step data asynchronously
            await LoadAllOnboardingData();
        }

        _isLoading = false;
    }

    private async Task LoadAllOnboardingData()
    {
        // Create all query tasks
        var personalInfoTask = LoadPersonalInfoAsync();
        var assetsTask = LoadAssetsAsync();
        // TODO: Add more step data loading tasks here
        // var debtsTask = LoadDebtsAsync();
        // var employmentTask = LoadEmploymentAsync();

        // Wait for all tasks to complete
        await Task.WhenAll(
            personalInfoTask,
            assetsTask
            // Add more tasks here as they're implemented
            // debtsTask,
            // employmentTask
        );
    }

    private async Task LoadPersonalInfoAsync()
    {
        try
        {
            var query = new GetPersonalInfoQuery { UserId = _userId };
            var result = await Mediator.Send(query);

            if (result.Success && result.PersonalInfo != null)
            {
                // Convert DTO to Model for UI use
                _onboardingData.PersonalInfo = new PersonalInfoModel
                {
                    Email = result.PersonalInfo.Email,
                    FirstName = result.PersonalInfo.FirstName,
                    LastName = result.PersonalInfo.LastName,
                    DateOfBirth = result.PersonalInfo.DateOfBirth,
                    CitizenshipStatus = result.PersonalInfo.CitizenshipStatus,
                    MaritalStatus = result.PersonalInfo.MaritalStatus,
                    HasCurrentChildren = result.PersonalInfo.HasCurrentChildren,
                    PlansFutureChildren = result.PersonalInfo.PlansFutureChildren,
                    IncludePartner = result.PersonalInfo.IncludePartner
                };
            }
        }
        catch (Exception)
        {
            // Silently fail - step will show as incomplete
        }
    }

    private async Task LoadAssetsAsync()
    {
        try
        {
            var query = new GetAssetsQuery { UserId = _userId };
            var result = await Mediator.Send(query);

            if (result.Success && result.Assets != null)
            {
                // Convert DTO to Model for UI use
                _onboardingData.Assets = new AssetsModel
                {
                    HasSavingsAccount = result.Assets.HasSavingsAccount,
                    HasTFSA = result.Assets.HasTFSA,
                    HasRRSP = result.Assets.HasRRSP,
                    HasRRIF = result.Assets.HasRRIF,
                    HasFHSA = result.Assets.HasFHSA,
                    HasRESP = result.Assets.HasRESP,
                    HasRDSP = result.Assets.HasRDSP,
                    HasNonRegistered = result.Assets.HasNonRegistered,
                    HasPension = result.Assets.HasPension,
                    HasPrincipalResidence = result.Assets.HasPrincipalResidence,
                    HasCar = result.Assets.HasCar,
                    HasInvestmentProperty = result.Assets.HasInvestmentProperty,
                    HasBusiness = result.Assets.HasBusiness,
                    HasIncorporation = result.Assets.HasIncorporation,
                    HasPreciousMetals = result.Assets.HasPreciousMetals,
                    HasOtherHardAssets = result.Assets.HasOtherHardAssets
                };
            }
        }
        catch (Exception)
        {
            // Silently fail - step will show as incomplete
        }
    }

    // TODO: Add methods for loading other step data
    // private async Task LoadDebtsAsync() { ... }
    // private async Task LoadEmploymentAsync() { ... }

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

    private void OnStepDataChanged(PersonalInfoModel updatedModel)
    {
        // Update the view model directly with the saved data
        _onboardingData.PersonalInfo = updatedModel;
        StateHasChanged();
    }

    private void OnAssetsChanged(AssetsModel updatedModel)
    {
        // Update the view model directly with the saved data
        _onboardingData.Assets = updatedModel;
        StateHasChanged();
    }
}

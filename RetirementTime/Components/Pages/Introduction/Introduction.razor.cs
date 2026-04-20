using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using RetirementTime.Application.Features.Onboarding.GetAssets;
using RetirementTime.Application.Features.Onboarding.GetDebt;
using RetirementTime.Application.Features.Onboarding.GetEmployment;
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
    private bool _hasCompletedIntro = false;

    // View model containing all onboarding data
    private OnboardingDataViewModel _onboardingData = new();

    // Calculate the active step (first incomplete step or last step if all complete)
    private int GetActiveStep()
    {
        if (!_onboardingData.IsStep1Complete) return 2; // Step 1 is PersonalInfo (step 2 in UI)
        if (!_onboardingData.IsStep2Complete) return 3; // Step 2 is Assets (step 3 in UI)
        if (!_onboardingData.IsStep3Complete) return 4; // Step 3 is Debt (step 4 in UI)
        if (!_onboardingData.IsStep4Complete) return 5; // Step 4 is Employment (step 5 in UI)
        return 5; // All complete, active step is the last step
    }

    protected override async Task OnInitializedAsync()
    {
        var user = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (user != null)
        {
            _userId = user.UserId;
            _hasCompletedIntro = user.HasCompletedIntro;

            if (!string.IsNullOrEmpty(user.FirstName))
                _initials = user.FirstName.Length >= 2 ? user.FirstName[..2].ToUpper() : user.FirstName.ToUpper();
            else if (!string.IsNullOrEmpty(user.Email))
                _initials = user.Email.Length >= 2 ? user.Email[..2].ToUpper() : user.Email.ToUpper();

            // Fetch all step data asynchronously
            await LoadAllOnboardingData();

            // Start at welcome page
            _currentStep = 1;
        }

        _isLoading = false;
    }

    private async Task LoadAllOnboardingData()
    {
        // Load data sequentially to avoid DbContext concurrency issues
        await LoadPersonalInfoAsync();
        await LoadAssetsAsync();
        await LoadDebtAsync();
        await LoadEmploymentAsync();
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

    private async Task LoadDebtAsync()
    {
        try
        {
            var query = new GetDebtQuery { UserId = _userId };
            var result = await Mediator.Send(query);

            if (result.Success && result.Debt != null)
            {
                // Convert DTO to Model for UI use
                _onboardingData.Debt = new DebtModel
                {
                    HasPrimaryMortgage = result.Debt.HasPrimaryMortgage,
                    HasInvestmentPropertyMortgage = result.Debt.HasInvestmentPropertyMortgage,
                    HasCarPayments = result.Debt.HasCarPayments,
                    HasStudentLoans = result.Debt.HasStudentLoans,
                    HasCreditCardDebt = result.Debt.HasCreditCardDebt,
                    HasPersonalLoans = result.Debt.HasPersonalLoans,
                    HasBusinessLoans = result.Debt.HasBusinessLoans,
                    HasTaxDebt = result.Debt.HasTaxDebt,
                    HasMedicalDebt = result.Debt.HasMedicalDebt,
                    HasInformalDebt = result.Debt.HasInformalDebt
                };
            }
        }
        catch (Exception)
        {
            // Silently fail - step will show as incomplete
        }
    }

    private async Task LoadEmploymentAsync()
    {
        try
        {
            var query = new GetEmploymentQuery { UserId = _userId };
            var result = await Mediator.Send(query);

            if (result.Success && result.Employment != null)
            {
                // Convert DTO to Model for UI use
                _onboardingData.Employment = new EmploymentModel
                {
                    IsEmployed = result.Employment.IsEmployed,
                    IsSelfEmployed = result.Employment.IsSelfEmployed,
                    PlannedRetirementAge = result.Employment.PlannedRetirementAge,
                    CppContributionYears = result.Employment.CppContributionYears,
                    HasRoyalties = result.Employment.HasRoyalties,
                    HasDividends = result.Employment.HasDividends,
                    HasRentalIncome = result.Employment.HasRentalIncome,
                    HasOtherIncome = result.Employment.HasOtherIncome
                };
            }
        }
        catch (Exception)
        {
            // Silently fail - step will show as incomplete
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

    private void OnDebtChanged(DebtModel updatedModel)
    {
        // Update the view model directly with the saved data
        _onboardingData.Debt = updatedModel;
        // Force re-render to update completion status in sidebar
        StateHasChanged();
    }

    private void OnEmploymentChanged(EmploymentModel updatedModel)
    {
        // Update the view model directly with the saved data
        _onboardingData.Employment = updatedModel;
        // Force re-render to update completion status in sidebar
        StateHasChanged();
    }
}

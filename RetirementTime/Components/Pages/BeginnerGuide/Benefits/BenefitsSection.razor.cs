using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Application.Features.BeginnerGuide.Benefits.GetGovernmentPension;
using RetirementTime.Application.Features.BeginnerGuide.Benefits.GetOtherRecurringGains;
using RetirementTime.Application.Features.BeginnerGuide.Benefits.GetPensionTypes;
using RetirementTime.Application.Features.BeginnerGuide.Benefits.GetPensions;
using RetirementTime.Application.Features.UserProgress.CompleteBeginnerGuide;
using RetirementTime.Models.BeginnerGuide.Benefits;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.BeginnerGuide.Benefits;

public partial class BenefitsSection
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    private long _userId;
    private int _currentStep;
    private bool _isLoading = true;

    // Step 1: Pensions
    private bool _hasPensions;
    private List<PensionFormModel> _pensions = new();
    private List<PensionTypeDto> _pensionTypes = new();

    // Step 2: Government Pension
    private GovernmentPensionFormModel _governmentPension = new();

    // Step 3: Other Recurring Gains
    private bool _hasOtherRecurringGains;
    private List<OtherRecurringGainFormModel> _otherRecurringGains = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadExistingData();
        _isLoading = false;
    }

    private async Task LoadExistingData()
    {
        try
        {
            var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
            if (authenticatedUser == null)
            {
                Navigation.NavigateTo("/");
                return;
            }

            _userId = authenticatedUser.UserId;

            // Load all data in parallel
            var pensionTypesTask = Mediator.Send(new GetPensionTypesQuery());
            var pensionsTask = Mediator.Send(new GetPensionsQuery { UserId = _userId });
            var governmentPensionTask = Mediator.Send(new GetGovernmentPensionQuery { UserId = _userId });
            var otherGainsTask = Mediator.Send(new GetOtherRecurringGainsQuery { UserId = _userId });

            await Task.WhenAll(pensionTypesTask, pensionsTask, governmentPensionTask, otherGainsTask);

            // Step 1
            _pensionTypes = await pensionTypesTask;
            var existingPensions = await pensionsTask;
            _hasPensions = existingPensions.Count > 0;
            _pensions = existingPensions.Select(p => new PensionFormModel
            {
                EmployerName = p.EmployerName,
                PensionTypeId = p.PensionTypeId
            }).ToList();

            // Step 2
            var existingGovPension = await governmentPensionTask;
            if (existingGovPension is not null)
            {
                _governmentPension = new GovernmentPensionFormModel
                {
                    YearsWorked = existingGovPension.YearsWorked,
                    HasSpecializedPublicSectorPension = existingGovPension.HasSpecializedPublicSectorPension,
                    SpecializedPensionName = existingGovPension.SpecializedPensionName
                };
            }

            // Step 3
            var existingGains = await otherGainsTask;
            _hasOtherRecurringGains = existingGains.Count > 0;
            _otherRecurringGains = existingGains.Select(g => new OtherRecurringGainFormModel
            {
                SourceName = g.SourceName,
                Amount = g.Amount,
                FrequencyId = g.FrequencyId
            }).ToList();
        }
        catch (Exception)
        {
            // Silently fail and start with empty forms
        }
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

    private async Task GoToNextSection()
    {
        await Mediator.Send(new CompleteBeginnerGuideCommand { UserId = _userId });
        Navigation.NavigateTo("/home");
    }

    private void GoToPreviousSection()
    {
        Navigation.NavigateTo("/beginner-guide/income");
    }
}

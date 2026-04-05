using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Onboarding.UpsertEmployment;
using RetirementTime.Models.Introduction;
using RetirementTime.Resources.Introduction;

namespace RetirementTime.Components.Pages.Introduction.Steps;

public partial class Step4Employment
{
    [Inject] private IStringLocalizer<IntroResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Parameter] public long UserId { get; set; }
    [Parameter] public EmploymentModel? InitialData { get; set; }
    [Parameter] public EventCallback<EmploymentModel> OnDataChanged { get; set; }
    [Parameter] public EventCallback OnFinish { get; set; }
    [Parameter] public EventCallback OnBack { get; set; }

    public EmploymentModel Model { get; set; } = new();
    private bool _isInitialized = false;

    // Step4 requires PlannedRetirementAge
    private bool IsFormValid => Model.PlannedRetirementAge.HasValue && Model.PlannedRetirementAge.Value > 0;

    protected override async Task OnParametersSetAsync()
    {
        if (!_isInitialized)
        {
            LoadInitialData();
            _isInitialized = true;
        }
    }

    private void LoadInitialData()
    {
        if (InitialData != null)
        {
            // Use the model directly from parent
            Model = InitialData;
        }
    }

    private async Task SaveData()
    {
        if (!_isInitialized || UserId == 0)
            return;

        try
        {
            var command = new UpsertEmploymentCommand
            {
                UserId = UserId,
                IsEmployed = Model.IsEmployed,
                IsSelfEmployed = Model.IsSelfEmployed,
                PlannedRetirementAge = Model.PlannedRetirementAge,
                CppContributionYears = Model.CppContributionYears,
                HasRoyalties = Model.HasRoyalties,
                HasDividends = Model.HasDividends,
                HasRentalIncome = Model.HasRentalIncome,
                HasOtherIncome = Model.HasOtherIncome
            };

            var result = await Mediator.Send(command);

            if (result.Success)
            {
                // Notify parent with the updated model
                await OnDataChanged.InvokeAsync(Model);
            }
        }
        catch (Exception)
        {
            // Silently fail - user can retry when navigating
        }
    }

    private async Task OnFieldChanged()
    {
        StateHasChanged();
    }

    private async Task HandleFinish()
    {
        await SaveData();
        await OnFinish.InvokeAsync();
    }

    private async Task HandleBack()
    {
        await SaveData();
        await OnBack.InvokeAsync();
    }
}


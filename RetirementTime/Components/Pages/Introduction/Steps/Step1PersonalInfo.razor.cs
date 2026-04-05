using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Onboarding.UpsertPersonalInfo;
using RetirementTime.Models.Introduction;
using RetirementTime.Resources.Introduction;

namespace RetirementTime.Components.Pages.Introduction.Steps;

public partial class Step1PersonalInfo
{
    [Inject] private IStringLocalizer<IntroResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Parameter] public long UserId { get; set; }
    [Parameter] public PersonalInfoModel? InitialData { get; set; }
    [Parameter] public EventCallback<PersonalInfoModel> OnDataChanged { get; set; }
    [Parameter] public EventCallback OnNext { get; set; }
    [Parameter] public EventCallback OnBack { get; set; }

    public PersonalInfoModel Model { get; set; } = new();
    private bool _isInitialized = false;

    private bool IsFormValid =>
        Model.DateOfBirth.HasValue &&
        !string.IsNullOrWhiteSpace(Model.CitizenshipStatus) &&
        !string.IsNullOrWhiteSpace(Model.MaritalStatus);

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

    public async Task SaveData()
    {
        if (!_isInitialized || UserId == 0 || !IsFormValid)
            return;

        try
        {
            var command = new UpsertPersonalInfoCommand
            {
                UserId = UserId,
                DateOfBirth = Model.DateOfBirth.Value,
                CitizenshipStatus = Model.CitizenshipStatus,
                MaritalStatus = Model.MaritalStatus,
                HasCurrentChildren = Model.HasCurrentChildren,
                PlansFutureChildren = Model.PlansFutureChildren,
                IncludePartner = Model.IncludePartner
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

    private async Task HandleNext()
    {
        await SaveData();
        await OnNext.InvokeAsync();
    }

    private async Task HandleBack()
    {
        await SaveData();
        await OnBack.InvokeAsync();
    }
}

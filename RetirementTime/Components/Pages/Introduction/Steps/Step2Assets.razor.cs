using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Onboarding.UpsertAssets;
using RetirementTime.Models.Introduction;
using RetirementTime.Resources.Introduction;

namespace RetirementTime.Components.Pages.Introduction.Steps;

public partial class Step2Assets
{
    [Inject] private IStringLocalizer<IntroResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Parameter] public long UserId { get; set; }
    [Parameter] public AssetsModel? InitialData { get; set; }
    [Parameter] public EventCallback<AssetsModel> OnDataChanged { get; set; }
    [Parameter] public EventCallback OnNext { get; set; }
    [Parameter] public EventCallback OnBack { get; set; }

    public AssetsModel Model { get; set; } = new();
    private bool _isInitialized = false;

    // Step2 has no required fields - all checkboxes are optional
    private bool IsFormValid => true;

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
            var command = new UpsertAssetsCommand
            {
                UserId = UserId,
                HasSavingsAccount = Model.HasSavingsAccount,
                HasTFSA = Model.HasTFSA,
                HasRRSP = Model.HasRRSP,
                HasRRIF = Model.HasRRIF,
                HasFHSA = Model.HasFHSA,
                HasRESP = Model.HasRESP,
                HasRDSP = Model.HasRDSP,
                HasNonRegistered = Model.HasNonRegistered,
                HasPension = Model.HasPension,
                HasPrincipalResidence = Model.HasPrincipalResidence,
                HasCar = Model.HasCar,
                HasInvestmentProperty = Model.HasInvestmentProperty,
                HasBusiness = Model.HasBusiness,
                HasIncorporation = Model.HasIncorporation,
                HasPreciousMetals = Model.HasPreciousMetals,
                HasOtherHardAssets = Model.HasOtherHardAssets
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
        await SaveData();
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


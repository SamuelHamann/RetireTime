using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Onboarding.UpsertDebt;
using RetirementTime.Models.Introduction;
using RetirementTime.Resources.Introduction;

namespace RetirementTime.Components.Pages.Introduction.Steps;

public partial class Step3Debt
{
    [Inject] private IStringLocalizer<IntroResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Parameter] public long UserId { get; set; }
    [Parameter] public DebtModel? InitialData { get; set; }
    [Parameter] public EventCallback<DebtModel> OnDataChanged { get; set; }
    [Parameter] public EventCallback OnNext { get; set; }
    [Parameter] public EventCallback OnBack { get; set; }

    public DebtModel Model { get; set; } = new();
    private bool _isInitialized = false;

    // Step3 has no required fields - all checkboxes are optional
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
            var command = new UpsertDebtCommand
            {
                UserId = UserId,
                HasPrimaryMortgage = Model.HasPrimaryMortgage,
                HasInvestmentPropertyMortgage = Model.HasInvestmentPropertyMortgage,
                HasCarPayments = Model.HasCarPayments,
                HasStudentLoans = Model.HasStudentLoans,
                HasCreditCardDebt = Model.HasCreditCardDebt,
                HasPersonalLoans = Model.HasPersonalLoans,
                HasBusinessLoans = Model.HasBusinessLoans,
                HasTaxDebt = Model.HasTaxDebt,
                HasMedicalDebt = Model.HasMedicalDebt,
                HasInformalDebt = Model.HasInformalDebt
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


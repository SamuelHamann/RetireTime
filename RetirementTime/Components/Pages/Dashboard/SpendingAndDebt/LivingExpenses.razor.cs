using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Spending.GetLivingExpenses;
using RetirementTime.Application.Features.Dashboard.Spending.SaveLivingExpenses;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Models.Spending;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.SpendingAndDebt;

public partial class LivingExpenses : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private SpendingNavigationService SpendingNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private SpendingLivingExpensesModel _model = new();
    private List<Frequency> _frequencies = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var result = await Mediator.Send(new GetLivingExpensesQuery(ScenarioId));
        _frequencies = result.Frequencies;

        if (result.Expenses is { } e)
        {
            _model.RentOrMortgage                 = e.RentOrMortgage;
            _model.RentOrMortgageFrequencyId      = e.RentOrMortgageFrequencyId;
            _model.Food                           = e.Food;
            _model.FoodFrequencyId                = e.FoodFrequencyId;
            _model.Utilities                      = e.Utilities;
            _model.UtilitiesFrequencyId           = e.UtilitiesFrequencyId;
            _model.Insurance                      = e.Insurance;
            _model.InsuranceFrequencyId           = e.InsuranceFrequencyId;
            _model.Gas                            = e.Gas;
            _model.GasFrequencyId                 = e.GasFrequencyId;
            _model.HomeMaintenance                = e.HomeMaintenance;
            _model.HomeMaintenanceFrequencyId     = e.HomeMaintenanceFrequencyId;
            _model.PropertyTax                    = e.PropertyTax;
            _model.PropertyTaxFrequencyId         = e.PropertyTaxFrequencyId;
            _model.Cellphone                      = e.Cellphone;
            _model.CellphoneFrequencyId           = e.CellphoneFrequencyId;
            _model.HealthSpendings                = e.HealthSpendings;
            _model.HealthSpendingsFrequencyId     = e.HealthSpendingsFrequencyId;
            _model.OtherLivingExpenses            = e.OtherLivingExpenses;
            _model.OtherLivingExpensesFrequencyId = e.OtherLivingExpensesFrequencyId;
        }

        _isLoading = false;
        StateHasChanged();
    }

    private async Task Save()
    {
        await Mediator.Send(new SaveLivingExpensesCommand
        {
            ScenarioId                     = ScenarioId,
            RentOrMortgage                 = _model.RentOrMortgage,
            RentOrMortgageFrequencyId      = _model.RentOrMortgageFrequencyId,
            Food                           = _model.Food,
            FoodFrequencyId                = _model.FoodFrequencyId,
            Utilities                      = _model.Utilities,
            UtilitiesFrequencyId           = _model.UtilitiesFrequencyId,
            Insurance                      = _model.Insurance,
            InsuranceFrequencyId           = _model.InsuranceFrequencyId,
            Gas                            = _model.Gas,
            GasFrequencyId                 = _model.GasFrequencyId,
            HomeMaintenance                = _model.HomeMaintenance,
            HomeMaintenanceFrequencyId     = _model.HomeMaintenanceFrequencyId,
            PropertyTax                    = _model.PropertyTax,
            PropertyTaxFrequencyId         = _model.PropertyTaxFrequencyId,
            Cellphone                      = _model.Cellphone,
            CellphoneFrequencyId           = _model.CellphoneFrequencyId,
            HealthSpendings                = _model.HealthSpendings,
            HealthSpendingsFrequencyId     = _model.HealthSpendingsFrequencyId,
            OtherLivingExpenses            = _model.OtherLivingExpenses,
            OtherLivingExpensesFrequencyId = _model.OtherLivingExpensesFrequencyId,
        });
    }

    private void NavigateNext() =>
        Navigation.NavigateTo($"/scenario/{ScenarioId}/spending/discretionary-expenses");
}

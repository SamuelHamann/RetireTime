using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Income.CreateOtherIncomeOrBenefits;
using RetirementTime.Application.Features.Dashboard.Income.DeleteOtherIncomeOrBenefits;
using RetirementTime.Application.Features.Dashboard.Income.GetOtherIncomeOrBenefits;
using RetirementTime.Application.Features.Dashboard.Income.UpdateOtherIncomeOrBenefits;
using RetirementTime.Models.Income;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class OtherIncome : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private List<OtherIncomeOrBenefitsItemModel> _incomeItems = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var items = await Mediator.Send(new GetOtherIncomeOrBenefitsQuery(ScenarioId));
        _incomeItems = items.Select(e => new OtherIncomeOrBenefitsItemModel { Id = e.Id, Name = e.Name, Amount = e.Amount }).ToList();
        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddIncome()
    {
        var result = await Mediator.Send(new CreateOtherIncomeOrBenefitsCommand(ScenarioId));
        if (result.Success) _incomeItems.Add(new OtherIncomeOrBenefitsItemModel { Id = result.IncomeId });
    }

    private async Task RemoveIncome(OtherIncomeOrBenefitsItemModel item)
    {
        await Mediator.Send(new DeleteOtherIncomeOrBenefitsCommand(item.Id));
        _incomeItems.Remove(item);
    }

    private async Task SaveIncome(OtherIncomeOrBenefitsItemModel item)
    {
        if (item.Id == 0) return;
        await Mediator.Send(new UpdateOtherIncomeOrBenefitsCommand { Id = item.Id, Name = item.Name, Amount = item.Amount });
    }
}

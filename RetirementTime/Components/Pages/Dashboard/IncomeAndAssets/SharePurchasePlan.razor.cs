using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Income.CreateSharePurchasePlan;
using RetirementTime.Application.Features.Dashboard.Income.DeleteSharePurchasePlan;
using RetirementTime.Application.Features.Dashboard.Income.GetSharePurchasePlan;
using RetirementTime.Application.Features.Dashboard.Income.UpdateSharePurchasePlan;
using RetirementTime.Models.Income;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class SharePurchasePlan : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private IncomeNavigationService IncomeNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private List<SharePurchasePlanItemModel> _planItems = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var items = await Mediator.Send(new GetSharePurchasePlanQuery(ScenarioId));
        _planItems = items.Select(e => new SharePurchasePlanItemModel { Id = e.Id, Name = e.Name, PercentOfSalaryEmployee = e.PercentOfSalaryEmployee, PercentOfSalaryEmployer = e.PercentOfSalaryEmployer, UseFlatAmountInsteadOfPercent = e.UseFlatAmountInsteadOfPercent }).ToList();
        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddPlan()
    {
        var result = await Mediator.Send(new CreateSharePurchasePlanCommand(ScenarioId));
        if (result.Success) _planItems.Add(new SharePurchasePlanItemModel { Id = result.PlanId });
    }

    private async Task RemovePlan(SharePurchasePlanItemModel item)
    {
        await Mediator.Send(new DeleteSharePurchasePlanCommand(item.Id));
        _planItems.Remove(item);
    }

    private async Task SavePlan(SharePurchasePlanItemModel item)
    {
        if (item.Id == 0) return;
        await Mediator.Send(new UpdateSharePurchasePlanCommand { Id = item.Id, Name = item.Name, PercentOfSalaryEmployee = item.PercentOfSalaryEmployee, PercentOfSalaryEmployer = item.PercentOfSalaryEmployer, UseFlatAmountInsteadOfPercent = item.UseFlatAmountInsteadOfPercent });
    }

    private void NavigateNext() => IncomeNavService.NavigateNext();
}

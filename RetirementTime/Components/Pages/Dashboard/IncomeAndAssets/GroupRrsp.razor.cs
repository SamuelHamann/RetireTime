using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Income.CreateGroupRrsp;
using RetirementTime.Application.Features.Dashboard.Income.DeleteGroupRrsp;
using RetirementTime.Application.Features.Dashboard.Income.GetGroupRrsp;
using RetirementTime.Application.Features.Dashboard.Income.UpdateGroupRrsp;
using RetirementTime.Models.Income;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class GroupRrsp : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private IncomeNavigationService IncomeNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }
    [Parameter] public long TimelineId { get; set; }

    private bool _isLoading = true;
    private List<GroupRrspItemModel> _planItems = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var items = await Mediator.Send(new GetGroupRrspQuery(ScenarioId, TimelineId));
        _planItems = items.Select(e => new GroupRrspItemModel { Id = e.Id, Name = e.Name, PercentOfSalaryEmployee = e.PercentOfSalaryEmployee, PercentOfSalaryEmployer = e.PercentOfSalaryEmployer }).ToList();
        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddPlan()
    {
        var result = await Mediator.Send(new CreateGroupRrspCommand(ScenarioId, TimelineId));
        if (result.Success) _planItems.Add(new GroupRrspItemModel { Id = result.PlanId });
    }

    private async Task RemovePlan(GroupRrspItemModel item)
    {
        await Mediator.Send(new DeleteGroupRrspCommand(item.Id));
        _planItems.Remove(item);
    }

    private async Task SavePlan(GroupRrspItemModel item)
    {
        if (item.Id == 0) return;
        await Mediator.Send(new UpdateGroupRrspCommand { Id = item.Id, Name = item.Name, PercentOfSalaryEmployee = item.PercentOfSalaryEmployee, PercentOfSalaryEmployer = item.PercentOfSalaryEmployer });
    }

    private void NavigateNext() => IncomeNavService.NavigateNext();
}

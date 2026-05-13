using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Income.CreateDefinedProfitSharing;
using RetirementTime.Application.Features.Dashboard.Income.DeleteDefinedProfitSharing;
using RetirementTime.Application.Features.Dashboard.Income.GetDefinedProfitSharing;
using RetirementTime.Application.Features.Dashboard.Income.UpdateDefinedProfitSharing;
using RetirementTime.Models.Income;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class DefinedProfitSharing : ComponentBase
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
    private List<DefinedProfitSharingItemModel> _planItems = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null)
        {
            Navigation.NavigateTo("/");
            return;
        }

        var items = await Mediator.Send(new GetDefinedProfitSharingQuery(ScenarioId, TimelineId));

        _planItems = items.Select(e => new DefinedProfitSharingItemModel
        {
            Id = e.Id,
            Name = e.Name,
            PercentOfSalaryEmployer = e.PercentOfSalaryEmployer
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddPlan()
    {
        var result = await Mediator.Send(new CreateDefinedProfitSharingCommand(ScenarioId, TimelineId));
        if (result.Success)
        {
            _planItems.Add(new DefinedProfitSharingItemModel { Id = result.PlanId });
        }
    }

    private async Task RemovePlan(DefinedProfitSharingItemModel item)
    {
        await Mediator.Send(new DeleteDefinedProfitSharingCommand(item.Id));
        _planItems.Remove(item);
    }

    private async Task SavePlan(DefinedProfitSharingItemModel item)
    {
        if (item.Id == 0) return;

        await Mediator.Send(new UpdateDefinedProfitSharingCommand
        {
            Id = item.Id,
            Name = item.Name,
            PercentOfSalaryEmployer = item.PercentOfSalaryEmployer
        });
    }

    private void NavigateNext() => IncomeNavService.NavigateNext();
}


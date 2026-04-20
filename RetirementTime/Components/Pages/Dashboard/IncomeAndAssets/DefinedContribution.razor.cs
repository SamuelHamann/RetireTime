using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Income.CreatePensionDefinedContribution;
using RetirementTime.Application.Features.Dashboard.Income.DeletePensionDefinedContribution;
using RetirementTime.Application.Features.Dashboard.Income.GetPensionDefinedContribution;
using RetirementTime.Application.Features.Dashboard.Income.UpdatePensionDefinedContribution;
using RetirementTime.Models.Income;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class DefinedContribution : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private IncomeNavigationService IncomeNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private List<PensionDefinedContributionItemModel> _planItems = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null)
        {
            Navigation.NavigateTo("/");
            return;
        }

        var items = await Mediator.Send(new GetPensionDefinedContributionQuery(ScenarioId));

        _planItems = items.Select(e => new PensionDefinedContributionItemModel
        {
            Id = e.Id,
            Name = e.Name,
            PercentOfSalaryEmployee = e.PercentOfSalaryEmployee,
            PercentOfSalaryEmployer = e.PercentOfSalaryEmployer
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddPlan()
    {
        var result = await Mediator.Send(new CreatePensionDefinedContributionCommand(ScenarioId));
        if (result.Success)
        {
            _planItems.Add(new PensionDefinedContributionItemModel { Id = result.PlanId });
        }
    }

    private async Task RemovePlan(PensionDefinedContributionItemModel item)
    {
        await Mediator.Send(new DeletePensionDefinedContributionCommand(item.Id));
        _planItems.Remove(item);
    }

    private async Task SavePlan(PensionDefinedContributionItemModel item)
    {
        if (item.Id == 0) return;

        await Mediator.Send(new UpdatePensionDefinedContributionCommand
        {
            Id = item.Id,
            Name = item.Name,
            PercentOfSalaryEmployee = item.PercentOfSalaryEmployee,
            PercentOfSalaryEmployer = item.PercentOfSalaryEmployer
        });
    }

    private void NavigateNext() => IncomeNavService.NavigateNext();
}


using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Income.CreateSelfEmploymentIncome;
using RetirementTime.Application.Features.Dashboard.Income.DeleteSelfEmploymentIncome;
using RetirementTime.Application.Features.Dashboard.Income.GetSelfEmploymentIncomes;
using RetirementTime.Application.Features.Dashboard.Income.UpdateSelfEmploymentIncome;
using RetirementTime.Models.Income;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class SelfEmployment : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private IncomeNavigationService IncomeNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private List<SelfEmploymentItemModel> _selfEmploymentItems = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null)
        {
            Navigation.NavigateTo("/");
            return;
        }

        var items = await Mediator.Send(new GetSelfEmploymentIncomesQuery(ScenarioId));

        _selfEmploymentItems = items.Select(e => new SelfEmploymentItemModel
        {
            Id = e.Id,
            Name = e.Name,
            GrossSalary = e.GrossSalary,
            NetSalary = e.NetSalary,
            GrossDividends = e.GrossDividends,
            NetDividends = e.NetDividends
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddSelfEmployment()
    {
        var result = await Mediator.Send(new CreateSelfEmploymentIncomeCommand(ScenarioId));
        if (result.Success)
        {
            _selfEmploymentItems.Add(new SelfEmploymentItemModel { Id = result.SelfEmploymentIncomeId });
        }
    }

    private async Task RemoveSelfEmployment(SelfEmploymentItemModel item)
    {
        await Mediator.Send(new DeleteSelfEmploymentIncomeCommand(item.Id));
        _selfEmploymentItems.Remove(item);
    }

    private async Task SaveSelfEmployment(SelfEmploymentItemModel item)
    {
        if (item.Id == 0) return;

        await Mediator.Send(new UpdateSelfEmploymentIncomeCommand
        {
            Id = item.Id,
            Name = item.Name,
            GrossSalary = item.GrossSalary,
            NetSalary = item.NetSalary,
            GrossDividends = item.GrossDividends,
            NetDividends = item.NetDividends
        });
    }

    private void NavigateNext() => IncomeNavService.NavigateNext();
}

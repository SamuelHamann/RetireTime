using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Common.GetFrequencies;
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
    [Parameter] public long TimelineId { get; set; }

    private bool _isLoading = true;
    private List<SelfEmploymentItemModel> _selfEmploymentItems = [];
    private List<FrequencyDto> _frequencies = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null)
        {
            Navigation.NavigateTo("/");
            return;
        }

        _frequencies = await Mediator.Send(new GetFrequenciesQuery());
        var items = await Mediator.Send(new GetSelfEmploymentIncomesQuery(ScenarioId, TimelineId));

        _selfEmploymentItems = items.Select(e => new SelfEmploymentItemModel
        {
            Id = e.Id,
            Name = e.Name,
            GrossSalary = e.GrossSalary,
            GrossSalaryFrequencyId = e.GrossSalaryFrequencyId,
            NetSalary = e.NetSalary,
            NetSalaryFrequencyId = e.NetSalaryFrequencyId,
            GrossDividends = e.GrossDividends,
            GrossDividendsFrequencyId = e.GrossDividendsFrequencyId,
            NetDividends = e.NetDividends,
            NetDividendsFrequencyId = e.NetDividendsFrequencyId
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddSelfEmployment()
    {
        var result = await Mediator.Send(new CreateSelfEmploymentIncomeCommand(ScenarioId, TimelineId));
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
            GrossSalaryFrequencyId = item.GrossSalaryFrequencyId,
            NetSalary = item.NetSalary,
            NetSalaryFrequencyId = item.NetSalaryFrequencyId,
            GrossDividends = item.GrossDividends,
            GrossDividendsFrequencyId = item.GrossDividendsFrequencyId,
            NetDividends = item.NetDividends,
            NetDividendsFrequencyId = item.NetDividendsFrequencyId
        });
    }

    private void NavigateNext() => IncomeNavService.NavigateNext();
}

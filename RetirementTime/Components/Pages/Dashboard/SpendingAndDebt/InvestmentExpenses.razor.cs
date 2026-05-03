using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Spending.GetInvestmentExpenses;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Models.Spending;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.SpendingAndDebt;

public partial class InvestmentExpenses : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }
    [Parameter] public long TimelineId { get; set; }

    private bool _isLoading = true;
    private List<SpendingInvestmentExpenseItemModel> _items = [];
    private List<Frequency> _frequencies = [];
    private List<AssetsInvestmentAccount> _investmentAccounts = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var result = await Mediator.Send(new GetInvestmentExpensesQuery(ScenarioId, TimelineId));
        _frequencies       = result.Frequencies;
        _investmentAccounts = result.InvestmentAccounts;
        _items = result.Expenses.Select(e => new SpendingInvestmentExpenseItemModel
        {
            Id                  = e.Id,
            Amount              = e.Amount,
            FrequencyId         = e.FrequencyId,
            InvestmentAccountId = e.InvestmentAccountId,
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddItem()
    {
        var result = await Mediator.Send(new CreateInvestmentExpenseCommand(ScenarioId, TimelineId));
        if (result.Success)
            _items.Add(new SpendingInvestmentExpenseItemModel
            {
                Id          = result.ItemId,
                FrequencyId = (int)FrequencyEnum.Monthly,
            });
    }

    private async Task RemoveItem(SpendingInvestmentExpenseItemModel item)
    {
        await Mediator.Send(new DeleteInvestmentExpenseCommand(item.Id));
        _items.Remove(item);
    }

    private async Task SaveItem(SpendingInvestmentExpenseItemModel item)
    {
        if (item.Id == 0) return;
        await Mediator.Send(new UpdateInvestmentExpenseCommand
        {
            Id                  = item.Id,
            Amount              = item.Amount,
            FrequencyId         = item.FrequencyId,
            InvestmentAccountId = item.InvestmentAccountId,
        });
    }

    private void NavigateNext() =>
        Navigation.NavigateTo($"/scenario/{ScenarioId}/spending");
}

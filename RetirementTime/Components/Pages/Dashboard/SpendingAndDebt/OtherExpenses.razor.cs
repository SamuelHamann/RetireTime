using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Spending.GetOtherExpenses;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Models.Spending;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.SpendingAndDebt;

public partial class OtherExpenses : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private SpendingNavigationService SpendingNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }
    [Parameter] public long TimelineId { get; set; }

    private bool _isLoading = true;
    private List<SpendingOtherExpenseItemModel> _items = [];
    private List<Frequency> _frequencies = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var result = await Mediator.Send(new GetOtherExpensesQuery(ScenarioId, TimelineId));
        _frequencies = result.Frequencies;
        _items = result.Expenses.Select(e => new SpendingOtherExpenseItemModel
        {
            Id          = e.Id,
            Name        = e.Name,
            Amount      = e.Amount,
            FrequencyId = e.FrequencyId,
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddItem()
    {
        var result = await Mediator.Send(new CreateOtherExpenseCommand(ScenarioId, TimelineId));
        if (result.Success)
            _items.Add(new SpendingOtherExpenseItemModel
            {
                Id          = result.ItemId,
                FrequencyId = (int)FrequencyEnum.Monthly,
            });
    }

    private async Task RemoveItem(SpendingOtherExpenseItemModel item)
    {
        await Mediator.Send(new DeleteOtherExpenseCommand(item.Id));
        _items.Remove(item);
    }

    private async Task SaveItem(SpendingOtherExpenseItemModel item)
    {
        if (item.Id == 0) return;
        await Mediator.Send(new UpdateOtherExpenseCommand
        {
            Id          = item.Id,
            Name        = item.Name,
            Amount      = item.Amount,
            FrequencyId = item.FrequencyId,
        });
    }

    private void NavigateNext() =>
        Navigation.NavigateTo($"/scenario/{ScenarioId}/spending");
}

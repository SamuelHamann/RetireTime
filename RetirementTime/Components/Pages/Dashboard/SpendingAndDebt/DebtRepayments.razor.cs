using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Spending.GetDebtRepayments;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Models.Spending;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.SpendingAndDebt;

public partial class DebtRepayments : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private SpendingNavigationService SpendingNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private List<SpendingDebtRepaymentItemModel> _linkedItems = [];
    private List<SpendingDebtRepaymentItemModel> _standaloneItems = [];
    private List<Frequency> _frequencies = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var result = await Mediator.Send(new GetDebtRepaymentsQuery(ScenarioId));
        _frequencies = result.Frequencies;

        var existingRepayments = result.Repayments;

        // Ensure one repayment exists per debt (auto-create missing ones)
        foreach (var debt in result.Debts)
        {
            var existing = existingRepayments.FirstOrDefault(r => r.GenericDebtId == debt.Id);
            if (existing == null)
            {
                var created = await Mediator.Send(new CreateDebtRepaymentCommand(ScenarioId, debt.Id));
                if (created.Success)
                {
                    _linkedItems.Add(new SpendingDebtRepaymentItemModel
                    {
                        Id           = created.ItemId,
                        GenericDebtId = debt.Id,
                        Name         = debt.Name,
                        FrequencyId  = (int)FrequencyEnum.Monthly,
                    });
                }
            }
            else
            {
                _linkedItems.Add(new SpendingDebtRepaymentItemModel
                {
                    Id           = existing.Id,
                    GenericDebtId = existing.GenericDebtId,
                    Name         = debt.Name,
                    Amount       = existing.Amount,
                    FrequencyId  = existing.FrequencyId,
                });
            }
        }

        _standaloneItems = existingRepayments
            .Where(r => r.GenericDebtId == null)
            .Select(r => new SpendingDebtRepaymentItemModel
            {
                Id          = r.Id,
                Name        = r.Name,
                Amount      = r.Amount,
                FrequencyId = r.FrequencyId,
            }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task SaveLinked(SpendingDebtRepaymentItemModel item)
    {
        await Mediator.Send(new UpdateDebtRepaymentCommand
        {
            Id           = item.Id,
            Name         = item.Name,
            Amount       = item.Amount,
            FrequencyId  = item.FrequencyId,
            GenericDebtId = item.GenericDebtId,
        });
    }

    private async Task AddStandalone()
    {
        var result = await Mediator.Send(new CreateDebtRepaymentCommand(ScenarioId));
        if (result.Success)
            _standaloneItems.Add(new SpendingDebtRepaymentItemModel
            {
                Id          = result.ItemId,
                FrequencyId = (int)FrequencyEnum.Monthly,
            });
    }

    private async Task RemoveStandalone(SpendingDebtRepaymentItemModel item)
    {
        await Mediator.Send(new DeleteDebtRepaymentCommand(item.Id));
        _standaloneItems.Remove(item);
    }

    private async Task SaveStandalone(SpendingDebtRepaymentItemModel item)
    {
        if (item.Id == 0) return;
        await Mediator.Send(new UpdateDebtRepaymentCommand
        {
            Id          = item.Id,
            Name        = item.Name,
            Amount      = item.Amount,
            FrequencyId = item.FrequencyId,
        });
    }

    private void NavigateNext() =>
        Navigation.NavigateTo($"/scenario/{ScenarioId}/spending/assets-expenses");
}

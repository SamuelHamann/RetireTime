using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Debt.CreateDebt;
using RetirementTime.Application.Features.Dashboard.Debt.DeleteDebt;
using RetirementTime.Application.Features.Dashboard.Debt.GetDebts;
using RetirementTime.Application.Features.Dashboard.Debt.UpdateDebt;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Models.Debt;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.SpendingAndDebt;

public partial class PersonalLoans : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private DebtNavigationService DebtNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    // All types that don't have dedicated pages (CarLoan now has its own page)
    private static readonly long[] DebtTypeIds =
    [
        (long)DebtTypeEnum.HomeEquityLineOfCredit,
        (long)DebtTypeEnum.PersonalLoan,
        (long)DebtTypeEnum.LineOfCredit,
        (long)DebtTypeEnum.Other
    ];

    private bool _isLoading = true;
    private List<GenericDebtItemModel> _items = [];
    private List<Frequency> _frequencies = [];
    private List<DebtType> _debtTypes = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var result = await Mediator.Send(new GetDebtsQuery(ScenarioId, DebtTypeIds));
        _frequencies = result.Frequencies;
        _debtTypes = result.DebtTypes;
        _items = result.Debts.Select(d => new GenericDebtItemModel
        {
            Id = d.Id, Name = d.Name, DebtTypeId = d.DebtTypeId,
            Balance = d.Balance, InterestRate = d.InterestRate,
            FrequencyId = d.FrequencyId, TermInYears = d.TermInYears
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddItem()
    {
        var result = await Mediator.Send(new CreateDebtCommand(ScenarioId, (long)DebtTypeEnum.PersonalLoan));
        if (result.Success)
            _items.Add(new GenericDebtItemModel
            {
                Id = result.DebtId, DebtTypeId = (long)DebtTypeEnum.PersonalLoan,
                FrequencyId = (int)FrequencyEnum.Annually
            });
    }

    private async Task RemoveItem(GenericDebtItemModel item)
    {
        await Mediator.Send(new DeleteDebtCommand(item.Id));
        _items.Remove(item);
    }

    private async Task SaveItem(GenericDebtItemModel item)
    {
        if (item.Id == 0) return;
        await Mediator.Send(new UpdateDebtCommand
        {
            Id = item.Id, Name = item.Name, DebtTypeId = item.DebtTypeId,
            Balance = item.Balance, InterestRate = item.InterestRate,
            FrequencyId = item.FrequencyId, TermInYears = item.TermInYears
        });
    }

    private void NavigateNext() => DebtNavService.NavigateNext();
}

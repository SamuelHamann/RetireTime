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

public partial class StudentLoans : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private DebtNavigationService DebtNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private static readonly long[] DebtTypeIds = [(long)DebtTypeEnum.StudentLoan];

    private bool _isLoading = true;
    private List<GenericDebtItemModel> _items = [];
    private List<Frequency> _frequencies = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var result = await Mediator.Send(new GetDebtsQuery(ScenarioId, DebtTypeIds));
        _frequencies = result.Frequencies;
        _items = result.Debts.Select(d => new GenericDebtItemModel
        {
            Id = d.Id, Name = d.Name, DebtTypeId = d.DebtTypeId,
            Balance = d.Balance, InterestRate = d.InterestRate, FrequencyId = d.FrequencyId
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddItem()
    {
        var result = await Mediator.Send(new CreateDebtCommand(ScenarioId, (long)DebtTypeEnum.StudentLoan));
        if (result.Success)
            _items.Add(new GenericDebtItemModel
            {
                Id = result.DebtId, Name = "Student Loan", DebtTypeId = (long)DebtTypeEnum.StudentLoan,
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
            Id = item.Id, Name = "Student Loan", DebtTypeId = item.DebtTypeId,
            Balance = item.Balance, InterestRate = item.InterestRate, FrequencyId = item.FrequencyId
        });
    }

    private void NavigateNext() => DebtNavService.NavigateNext();
}

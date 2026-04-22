using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.PersistingIncome.CreateOtherPersistingIncome;
using RetirementTime.Application.Features.Dashboard.PersistingIncome.DeleteOtherPersistingIncome;
using RetirementTime.Application.Features.Dashboard.PersistingIncome.GetOtherPersistingIncomes;
using RetirementTime.Application.Features.Dashboard.PersistingIncome.UpdateOtherPersistingIncome;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Models.Income;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class OtherPersistingIncome : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private List<OtherPersistingIncomeItemModel> _incomeItems = [];
    private List<Frequency> _frequencies = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var result = await Mediator.Send(new GetOtherPersistingIncomesQuery(ScenarioId));
        _frequencies = result.Frequencies;
        _incomeItems = result.Items.Select(e => new OtherPersistingIncomeItemModel
        {
            Id = e.Id,
            Name = e.Name,
            Amount = e.Amount,
            FrequencyId = e.FrequencyId,
            Taxable = e.Taxable
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddIncome()
    {
        var result = await Mediator.Send(new CreateOtherPersistingIncomeCommand(ScenarioId));
        if (result.Success) _incomeItems.Add(new OtherPersistingIncomeItemModel
        {
            Id = result.IncomeId,
            FrequencyId = (int)FrequencyEnum.Annually
        });
    }

    private async Task RemoveIncome(OtherPersistingIncomeItemModel item)
    {
        await Mediator.Send(new DeleteOtherPersistingIncomeCommand(item.Id));
        _incomeItems.Remove(item);
    }

    private async Task SaveIncome(OtherPersistingIncomeItemModel item)
    {
        if (item.Id == 0) return;
        await Mediator.Send(new UpdateOtherPersistingIncomeCommand
        {
            Id = item.Id,
            Name = item.Name,
            Amount = item.Amount,
            FrequencyId = item.FrequencyId,
            Taxable = item.Taxable
        });
    }
}

using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Income.CreatePropertyIncome;
using RetirementTime.Application.Features.Dashboard.Income.DeletePropertyIncome;
using RetirementTime.Application.Features.Dashboard.Income.GetPropertyIncome;
using RetirementTime.Application.Features.Dashboard.Income.UpdatePropertyIncome;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Interfaces.Repositories;
using RetirementTime.Models.Income;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class PropertyIncome : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private IFrequencyRepository FrequencyRepository { get; set; } = default!;
    [Inject] private IAssetsInvestmentPropertyRepository PropertyRepository { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }
    [Parameter] public long TimelineId { get; set; }

    private bool _isLoading = true;
    private List<PropertyIncomeItemModel> _incomeItems = [];
    private List<Frequency> _frequencies = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        _frequencies = await FrequencyRepository.GetFrequencies();

        // Fetch existing investment properties and create pre-filled entries
        var existingProperties = await PropertyRepository.GetByScenarioIdAsync(ScenarioId);

        // Fetch already-saved property income entries for this timeline
        var savedItems = await Mediator.Send(new GetPropertyIncomeQuery(ScenarioId, TimelineId));
        var savedIds   = savedItems.Select(e => e.InvestmentPropertyId).ToHashSet();

        // Auto-create entries for investment properties not yet in the list
        foreach (var prop in existingProperties.Where(p => !savedIds.Contains(p.Id)))
        {
            var result = await Mediator.Send(new CreatePropertyIncomeCommand(ScenarioId, TimelineId, prop.Id, prop.Name));
            if (result.Success)
                savedItems.Add(new Domain.Entities.Dashboard.Income.PropertyIncome
                {
                    Id                   = result.IncomeId,
                    InvestmentPropertyId = prop.Id,
                    Name                 = prop.Name,
                    FrequencyId          = (int)FrequencyEnum.Monthly,
                });
        }

        _incomeItems = savedItems.Select(e => new PropertyIncomeItemModel
        {
            Id                   = e.Id,
            InvestmentPropertyId = e.InvestmentPropertyId,
            Name                 = e.Name,
            Amount               = e.Amount,
            FrequencyId          = e.FrequencyId,
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddIncome()
    {
        var defaultFreqId = _frequencies.FirstOrDefault()?.Id ?? (int)FrequencyEnum.Monthly;
        var result = await Mediator.Send(new CreatePropertyIncomeCommand(ScenarioId, TimelineId, null, string.Empty));
        if (result.Success)
            _incomeItems.Add(new PropertyIncomeItemModel
            {
                Id          = result.IncomeId,
                FrequencyId = defaultFreqId,
            });
    }

    private async Task RemoveIncome(PropertyIncomeItemModel item)
    {
        await Mediator.Send(new DeletePropertyIncomeCommand(item.Id));
        _incomeItems.Remove(item);
    }

    private async Task SaveIncome(PropertyIncomeItemModel item)
    {
        if (item.Id == 0) return;
        await Mediator.Send(new UpdatePropertyIncomeCommand
        {
            Id          = item.Id,
            Name        = item.Name,
            Amount      = item.Amount,
            FrequencyId = item.FrequencyId,
        });
    }
}


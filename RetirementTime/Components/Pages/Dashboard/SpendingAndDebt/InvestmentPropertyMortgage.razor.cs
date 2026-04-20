using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Asset.GetAssetsInvestmentProperties;
using RetirementTime.Application.Features.Dashboard.Debt.CreateDebt;
using RetirementTime.Application.Features.Dashboard.Debt.DeleteDebt;
using RetirementTime.Application.Features.Dashboard.Debt.GetDebts;
using RetirementTime.Application.Features.Dashboard.Debt.UpdateDebt;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Models.Debt;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.SpendingAndDebt;

public partial class InvestmentPropertyMortgage : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private DebtNavigationService DebtNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private static readonly long[] DebtTypeIds = [(long)DebtTypeEnum.Mortgage];

    private bool _isLoading = true;
    private List<AssetsInvestmentProperty> _properties = [];
    // One debt model per linked asset (key = AssetId); lazy-created on first save
    private Dictionary<long, GenericDebtItemModel> _linkedByAsset = [];
    // Standalone debts (no linked asset)
    private List<GenericDebtItemModel> _standalone = [];
    private List<Frequency> _frequencies = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var debtResult = await Mediator.Send(new GetDebtsQuery(ScenarioId, DebtTypeIds));
        var properties  = await Mediator.Send(new GetAssetsInvestmentPropertiesQuery(ScenarioId));

        _properties  = properties;
        _frequencies = debtResult.Frequencies;

        var existingDebts = debtResult.Debts.Select(d => new GenericDebtItemModel
        {
            Id             = d.Id,
            Name           = d.Name,
            DebtTypeId     = d.DebtTypeId,
            Balance        = d.Balance,
            InterestRate   = d.InterestRate,
            FrequencyId    = d.FrequencyId,
            TermInYears    = d.TermInYears,
            LinkedAssetId  = d.DebtAgainstAssetId,
            LinkedAssetName = properties.FirstOrDefault(p => p.Id == d.DebtAgainstAssetId)?.Name
        }).ToList();

        // One slot per property: use existing linked debt or a fresh unsaved model
        foreach (var property in _properties)
        {
            var existing = existingDebts.FirstOrDefault(d => d.LinkedAssetId == property.Id);
            _linkedByAsset[property.Id] = existing ?? new GenericDebtItemModel
            {
                Id            = 0,
                DebtTypeId    = (long)DebtTypeEnum.Mortgage,
                FrequencyId   = (int)FrequencyEnum.Annually,
                LinkedAssetId = property.Id,
                LinkedAssetName = property.Name,
                Name          = $"{property.Name} Mortgage"
            };
        }

        _standalone = existingDebts.Where(d => !d.LinkedAssetId.HasValue).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    public GenericDebtItemModel GetLinkedDebt(long propertyId) =>
        _linkedByAsset.TryGetValue(propertyId, out var m) ? m : new();

    public List<GenericDebtItemModel> GetStandalone() => _standalone;

    // Linked debts: lazy-create on first field change, then update
    private async Task SaveLinkedItem(GenericDebtItemModel item)
    {
        if (item.Id == 0)
        {
            var created = await Mediator.Send(new CreateDebtCommand(ScenarioId, (long)DebtTypeEnum.Mortgage, item.LinkedAssetId));
            if (!created.Success) return;
            item.Id = created.DebtId;
        }
        await Mediator.Send(new UpdateDebtCommand
        {
            Id                 = item.Id,
            Name               = $"{item.LinkedAssetName} Mortgage",
            DebtTypeId         = item.DebtTypeId,
            Balance            = item.Balance,
            InterestRate       = item.InterestRate,
            FrequencyId        = item.FrequencyId,
            TermInYears        = item.TermInYears,
            DebtAgainstAssetId = item.LinkedAssetId
        });
    }

    private async Task AddStandalone()
    {
        var result = await Mediator.Send(new CreateDebtCommand(ScenarioId, (long)DebtTypeEnum.Mortgage));
        if (result.Success)
            _standalone.Add(new GenericDebtItemModel
            {
                Id         = result.DebtId,
                DebtTypeId = (long)DebtTypeEnum.Mortgage,
                FrequencyId = (int)FrequencyEnum.Annually
            });
    }

    private async Task RemoveStandalone(GenericDebtItemModel item)
    {
        await Mediator.Send(new DeleteDebtCommand(item.Id));
        _standalone.Remove(item);
    }

    private async Task SaveStandalone(GenericDebtItemModel item)
    {
        if (item.Id == 0) return;
        await Mediator.Send(new UpdateDebtCommand
        {
            Id           = item.Id,
            Name         = item.Name,
            DebtTypeId   = item.DebtTypeId,
            Balance      = item.Balance,
            InterestRate = item.InterestRate,
            FrequencyId  = item.FrequencyId,
            TermInYears  = item.TermInYears
        });
    }

    private void NavigateNext() => DebtNavService.NavigateNext();
}

using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Asset.GetPhysicalAssets;
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

public partial class CarLoans : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private DebtNavigationService DebtNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private static readonly long[] DebtTypeIds = [(long)DebtTypeEnum.CarLoan];

    private bool _isLoading = true;
    private List<AssetsPhysicalAsset> _vehicles = [];
    // One debt model per linked vehicle (key = AssetId); lazy-created on first save
    private Dictionary<long, GenericDebtItemModel> _linkedByVehicle = [];
    // Standalone car loans (no linked vehicle)
    private List<GenericDebtItemModel> _standalone = [];
    private List<Frequency> _frequencies = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var debtResult          = await Mediator.Send(new GetDebtsQuery(ScenarioId, DebtTypeIds));
        var physicalAssetsResult = await Mediator.Send(new GetPhysicalAssetsQuery(ScenarioId));

        _vehicles = physicalAssetsResult.Assets
            .Where(a => a.AssetTypeId == (long)AssetTypeEnum.Vehicle)
            .ToList();

        _frequencies = debtResult.Frequencies;

        var existingDebts = debtResult.Debts.Select(d => new GenericDebtItemModel
        {
            Id              = d.Id,
            Name            = d.Name,
            DebtTypeId      = d.DebtTypeId,
            Balance         = d.Balance,
            InterestRate    = d.InterestRate,
            FrequencyId     = d.FrequencyId,
            LinkedAssetId   = d.DebtAgainstAssetId,
            LinkedAssetName = _vehicles.FirstOrDefault(v => v.Id == d.DebtAgainstAssetId)?.Name
        }).ToList();

        // One slot per vehicle: use existing linked debt or a fresh unsaved model
        foreach (var vehicle in _vehicles)
        {
            var existing = existingDebts.FirstOrDefault(d => d.LinkedAssetId == vehicle.Id);
            _linkedByVehicle[vehicle.Id] = existing ?? new GenericDebtItemModel
            {
                Id              = 0,
                DebtTypeId      = (long)DebtTypeEnum.CarLoan,
                FrequencyId     = (int)FrequencyEnum.Annually,
                LinkedAssetId   = vehicle.Id,
                LinkedAssetName = vehicle.Name,
                Name            = $"{vehicle.Name} Loan"
            };
        }

        _standalone = existingDebts.Where(d => !d.LinkedAssetId.HasValue).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    public GenericDebtItemModel GetLinkedDebt(long vehicleId) =>
        _linkedByVehicle.TryGetValue(vehicleId, out var m) ? m : new();

    public List<GenericDebtItemModel> GetStandalone() => _standalone;

    // Linked debts: lazy-create on first field change, then update
    private async Task SaveLinkedItem(GenericDebtItemModel item)
    {
        if (item.Id == 0)
        {
            var created = await Mediator.Send(new CreateDebtCommand(ScenarioId, (long)DebtTypeEnum.CarLoan, item.LinkedAssetId));
            if (!created.Success) return;
            item.Id = created.DebtId;
        }
        await Mediator.Send(new UpdateDebtCommand
        {
            Id                 = item.Id,
            Name               = $"{item.LinkedAssetName} Loan",
            DebtTypeId         = item.DebtTypeId,
            Balance            = item.Balance,
            InterestRate       = item.InterestRate,
            FrequencyId        = item.FrequencyId,
            DebtAgainstAssetId = item.LinkedAssetId
        });
    }

    private async Task AddStandalone()
    {
        var result = await Mediator.Send(new CreateDebtCommand(ScenarioId, (long)DebtTypeEnum.CarLoan));
        if (result.Success)
            _standalone.Add(new GenericDebtItemModel
            {
                Id         = result.DebtId,
                DebtTypeId = (long)DebtTypeEnum.CarLoan,
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
            FrequencyId  = item.FrequencyId
        });
    }

    private void NavigateNext() => DebtNavService.NavigateNext();
}

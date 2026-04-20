using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Asset.GetAssetsHome;
using RetirementTime.Application.Features.Dashboard.Debt.GetHomeMortgage;
using RetirementTime.Application.Features.Dashboard.Debt.SaveHomeMortgage;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Models.Debt;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.SpendingAndDebt;

public partial class HomeMortgage : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private DebtNavigationService DebtNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private HomeMortgageModel _model = new();
    private List<Frequency> _frequencies = [];
    private AssetsHome? _homeAsset;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var mortgageResult = await Mediator.Send(new GetHomeMortgageQuery(ScenarioId));
        _homeAsset   = await Mediator.Send(new GetAssetsHomeQuery(ScenarioId));
        _frequencies = mortgageResult.Frequencies;

        if (mortgageResult.Debt != null)
        {
            _model.Balance      = mortgageResult.Debt.Balance;
            _model.InterestRate = mortgageResult.Debt.InterestRate;
            _model.FrequencyId  = mortgageResult.Debt.FrequencyId;
            _model.TermInYears  = mortgageResult.Debt.TermInYears;
        }
        else
        {
            _model.FrequencyId = (int)FrequencyEnum.Annually;
        }

        _isLoading = false;
        StateHasChanged();
    }

    private async Task Save()
    {
        await Mediator.Send(new SaveHomeMortgageCommand
        {
            ScenarioId         = ScenarioId,
            Balance            = _model.Balance,
            InterestRate       = _model.InterestRate,
            FrequencyId        = _model.FrequencyId,
            TermInYears        = _model.TermInYears,
            DebtAgainstAssetId = _homeAsset?.Id
        });
    }

    private void NavigateNext() => DebtNavService.NavigateNext();
}

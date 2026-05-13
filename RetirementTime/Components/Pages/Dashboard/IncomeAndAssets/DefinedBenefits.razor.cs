using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Income.CreatePensionDefinedBenefits;
using RetirementTime.Application.Features.Dashboard.Income.DeletePensionDefinedBenefits;
using RetirementTime.Application.Features.Dashboard.Income.GetPensionDefinedBenefits;
using RetirementTime.Application.Features.Dashboard.Income.UpdatePensionDefinedBenefits;
using RetirementTime.Models.Income;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class DefinedBenefits : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private IncomeNavigationService IncomeNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }
    [Parameter] public long TimelineId { get; set; }

    private bool _isLoading = true;
    private List<PensionDefinedBenefitsItemModel> _pensionItems = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null)
        {
            Navigation.NavigateTo("/");
            return;
        }

        var items = await Mediator.Send(new GetPensionDefinedBenefitsQuery(ScenarioId, TimelineId));

        _pensionItems = items.Select(e => new PensionDefinedBenefitsItemModel
        {
            Id = e.Id,
            Name = e.Name,
            StartAge = e.StartAge,
            BenefitsPre65 = e.BenefitsPre65,
            BenefitsPost65 = e.BenefitsPost65,
            PercentIndexedToInflation = e.PercentIndexedToInflation,
            PercentSurvivorBenefits = e.PercentSurvivorBenefits,
            RrspAdjustment = e.RrspAdjustment
        }).ToList();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task AddPension()
    {
        var result = await Mediator.Send(new CreatePensionDefinedBenefitsCommand(ScenarioId, TimelineId));
        if (result.Success)
        {
            _pensionItems.Add(new PensionDefinedBenefitsItemModel { Id = result.PensionId });
        }
    }

    private async Task RemovePension(PensionDefinedBenefitsItemModel item)
    {
        await Mediator.Send(new DeletePensionDefinedBenefitsCommand(item.Id));
        _pensionItems.Remove(item);
    }

    private async Task SavePension(PensionDefinedBenefitsItemModel item)
    {
        if (item.Id == 0) return;

        await Mediator.Send(new UpdatePensionDefinedBenefitsCommand
        {
            Id = item.Id,
            Name = item.Name,
            StartAge = item.StartAge,
            BenefitsPre65 = item.BenefitsPre65,
            BenefitsPost65 = item.BenefitsPost65,
            PercentIndexedToInflation = item.PercentIndexedToInflation,
            PercentSurvivorBenefits = item.PercentSurvivorBenefits,
            RrspAdjustment = item.RrspAdjustment
        });
    }

    private void NavigateNext() => IncomeNavService.NavigateNext();
}

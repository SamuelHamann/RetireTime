using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Income.GetOasCppIncome;
using RetirementTime.Application.Features.Dashboard.Income.SaveOasCppIncome;
using RetirementTime.Models.Income;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class OasCpp : ComponentBase
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
    private OasCppModel _model = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var existing = await Mediator.Send(new GetOasCppIncomeQuery(ScenarioId, TimelineId));
        if (existing != null)
        {
            _model = new OasCppModel
            {
                IncomeLastYear = existing.IncomeLastYear,
                Income2YearsAgo = existing.Income2YearsAgo,
                Income3YearsAgo = existing.Income3YearsAgo,
                Income4YearsAgo = existing.Income4YearsAgo,
                Income5YearsAgo = existing.Income5YearsAgo,
                YearsSpentInCanada = existing.YearsSpentInCanada
            };
        }

        _isLoading = false;
        StateHasChanged();
    }

    private async Task SaveAsync()
    {
        await Mediator.Send(new SaveOasCppIncomeCommand
        {
            ScenarioId = ScenarioId,
            TimelineId = TimelineId,
            IncomeLastYear = _model.IncomeLastYear,
            Income2YearsAgo = _model.Income2YearsAgo,
            Income3YearsAgo = _model.Income3YearsAgo,
            Income4YearsAgo = _model.Income4YearsAgo,
            Income5YearsAgo = _model.Income5YearsAgo,
            YearsSpentInCanada = _model.YearsSpentInCanada
        });
    }

    private void NavigateNext() => IncomeNavService.NavigateNext();
}

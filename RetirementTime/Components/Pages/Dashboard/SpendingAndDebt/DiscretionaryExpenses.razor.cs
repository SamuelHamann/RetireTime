using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.Spending.GetDiscretionaryExpenses;
using RetirementTime.Application.Features.Dashboard.Spending.SaveDiscretionaryExpenses;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Models.Spending;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.SpendingAndDebt;

public partial class DiscretionaryExpenses : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private SpendingNavigationService SpendingNavService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private bool _isLoading = true;
    private SpendingDiscretionaryExpensesModel _model = new();
    private List<Frequency> _frequencies = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var result = await Mediator.Send(new GetDiscretionaryExpensesQuery(ScenarioId));
        _frequencies = result.Frequencies;

        if (result.Expenses is { } e)
        {
            _model.GymMembership                         = e.GymMembership;
            _model.GymMembershipFrequencyId              = e.GymMembershipFrequencyId;
            _model.Subscriptions                         = e.Subscriptions;
            _model.SubscriptionsFrequencyId              = e.SubscriptionsFrequencyId;
            _model.EatingOut                             = e.EatingOut;
            _model.EatingOutFrequencyId                  = e.EatingOutFrequencyId;
            _model.Entertainment                         = e.Entertainment;
            _model.EntertainmentFrequencyId              = e.EntertainmentFrequencyId;
            _model.Travel                                = e.Travel;
            _model.TravelFrequencyId                     = e.TravelFrequencyId;
            _model.CharitableDonations                   = e.CharitableDonations;
            _model.CharitableDonationsFrequencyId        = e.CharitableDonationsFrequencyId;
            _model.OtherDiscretionaryExpenses            = e.OtherDiscretionaryExpenses;
            _model.OtherDiscretionaryExpensesFrequencyId = e.OtherDiscretionaryExpensesFrequencyId;
        }

        _isLoading = false;
        StateHasChanged();
    }

    private async Task Save()
    {
        await Mediator.Send(new SaveDiscretionaryExpensesCommand
        {
            ScenarioId                         = ScenarioId,
            GymMembership                      = _model.GymMembership,
            GymMembershipFrequencyId           = _model.GymMembershipFrequencyId,
            Subscriptions                      = _model.Subscriptions,
            SubscriptionsFrequencyId           = _model.SubscriptionsFrequencyId,
            EatingOut                          = _model.EatingOut,
            EatingOutFrequencyId               = _model.EatingOutFrequencyId,
            Entertainment                      = _model.Entertainment,
            EntertainmentFrequencyId           = _model.EntertainmentFrequencyId,
            Travel                             = _model.Travel,
            TravelFrequencyId                  = _model.TravelFrequencyId,
            CharitableDonations                = _model.CharitableDonations,
            CharitableDonationsFrequencyId     = _model.CharitableDonationsFrequencyId,
            OtherDiscretionaryExpenses         = _model.OtherDiscretionaryExpenses,
            OtherDiscretionaryExpensesFrequencyId = _model.OtherDiscretionaryExpensesFrequencyId,
        });
    }

    private void NavigateNext() =>
        Navigation.NavigateTo($"/scenario/{ScenarioId}/spending/debt-repayments");
}

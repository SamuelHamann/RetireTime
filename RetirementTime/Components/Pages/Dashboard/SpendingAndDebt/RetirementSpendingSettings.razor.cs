using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.RetirementSpending.DeleteRetirementSpending;
using RetirementTime.Application.Features.Dashboard.RetirementSpending.GetRetirementSpendings;
using RetirementTime.Application.Features.Dashboard.RetirementSpending.UpdateRetirementSpending;
using RetirementTime.Models.Spending;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.SpendingAndDebt;

public partial class RetirementSpendingSettings : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }
    [Parameter] public long RetirementSpendingId { get; set; }

    private bool _isLoading = true;
    private bool _isSaving;
    private bool _isDeleting;
    private string _successMessage = string.Empty;
    private string _errorMessage = string.Empty;
    private RetirementSpendingModel _model = new();
    private List<RetirementSpendingDto> _otherSpendings = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var all = await Mediator.Send(new GetRetirementSpendingsQuery(ScenarioId));
        _otherSpendings = all.Where(e => e.Id != RetirementSpendingId).ToList();

        var current = all.FirstOrDefault(e => e.Id == RetirementSpendingId);
        if (current is not null)
        {
            _model = new RetirementSpendingModel
            {
                Id             = current.Id,
                Name           = current.Name,
                AgeFrom        = current.AgeFrom,
                AgeTo          = current.AgeTo,
                IsFullyCreated = current.IsFullyCreated
            };
        }

        _isLoading = false;
        StateHasChanged();
    }

    private async Task Save()
    {
        if (_isSaving) return;
        _isSaving = true;
        _successMessage = string.Empty;
        _errorMessage = string.Empty;

        var result = await Mediator.Send(new UpdateRetirementSpendingCommand
        {
            Id             = RetirementSpendingId,
            ScenarioId     = ScenarioId,
            Name           = _model.Name,
            AgeFrom        = _model.AgeFrom,
            AgeTo          = _model.AgeTo,
            IsFullyCreated = true  // mark fully created on first save
        });

        if (result.Success)
        {
            _model.IsFullyCreated = true;
            _successMessage = Localizer["RetirementSpending_SavedSuccess"];
        }
        else
            _errorMessage = result.ErrorMessage ?? Localizer["RetirementSpending_SaveError"];

        _isSaving = false;
        StateHasChanged();
    }

    private async Task Delete()
    {
        if (_isDeleting) return;
        _isDeleting = true;

        var result = await Mediator.Send(new DeleteRetirementSpendingCommand(RetirementSpendingId));

        if (result.Success)
            Navigation.NavigateTo($"/scenario/{ScenarioId}/settings", forceLoad: false);
        else
        {
            _errorMessage = result.ErrorMessage ?? Localizer["RetirementSpending_DeleteError"];
            _isDeleting = false;
            StateHasChanged();
        }
    }
}

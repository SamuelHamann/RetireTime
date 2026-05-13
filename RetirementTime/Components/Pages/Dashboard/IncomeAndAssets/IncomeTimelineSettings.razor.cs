using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using RetirementTime.Application.Features.Dashboard.RetirementSpending.DeleteRetirementSpending;
using RetirementTime.Application.Features.Dashboard.RetirementSpending.GetRetirementSpendings;
using RetirementTime.Application.Features.Dashboard.RetirementSpending.SaveExpenseTimeline;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Models.Spending;
using RetirementTime.Resources.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class IncomeTimelineSettings : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IMediator Mediator { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }
    [Parameter] public long TimelineId { get; set; }

    private bool _isLoading = true;
    private bool _isSaving;
    private bool _isDeleting;
    private string _errorMessage = string.Empty;
    private RetirementSpendingModel _model = new();
    private List<RetirementSpendingDto> _otherTimelines = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null) { Navigation.NavigateTo("/"); return; }

        var all = await Mediator.Send(new GetRetirementSpendingsQuery(ScenarioId));
        // Only show other Income timelines as clone options
        _otherTimelines = all.Where(e => e.Id != TimelineId && e.TimelineType == RetirementTimelineTypeEnum.Income).ToList();

        var current = all.FirstOrDefault(e => e.Id == TimelineId);
        if (current is not null)
        {
            _model = new RetirementSpendingModel
            {
                Id             = current.Id,
                Name           = current.Name,
                AgeFrom        = current.AgeFrom,
                AgeTo          = current.AgeTo,
                TimelineType   = current.TimelineType,
                IsFullyCreated = current.IsFullyCreated,
            };
        }

        _isLoading = false;
        StateHasChanged();
    }

    private async Task Save()
    {
        if (_isSaving) return;
        _isSaving = true;
        _errorMessage = string.Empty;

        var result = await Mediator.Send(new SaveExpenseTimelineCommand
        {
            Id           = TimelineId,
            ScenarioId   = ScenarioId,
            Name         = _model.Name,
            AgeFrom      = _model.AgeFrom,
            AgeTo        = _model.AgeTo,
            TimelineType = RetirementTimelineTypeEnum.Income,
            IsFirstSave  = !_model.IsFullyCreated,
            CloneFromTimelineId = _model.CloneFromId > 0 ? _model.CloneFromId : null,
        });

        if (result.Success)
            Navigation.NavigateTo($"/scenario/{ScenarioId}/income/timeline/{TimelineId}/employment", forceLoad: false);
        else
        {
            _errorMessage = result.ErrorMessage ?? Localizer["IncomeTimeline_SaveError"];
            _isSaving = false;
            StateHasChanged();
        }
    }

    private async Task Delete()
    {
        if (_isDeleting) return;
        _isDeleting = true;

        var result = await Mediator.Send(new DeleteRetirementSpendingCommand(TimelineId));

        if (result.Success)
            Navigation.NavigateTo($"/scenario/{ScenarioId}/income", forceLoad: false);
        else
        {
            _errorMessage = result.ErrorMessage ?? Localizer["IncomeTimeline_DeleteError"];
            _isDeleting = false;
            StateHasChanged();
        }
    }
}


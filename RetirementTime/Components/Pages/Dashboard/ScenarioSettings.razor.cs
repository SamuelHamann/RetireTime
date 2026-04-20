using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Application.Features.Dashboard.DeleteScenario;
using RetirementTime.Application.Features.Dashboard.GetScenarios;
using RetirementTime.Application.Features.Dashboard.UpdateScenario;
using RetirementTime.Models.Dashboard;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard;

public partial class ScenarioSettings : ComponentBase, IDisposable
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private AuthService AuthService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private DelayedLoadingService LoadingService { get; set; } = default!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

    [Parameter] public long ScenarioId { get; set; }

    private ScenarioSettingsModel _model = new();
    private List<Application.Features.Dashboard.Common.ScenarioDto> _otherScenarios = [];
    private bool _isSaving = false;
    private bool _isDeleting = false;
    private bool _scenarioFound = true;
    private bool _scenarioFullyCreated = false;
    private string? _successMessage;
    private string? _errorMessage;
    private long _userId;

    protected override async Task OnInitializedAsync()
    {
        LoadingService.StartLoading(StateHasChanged, delayMs: 500);

        var authenticatedUser = await AuthService.GetAuthenticatedUserAsync(AuthenticationState);
        if (authenticatedUser == null)
        {
            Navigation.NavigateTo("/");
            return;
        }

        _userId = authenticatedUser.UserId;

        await LoadScenario();
        await LoadOtherScenarios();

        // Stop loading when done
        LoadingService.StopLoading();
    }

    protected override async Task OnParametersSetAsync()
    {
        // Reload when ScenarioId changes
        if (!LoadingService.IsLoading && ScenarioId > 0)
        {
            LoadingService.StartLoading(StateHasChanged, delayMs: 150);

            await LoadScenario();
            await LoadOtherScenarios();

            LoadingService.StopLoading();
        }
    }

    private async Task LoadScenario()
    {
        var result = await Mediator.Send(new GetScenariosQuery { UserId = _userId });

        if (result.Success)
        {
            var scenario = result.Scenarios.FirstOrDefault(s => s.ScenarioId == ScenarioId);

            if (scenario != null)
            {
                _model.ScenarioName = scenario.ScenarioName;
                _scenarioFullyCreated = scenario.ScenarioFullyCreated;
                _scenarioFound = true;
            }
            else
            {
                _scenarioFound = false;
            }
        }
    }

    private async Task LoadOtherScenarios()
    {
        var result = await Mediator.Send(new GetScenariosQuery { UserId = _userId });

        if (result.Success)
        {
            _otherScenarios = result.Scenarios.Where(s => s.ScenarioId != ScenarioId).ToList();
        }
    }

    private async Task HandleSave()
    {
        _isSaving = true;
        _successMessage = null;
        _errorMessage = null;
        var isFirstSave = !_scenarioFullyCreated;

        try
        {
            // Update scenario name
            var updateCommand = new UpdateScenarioCommand
            {
                ScenarioId = ScenarioId,
                UserId = _userId,
                ScenarioName = _model.ScenarioName
            };

            var updateResult = await Mediator.Send(updateCommand);

            if (!updateResult.Success)
            {
                _errorMessage = updateResult.ErrorMessage ?? "Failed to update scenario.";
                return;
            }

            

            

            // Handle cloning if selected (must check before clearing)
            if (!string.IsNullOrEmpty(_model.CloneFromScenarioId) && _scenarioFullyCreated == false)
            {
                // TODO: Implement clone functionality when we have scenario data
                // For now, just show a message
                _successMessage = "Scenario name updated! Cloning feature coming soon.";
            }
            else
            {
                _successMessage = "Scenario settings saved successfully!";
            }

            if (isFirstSave)
            {
                Navigation.NavigateTo($"/scenario/{ScenarioId}/income", forceLoad: false);
                return;
            }
            _scenarioFullyCreated = true;

        }
        catch (Exception)
        {
            _errorMessage = "An error occurred while saving scenario settings.";
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task HandleDelete()
    {
        if (!await ConfirmDelete())
            return;

        _isDeleting = true;
        _successMessage = null;
        _errorMessage = null;

        try
        {
            var command = new DeleteScenarioCommand
            {
                ScenarioId = ScenarioId,
                UserId = _userId
            };

            var result = await Mediator.Send(command);

            if (result.Success)
            {
                Navigation.NavigateTo("/home", forceLoad: false);
            }
            else
            {
                _errorMessage = result.ErrorMessage ?? "Failed to delete scenario.";
            }
        }
        catch (Exception)
        {
            _errorMessage = "An error occurred while deleting the scenario.";
        }
        finally
        {
            _isDeleting = false;
        }
    }

    private async Task<bool> ConfirmDelete()
    {
        // Simple confirmation - in production you might want a proper confirmation modal
        return await Task.FromResult(true); // TODO: Add proper confirmation dialog
    }

    public void Dispose()
    {
        LoadingService?.Dispose();
    }
}

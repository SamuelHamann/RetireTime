using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RetirementTime.Application.Features.Dashboard.DeleteScenario;
using RetirementTime.Application.Features.Dashboard.GetAssumptions;
using RetirementTime.Application.Features.Dashboard.GetScenarios;
using RetirementTime.Application.Features.Dashboard.SaveAssumptions;
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
    private AssumptionsModel _assumptionsModel = new();
    private List<Application.Features.Dashboard.Common.ScenarioDto> _otherScenarios = [];
    private bool _isSaving = false;
    private bool _isDeleting = false;
    private bool _isSavingAssumptions = false;
    private bool _scenarioFound = true;
    private bool _scenarioFullyCreated = false;
    private string? _successMessage;
    private string? _errorMessage;
    private string? _assumptionsSuccessMessage;
    private string? _assumptionsErrorMessage;
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

                if (_scenarioFullyCreated)
                    await LoadAssumptions();
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

            

            

            _scenarioFullyCreated = true;
            _successMessage = null;

            if (isFirstSave)
            {
                await LoadAssumptions();
                StateHasChanged();
                return;
            }

            // Handle cloning if selected (must check before clearing)
            if (!string.IsNullOrEmpty(_model.CloneFromScenarioId))
            {
                // TODO: Implement clone functionality when we have scenario data
                _successMessage = "Scenario name updated! Cloning feature coming soon.";
            }
            else
            {
                _successMessage = "Scenario settings saved successfully!";
            }


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

    private async Task LoadAssumptions()
    {
        var result = await Mediator.Send(new GetAssumptionsQuery { ScenarioId = ScenarioId });
        if (result.Success && result.Assumptions != null)
        {
            var a = result.Assumptions;
            _assumptionsModel.YearlyInflationRate = a.YearlyInflationRate;
            _assumptionsModel.YearlyPropertyAppreciation = a.YearlyPropertyAppreciation;
            _assumptionsModel.StockAllocation = a.StockAllocation;
            _assumptionsModel.StockYearlyReturn = a.StockYearlyReturn;
            _assumptionsModel.StockYearlyDividend = a.StockYearlyDividend;
            _assumptionsModel.StockCanadianAllocation = a.StockCanadianAllocation;
            _assumptionsModel.StockForeignAllocation = a.StockForeignAllocation;
            _assumptionsModel.StockFees = a.StockFees;
            _assumptionsModel.BondAllocation = a.BondAllocation;
            _assumptionsModel.BondYearlyReturn = a.BondYearlyReturn;
            _assumptionsModel.BondFees = a.BondFees;
            _assumptionsModel.CashAllocation = a.CashAllocation;
            _assumptionsModel.CashYearlyReturn = a.CashYearlyReturn;
        }
    }

    private async Task HandleSaveAssumptions()
    {
        _assumptionsSuccessMessage = null;
        _assumptionsErrorMessage = null;

        try
        {
            var command = new SaveAssumptionsCommand
            {
                ScenarioId = ScenarioId,
                YearlyInflationRate = _assumptionsModel.YearlyInflationRate,
                YearlyPropertyAppreciation = _assumptionsModel.YearlyPropertyAppreciation,
                StockAllocation = _assumptionsModel.StockAllocation,
                StockYearlyReturn = _assumptionsModel.StockYearlyReturn,
                StockYearlyDividend = _assumptionsModel.StockYearlyDividend,
                StockCanadianAllocation = _assumptionsModel.StockCanadianAllocation,
                StockForeignAllocation = _assumptionsModel.StockForeignAllocation,
                StockFees = _assumptionsModel.StockFees,
                BondAllocation = _assumptionsModel.BondAllocation,
                BondYearlyReturn = _assumptionsModel.BondYearlyReturn,
                BondFees = _assumptionsModel.BondFees,
                CashAllocation = _assumptionsModel.CashAllocation,
                CashYearlyReturn = _assumptionsModel.CashYearlyReturn,
            };

            var result = await Mediator.Send(command);

            if (result.Success)
            {
                _assumptionsSuccessMessage = "Saved";
                StateHasChanged();
                await Task.Delay(2000);
                _assumptionsSuccessMessage = null;
            }
            else
            {
                _assumptionsErrorMessage = result.ErrorMessage ?? "Failed to save assumptions.";
            }
        }
        catch (Exception)
        {
            _assumptionsErrorMessage = "An error occurred while saving assumptions.";
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

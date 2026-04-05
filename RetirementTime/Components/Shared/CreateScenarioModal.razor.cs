using MediatR;
using Microsoft.AspNetCore.Components;
using RetirementTime.Application.Features.Dashboard.Common;
using RetirementTime.Application.Features.Dashboard.CreateScenario;
using RetirementTime.Models.Dashboard;

namespace RetirementTime.Components.Shared;

public partial class CreateScenarioModal : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Parameter] public long UserId { get; set; }
    [Parameter] public List<ScenarioDto> ExistingScenarios { get; set; } = [];
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback<long> OnScenarioCreated { get; set; }

    private CreateScenarioModel _model = new();
    private bool _isSubmitting = false;

    private async Task HandleSubmit()
    {
        _isSubmitting = true;

        var command = new CreateScenarioCommand
        {
            UserId = UserId,
            ScenarioName = _model.ScenarioName,
            CloneFromScenarioId = string.IsNullOrEmpty(_model.CloneFromScenarioId) ? null : long.Parse(_model.CloneFromScenarioId)
        };

        var result = await Mediator.Send(command);

        _isSubmitting = false;

        if (result.Success)
        {
            await OnScenarioCreated.InvokeAsync(result.ScenarioId);
            await OnClose.InvokeAsync();
        }
    }

    private async Task OnCancel()
    {
        await OnClose.InvokeAsync();
    }
}

using MediatR;
using Microsoft.AspNetCore.Components;
using RetirementTime.Application.Features.BeginnerGuide.Benefits.GetPensionTypes;
using RetirementTime.Application.Features.BeginnerGuide.Benefits.UpsertPensions;
using RetirementTime.Models.BeginnerGuide.Benefits;

namespace RetirementTime.Components.Pages.BeginnerGuide.Benefits;

public partial class Step1Pension : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Parameter] public long UserId { get; set; }
    [Parameter] public bool HasPensions { get; set; }
    [Parameter] public EventCallback<bool> HasPensionsChanged { get; set; }
    [Parameter] public List<PensionFormModel> Pensions { get; set; } = new();
    [Parameter] public List<PensionTypeDto> PensionTypes { get; set; } = new();
    [Parameter] public EventCallback OnNext { get; set; }
    [Parameter] public EventCallback OnPrevious { get; set; }

    private bool _isSaving;

    private async Task OnHasPensionsChanged(bool value)
    {
        HasPensions = value;
        await HasPensionsChanged.InvokeAsync(value);
    }

    private void AddPension()
    {
        Pensions.Add(new PensionFormModel());
    }

    private void RemovePension(int index)
    {
        if (index >= 0 && index < Pensions.Count)
        {
            Pensions.RemoveAt(index);
        }
    }

    private async Task HandleSaveAndContinue()
    {
        _isSaving = true;

        var command = new UpsertPensionsCommand
        {
            UserId = UserId,
            HasPensions = HasPensions,
            Pensions = Pensions.Select(p => new PensionInputDto
            {
                EmployerName = p.EmployerName,
                PensionTypeId = p.PensionTypeId
            }).ToList()
        };

        var result = await Mediator.Send(command);

        _isSaving = false;

        if (result.Success)
        {
            await OnNext.InvokeAsync();
        }
    }

    private async Task HandleNext()
    {
        await OnNext.InvokeAsync();
    }

    private async Task HandlePrevious()
    {
        await OnPrevious.InvokeAsync();
    }
}

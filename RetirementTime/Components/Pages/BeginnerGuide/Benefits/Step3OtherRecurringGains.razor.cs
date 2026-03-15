using MediatR;
using Microsoft.AspNetCore.Components;
using RetirementTime.Application.Features.BeginnerGuide.Benefits.UpsertOtherRecurringGains;
using RetirementTime.Application.Features.Common.GetFrequencies;
using RetirementTime.Models.BeginnerGuide.Benefits;

namespace RetirementTime.Components.Pages.BeginnerGuide.Benefits;

public partial class Step3OtherRecurringGains : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Parameter] public long UserId { get; set; }
    [Parameter] public bool HasOtherRecurringGains { get; set; }
    [Parameter] public EventCallback<bool> HasOtherRecurringGainsChanged { get; set; }
    [Parameter] public List<OtherRecurringGainFormModel> OtherRecurringGains { get; set; } = new();
    [Parameter] public EventCallback OnPrevious { get; set; }
    [Parameter] public EventCallback OnNext { get; set; }

    private bool _isSaving;
    private List<FrequencyDto> _frequencies = new();

    protected override async Task OnInitializedAsync()
    {
        _frequencies = await Mediator.Send(new GetFrequenciesQuery());
    }

    private async Task OnHasGainsChanged(bool value)
    {
        HasOtherRecurringGains = value;
        await HasOtherRecurringGainsChanged.InvokeAsync(value);
    }

    private void AddRecurringGain()
    {
        OtherRecurringGains.Add(new OtherRecurringGainFormModel());
    }

    private void RemoveRecurringGain(int index)
    {
        if (index >= 0 && index < OtherRecurringGains.Count)
        {
            OtherRecurringGains.RemoveAt(index);
        }
    }

    private async Task HandleSaveAndComplete()
    {
        _isSaving = true;

        var command = new UpsertOtherRecurringGainsCommand
        {
            UserId = UserId,
            HasOtherRecurringGains = HasOtherRecurringGains,
            Gains = OtherRecurringGains.Select(g => new OtherRecurringGainInputDto
            {
                SourceName = g.SourceName,
                Amount = g.Amount,
                FrequencyId = g.FrequencyId
            }).ToList()
        };

        var result = await Mediator.Send(command);

        _isSaving = false;

        if (result.Success)
        {
            await OnNext.InvokeAsync();
        }
    }

    private async Task HandleComplete()
    {
        await OnNext.InvokeAsync();
    }

    private async Task HandlePrevious()
    {
        await OnPrevious.InvokeAsync();
    }
}

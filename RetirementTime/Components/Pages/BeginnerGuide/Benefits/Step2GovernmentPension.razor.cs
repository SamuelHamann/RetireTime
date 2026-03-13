using MediatR;
using Microsoft.AspNetCore.Components;
using RetirementTime.Application.Features.BeginnerGuide.Benefits.UpsertGovernmentPension;
using RetirementTime.Models.BeginnerGuide.Benefits;

namespace RetirementTime.Components.Pages.BeginnerGuide.Benefits;

public partial class Step2GovernmentPension : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; } = default!;

    [Parameter] public long UserId { get; set; }
    [Parameter] public GovernmentPensionFormModel GovernmentPension { get; set; } = new();
    [Parameter] public EventCallback OnNext { get; set; }
    [Parameter] public EventCallback OnPrevious { get; set; }

    private bool _isSaving;

    private async Task HandleSaveAndContinue()
    {
        _isSaving = true;

        var command = new UpsertGovernmentPensionCommand
        {
            UserId = UserId,
            YearsWorked = GovernmentPension.YearsWorked,
            HasSpecializedPublicSectorPension = GovernmentPension.HasSpecializedPublicSectorPension,
            SpecializedPensionName = GovernmentPension.SpecializedPensionName
        };

        var result = await Mediator.Send(command);

        _isSaving = false;

        if (result.Success)
        {
            await OnNext.InvokeAsync();
        }
    }

    private async Task HandlePrevious()
    {
        await OnPrevious.InvokeAsync();
    }
}

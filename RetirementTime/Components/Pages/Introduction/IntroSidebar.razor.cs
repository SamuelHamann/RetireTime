using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Resources.Introduction;
using RetirementTime.Resources.Common;

namespace RetirementTime.Components.Pages.Introduction;

public partial class IntroSidebar
{
    [Inject] private IStringLocalizer<IntroResources> Localizer { get; set; } = default!;

    [Parameter] public int CurrentStep { get; set; } = 1;
    [Parameter] public int TotalSteps { get; set; } = 5;
    [Parameter] public string[] StepNames { get; set; } = [];
    [Parameter] public EventCallback<int> OnStepSelected { get; set; }
    [Parameter] public bool[] StepCompletionStatus { get; set; } = [];
}


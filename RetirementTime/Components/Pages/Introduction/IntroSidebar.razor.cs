using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Resources.Introduction;
using RetirementTime.Resources.Common;

namespace RetirementTime.Components.Pages.Introduction;

public partial class IntroSidebar
{
    [Parameter] public int CurrentStep { get; set; } = 1;
    [Parameter] public int TotalSteps { get; set; } = 5;
    [Parameter] public int ActiveStep { get; set; } = 1;
    [Parameter] public string[] StepNames { get; set; } = [];
    [Parameter] public EventCallback<int> OnStepSelected { get; set; }
    [Parameter] public bool[] StepCompletionStatus { get; set; } = [];

    private bool IsStepSelected(int step) => step == CurrentStep;

    private bool IsStepCompleted(int step)
    {
        // Check if step has completion status in array
        if (step - 1 >= 0 && step - 1 < StepCompletionStatus.Length)
            return StepCompletionStatus[step - 1];

        return false;
    }

    private bool IsStepAccessible(int step)
    {
        // Completed steps are always accessible
        if (IsStepCompleted(step))
            return true;

        // Active step (first incomplete step) is accessible
        if (step == ActiveStep)
            return true;

        // Future incomplete steps are not accessible
        return false;
    }
}


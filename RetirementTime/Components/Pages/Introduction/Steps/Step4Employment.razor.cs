using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Models.Introduction;
using RetirementTime.Resources.Introduction;

namespace RetirementTime.Components.Pages.Introduction.Steps;

public partial class Step4Employment
{
    [Inject] private IStringLocalizer<IntroResources> Localizer { get; set; } = default!;

    public EmploymentModel Model { get; set; } = new();
}


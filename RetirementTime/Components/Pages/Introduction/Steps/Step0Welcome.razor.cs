using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Resources.Introduction;

namespace RetirementTime.Components.Pages.Introduction.Steps;

public partial class Step0Welcome
{
    [Inject] private IStringLocalizer<IntroResources> Localizer { get; set; } = default!;

    [Parameter] public EventCallback OnStart { get; set; }
}


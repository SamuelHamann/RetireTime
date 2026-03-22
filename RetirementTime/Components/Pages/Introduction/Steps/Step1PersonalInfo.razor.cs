using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using RetirementTime.Models.Introduction;
using RetirementTime.Resources.Introduction;

namespace RetirementTime.Components.Pages.Introduction.Steps;

public partial class Step1PersonalInfo
{
    [Inject] private IStringLocalizer<IntroResources> Localizer { get; set; } = default!;

    public PersonalInfoModel Model { get; set; } = new();
}

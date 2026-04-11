using Microsoft.AspNetCore.Components;
using RetirementTime.Resources.Dashboard;
using Microsoft.Extensions.Localization;
using RetirementTime.Services;

namespace RetirementTime.Components.Pages.Dashboard.IncomeAndAssets;

public partial class GroupRrsp : ComponentBase
{
    [Inject] private IStringLocalizer<DashboardResources> Localizer { get; set; } = default!;
    [Inject] private IncomeNavigationService IncomeNavService { get; set; } = default!;

    [Parameter] public long ScenarioId { get; set; }
    

    private void NavigateNext() => IncomeNavService.NavigateNext();
}

using System.ComponentModel.DataAnnotations;

namespace RetirementTime.Models.Dashboard;

public class ScenarioSettingsModel
{
    [Required(ErrorMessage = "Scenario name is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Scenario name must be between 1 and 100 characters")]
    public string ScenarioName { get; set; } = string.Empty;

    public string? CloneFromScenarioId { get; set; }
}

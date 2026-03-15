using System.ComponentModel.DataAnnotations;

namespace RetirementTime.Models.BeginnerGuide.Benefits;

public class GovernmentPensionFormModel
{
    [Required(ErrorMessage = "Years worked is required")]
    [Range(0, 100, ErrorMessage = "Years worked must be between 0 and 100")]
    public int YearsWorked { get; set; }

    public bool HasSpecializedPublicSectorPension { get; set; }

    [MaxLength(200, ErrorMessage = "Pension name cannot exceed 200 characters")]
    public string? SpecializedPensionName { get; set; }
}

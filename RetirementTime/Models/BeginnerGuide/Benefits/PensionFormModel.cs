using System.ComponentModel.DataAnnotations;

namespace RetirementTime.Models.BeginnerGuide.Benefits;

public class PensionFormModel
{
    [Required(ErrorMessage = "Employer name is required")]
    [MaxLength(200, ErrorMessage = "Employer name cannot exceed 200 characters")]
    public string EmployerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pension type is required")]
    public int PensionTypeId { get; set; }
}

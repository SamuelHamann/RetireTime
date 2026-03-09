using System.ComponentModel.DataAnnotations;

namespace RetirementTime.Models.BeginnerGuide.Benefits;

public class OtherRecurringGainFormModel
{
    [Required(ErrorMessage = "Source name is required")]
    [MaxLength(200, ErrorMessage = "Source name cannot exceed 200 characters")]
    public string SourceName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Amount is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Frequency is required")]
    public int FrequencyId { get; set; }
}

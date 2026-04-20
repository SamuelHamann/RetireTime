using System.ComponentModel.DataAnnotations;

namespace RetirementTime.Models.Introduction;

public class EmploymentModel
{
    // Employment
    public bool IsEmployed { get; set; }
    public bool IsSelfEmployed { get; set; }

    [Range(18, 80, ErrorMessage = "Please enter a valid retirement age between 18 and 80.")]
    public int? PlannedRetirementAge { get; set; }

    [Range(0, 60, ErrorMessage = "Please enter a valid number of years between 0 and 60.")]
    public int? CppContributionYears { get; set; }

    // Income
    public bool HasRoyalties { get; set; }
    public bool HasDividends { get; set; }
    public bool HasRentalIncome { get; set; }
    public bool HasOtherIncome { get; set; }
}


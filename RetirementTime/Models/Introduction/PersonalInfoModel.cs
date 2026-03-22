using System.ComponentModel.DataAnnotations;

namespace RetirementTime.Models.Introduction;

public class PersonalInfoModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [MaxLength(100, ErrorMessage = "First name must be 100 characters or fewer")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(100, ErrorMessage = "Last name must be 100 characters or fewer")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required")]
    public DateOnly? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Citizenship status is required")]
    public string CitizenshipStatus { get; set; } = string.Empty;

    [Required(ErrorMessage = "Marital status is required")]
    public string MaritalStatus { get; set; } = string.Empty;

    public bool HasCurrentChildren { get; set; }
    public bool PlansFutureChildren { get; set; }
    public bool IncludePartner { get; set; }
}


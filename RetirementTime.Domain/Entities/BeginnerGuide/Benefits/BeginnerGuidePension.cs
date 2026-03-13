namespace RetirementTime.Domain.Entities.BeginnerGuide.Benefits;

public class BeginnerGuidePension
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public int PensionTypeId { get; set; }
    public string EmployerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User? User { get; set; }
    public BeginnerGuidePensionType? PensionType { get; set; }
}


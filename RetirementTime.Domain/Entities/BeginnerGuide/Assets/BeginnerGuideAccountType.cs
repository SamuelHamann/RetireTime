namespace RetirementTime.Domain.Entities.BeginnerGuide.Assets;

public class BeginnerGuideAccountType
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int CountryId { get; set; }
    public int? SubdivisionId { get; set; }
    
    // Navigation properties
    public Location.Country? Country { get; set; }
    public Location.Subdivision? Subdivision { get; set; }
}


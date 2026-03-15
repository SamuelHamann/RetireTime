using System.Collections.ObjectModel;

namespace RetirementTime.Domain.Entities.Location;

public class Country
{
    public int Id { get; set; }
    public required string CountryCode { get; set; }
    public required string Name { get; set; }
    
    public virtual Collection<Subdivision> Subdivisions { get; set; } = new();
    
}
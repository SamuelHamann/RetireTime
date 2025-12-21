namespace RetimrementTime.Domain.Entities.Location;

public class Subdivision
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Code { get; set; }
}
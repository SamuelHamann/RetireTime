namespace RetimrementTime.Domain.Entities.Location;

public class LocationInfo
{
    public int Id { get; set; }
    public required Country Country { get; set; }
    public Subdivision? Subdivision { get; set; }
}
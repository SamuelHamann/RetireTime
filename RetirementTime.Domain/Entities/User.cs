using RetimrementTime.Domain.Entities.Location;
namespace RetimrementTime.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required bool IsActive { get; set; }
    public required Role Role { get; set; }
    public required LocationInfo LocationInfo { get; set; }
    public User? Spouse { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}
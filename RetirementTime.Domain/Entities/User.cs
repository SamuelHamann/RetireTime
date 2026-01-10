using RetirementTime.Domain.Entities.Location;

namespace RetirementTime.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required bool IsActive { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    
    public int RoleId { get; set; }
    public int CountryId { get; set; }
    public int? SubdivisionId { get; set; }
    public long? SpouseId { get; set; }
    public virtual required Role Role { get; set; }
    public virtual required Country Country { get; set; }
    public virtual Subdivision? Subdivision { get; set; }
    public virtual User? Spouse { get; set; }
}
namespace RetirementTime.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public required string Name { get; set; }
    
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
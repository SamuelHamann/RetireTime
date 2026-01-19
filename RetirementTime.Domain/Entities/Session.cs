namespace RetirementTime.Domain.Entities;

public class Session
{
    public Guid SessionId { get; set; }
    public required long UserId { get; set; }
    public required User User { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime ValidUntil { get; set; }
}


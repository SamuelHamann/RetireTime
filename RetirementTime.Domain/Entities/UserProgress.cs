namespace RetirementTime.Domain.Entities;

public class UserProgress
{
    public long UserId { get; set; }
    public bool HasFinishedBeginnerGuide { get; set; }

    public virtual required User User { get; set; }
}


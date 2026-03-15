using RetirementTime.Domain.Entities.Common;

namespace RetirementTime.Domain.Entities.BeginnerGuide.Benefits;

public class BeginnerGuideOtherRecurringGain
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int FrequencyId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User? User { get; set; }
    public Frequency? Frequency { get; set; }
}


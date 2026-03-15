namespace RetirementTime.Domain.Entities.Common;

public class Frequency
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int FrequencyPerYear { get; set; }
}

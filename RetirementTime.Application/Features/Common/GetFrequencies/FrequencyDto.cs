namespace RetirementTime.Application.Features.Common.GetFrequencies;

public class FrequencyDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int FrequencyPerYear { get; set; }
}

namespace RetirementTime.Application.Common;

public record BaseResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
}


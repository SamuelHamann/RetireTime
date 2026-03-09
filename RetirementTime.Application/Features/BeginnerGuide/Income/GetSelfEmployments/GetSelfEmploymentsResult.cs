namespace RetirementTime.Application.Features.BeginnerGuide.Income.GetSelfEmployments;

public class GetSelfEmploymentsResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public List<SelfEmploymentDto> SelfEmployments { get; set; } = new();
}

public class SelfEmploymentDto
{
    public long Id { get; set; }
    public required string BusinessName { get; set; }
    public required decimal AnnualSalary { get; set; }
    public required decimal AverageAnnualRevenueIncrease { get; set; }
    public required decimal MonthlyDividends { get; set; }
    public List<AdditionalCompensationDto> AdditionalCompensations { get; set; } = new();
}

public class AdditionalCompensationDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required decimal Amount { get; set; }
    public required int FrequencyId { get; set; }
}

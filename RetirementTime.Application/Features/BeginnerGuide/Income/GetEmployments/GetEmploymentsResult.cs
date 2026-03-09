namespace RetirementTime.Application.Features.BeginnerGuide.Income.GetEmployments;

public class GetEmploymentsResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public List<EmploymentDto> Employments { get; set; } = new();
}

public class EmploymentDto
{
    public long Id { get; set; }
    public required string EmployerName { get; set; }
    public required decimal AnnualSalary { get; set; }
    public required decimal AverageAnnualWageIncrease { get; set; }
    public List<AdditionalCompensationDto> AdditionalCompensations { get; set; } = new();
}

public class AdditionalCompensationDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required decimal Amount { get; set; }
    public required int FrequencyId { get; set; }
}

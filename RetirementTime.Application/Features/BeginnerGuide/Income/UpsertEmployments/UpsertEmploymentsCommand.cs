using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.BeginnerGuide.Income.UpsertEmployments;

public class UpsertEmploymentsCommand : IRequest<UpsertEmploymentsResult>
{
    public required long UserId { get; set; }
    public required List<EmploymentDto> Employments { get; set; }
}

public class EmploymentDto
{
    public required string EmployerName { get; set; }
    public required decimal AnnualSalary { get; set; }
    public required decimal AverageAnnualWageIncrease { get; set; }
    public List<AdditionalCompensationDto> AdditionalCompensations { get; set; } = new();
}

public class AdditionalCompensationDto
{
    public required string Name { get; set; }
    public required decimal Amount { get; set; }
    public required int FrequencyId { get; set; }
}

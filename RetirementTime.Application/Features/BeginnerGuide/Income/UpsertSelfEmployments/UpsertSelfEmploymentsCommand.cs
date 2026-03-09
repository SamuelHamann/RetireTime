using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.BeginnerGuide.Income.UpsertSelfEmployments;

public class UpsertSelfEmploymentsCommand : IRequest<UpsertSelfEmploymentsResult>
{
    public required long UserId { get; set; }
    public required List<SelfEmploymentDto> SelfEmployments { get; set; }
}

public class SelfEmploymentDto
{
    public required string BusinessName { get; set; }
    public required decimal AnnualSalary { get; set; }
    public required decimal AverageAnnualRevenueIncrease { get; set; }
    public required decimal MonthlyDividends { get; set; }
    public List<AdditionalCompensationDto> AdditionalCompensations { get; set; } = new();
}

public class AdditionalCompensationDto
{
    public required string Name { get; set; }
    public required decimal Amount { get; set; }
    public required int FrequencyId { get; set; }
}

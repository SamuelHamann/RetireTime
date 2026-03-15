using MediatR;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.GetGovernmentPension;

public class GetGovernmentPensionQuery : IRequest<GovernmentPensionDto?>
{
    public long UserId { get; set; }
}

public class GovernmentPensionDto
{
    public long Id { get; set; }
    public int YearsWorked { get; set; }
    public bool HasSpecializedPublicSectorPension { get; set; }
    public string? SpecializedPensionName { get; set; }
}


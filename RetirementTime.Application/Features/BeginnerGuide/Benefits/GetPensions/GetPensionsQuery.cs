using MediatR;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.GetPensions;

public class GetPensionsQuery : IRequest<List<PensionDto>>
{
    public long UserId { get; set; }
}

public class PensionDto
{
    public long Id { get; set; }
    public int PensionTypeId { get; set; }
    public string EmployerName { get; set; } = string.Empty;
}


using MediatR;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.GetPensionTypes;

public class GetPensionTypesQuery : IRequest<List<PensionTypeDto>>
{
}

public class PensionTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}


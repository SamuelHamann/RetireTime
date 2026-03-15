using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.UpsertPensions;

public class UpsertPensionsCommand : IRequest<UpsertPensionsResult>
{
    public long UserId { get; set; }
    public bool HasPensions { get; set; }
    public List<PensionInputDto> Pensions { get; set; } = new();
}

public class PensionInputDto
{
    public long? Id { get; set; }
    public int PensionTypeId { get; set; }
    public string EmployerName { get; set; } = string.Empty;
}

public record UpsertPensionsResult : BaseResult
{
    public List<long> PensionIds { get; init; } = new();
}


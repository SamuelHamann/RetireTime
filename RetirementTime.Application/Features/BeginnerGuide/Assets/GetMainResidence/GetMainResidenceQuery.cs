using MediatR;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.GetMainResidence;

public record GetMainResidenceQuery : IRequest<GetMainResidenceResult>
{
    public required long UserId { get; init; }
}

public record GetMainResidenceResult
{
    public MainResidence? MainResidence { get; init; }
}


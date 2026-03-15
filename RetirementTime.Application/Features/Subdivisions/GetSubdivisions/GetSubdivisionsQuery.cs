using MediatR;
using RetirementTime.Domain.Entities.Location;

namespace RetirementTime.Application.Features.Subdivisions.GetSubdivisions;

public record GetSubdivisionsQuery : IRequest<List<Subdivision>>
{
    public int CountryId { get; init; }
};
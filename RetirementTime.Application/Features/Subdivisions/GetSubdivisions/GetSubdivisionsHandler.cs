using MediatR;
using RetirementTime.Domain.Entities.Location;
using RetirementTime.Domain.Interfaces;

namespace RetirementTime.Application.Features.Subdivisions.GetSubdivisions;

public class GetSubdivisionsHandler(ISubdivisionRepository subdivisionRepository) 
    : IRequestHandler<GetSubdivisionsQuery, List<Subdivision>>
{
    public async Task<List<Subdivision>> Handle(GetSubdivisionsQuery request, CancellationToken cancellationToken)
    {
        return await subdivisionRepository.GetSubdivisionsByCountryId(request.CountryId);
    }
}
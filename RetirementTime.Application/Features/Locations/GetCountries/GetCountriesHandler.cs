using MediatR;
using RetirementTime.Domain.Entities.Location;
using RetirementTime.Domain.Interfaces;

namespace RetirementTime.Application.Features.Locations.GetCountries;

public class GetCountriesHandler(ICountryRepository countryRepository)
    : IRequestHandler<GetCountriesQuery, List<Country>>
{
    public async Task<List<Country>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        return await countryRepository.GetCountries();
    }
}
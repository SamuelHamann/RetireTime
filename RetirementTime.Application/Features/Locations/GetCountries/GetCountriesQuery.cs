using MediatR;
using RetirementTime.Domain.Entities.Location;

namespace RetirementTime.Application.Features.Locations.GetCountries;

public record GetCountriesQuery : IRequest<List<Country>>;


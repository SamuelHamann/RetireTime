using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Location;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Locations.GetCountries;

public partial class GetCountriesHandler(
    ICountryRepository countryRepository,
    ILogger<GetCountriesHandler> logger)
    : IRequestHandler<GetCountriesQuery, List<Country>>
{
    public async Task<List<Country>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        LogStartingGetCountriesHandler(logger);

        try
        {
            var countries = await countryRepository.GetCountries();
            
            LogSuccessfullyRetrievedCountries(logger, countries.Count);
            
            return countries;
        }
        catch (Exception ex)
        {
            LogErrorOccurredWhileRetrievingCountries(logger, ex.Message);
            return new List<Country>();
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetCountries handler")]
    static partial void LogStartingGetCountriesHandler(ILogger<GetCountriesHandler> logger);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {count} countries")]
    static partial void LogSuccessfullyRetrievedCountries(ILogger<GetCountriesHandler> logger, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred while retrieving countries | Exception: {exception}")]
    static partial void LogErrorOccurredWhileRetrievingCountries(ILogger<GetCountriesHandler> logger, string exception);
}
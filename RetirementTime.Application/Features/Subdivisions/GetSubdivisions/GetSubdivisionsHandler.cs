using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.Location;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Subdivisions.GetSubdivisions;

public partial class GetSubdivisionsHandler(
    ISubdivisionRepository subdivisionRepository,
    ILogger<GetSubdivisionsHandler> logger) 
    : IRequestHandler<GetSubdivisionsQuery, List<Subdivision>>
{
    public async Task<List<Subdivision>> Handle(GetSubdivisionsQuery request, CancellationToken cancellationToken)
    {
        LogStartingGetSubdivisionsHandlerForCountryId(logger, request.CountryId);

        try
        {
            var subdivisions = await subdivisionRepository.GetSubdivisionsByCountryId(request.CountryId);
            
            LogSuccessfullyRetrievedSubdivisionsForCountryId(logger, subdivisions.Count, request.CountryId);
            
            return subdivisions;
        }
        catch (Exception ex)
        {
            LogErrorOccurredWhileRetrievingSubdivisionsForCountryId(logger, ex.Message, request.CountryId);
            return new List<Subdivision>();
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetSubdivisions handler for CountryId: {countryId}")]
    static partial void LogStartingGetSubdivisionsHandlerForCountryId(ILogger<GetSubdivisionsHandler> logger, int countryId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {count} subdivisions for CountryId: {countryId}")]
    static partial void LogSuccessfullyRetrievedSubdivisionsForCountryId(ILogger<GetSubdivisionsHandler> logger, int count, int countryId);

    [LoggerMessage(LogLevel.Error, "Error occurred while retrieving subdivisions for CountryId: {countryId} | Exception: {exception}")]
    static partial void LogErrorOccurredWhileRetrievingSubdivisionsForCountryId(ILogger<GetSubdivisionsHandler> logger, string exception, int countryId);
}
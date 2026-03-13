using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.GetPensionTypes;

public partial class GetPensionTypesHandler(
    IBeginnerGuidePensionTypeRepository pensionTypeRepository,
    ILogger<GetPensionTypesHandler> logger) : IRequestHandler<GetPensionTypesQuery, List<PensionTypeDto>>
{
    public async Task<List<PensionTypeDto>> Handle(GetPensionTypesQuery request, CancellationToken cancellationToken)
    {
        LogStartingQuery(logger);

        try
        {
            var pensionTypes = await pensionTypeRepository.GetAllAsync();

            var result = pensionTypes.Select(pt => new PensionTypeDto
            {
                Id = pt.Id,
                Name = pt.Name,
                Description = pt.Description
            }).ToList();

            LogQuerySuccessful(logger, result.Count);

            return result;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message);
            return [];
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetPensionTypes query")]
    static partial void LogStartingQuery(ILogger<GetPensionTypesHandler> logger);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} pension types")]
    static partial void LogQuerySuccessful(ILogger<GetPensionTypesHandler> logger, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred while retrieving pension types | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetPensionTypesHandler> logger, string exception);
}


using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Common.GetFrequencies;

public partial class GetFrequenciesHandler(
    IFrequencyRepository repository,
    ILogger<GetFrequenciesHandler> logger) : IRequestHandler<GetFrequenciesQuery, List<FrequencyDto>>
{
    public async Task<List<FrequencyDto>> Handle(GetFrequenciesQuery request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger);

        try
        {
            var frequencies = await repository.GetAllAsync();
            
            var frequencyDtos = frequencies.Select(f => new FrequencyDto
            {
                Id = f.Id,
                Name = f.Name,
                FrequencyPerYear = f.FrequencyPerYear
            }).ToList();
            
            LogSuccessfullyCompleted(logger, frequencyDtos.Count);
            
            return frequencyDtos;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message);
            return new List<FrequencyDto>();
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetFrequencies handler")]
    static partial void LogStartingHandler(ILogger<GetFrequenciesHandler> logger);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} frequencies")]
    static partial void LogSuccessfullyCompleted(ILogger<GetFrequenciesHandler> logger, int Count);

    [LoggerMessage(LogLevel.Error, "Error occurred while getting frequencies | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetFrequenciesHandler> logger, string Exception);
}

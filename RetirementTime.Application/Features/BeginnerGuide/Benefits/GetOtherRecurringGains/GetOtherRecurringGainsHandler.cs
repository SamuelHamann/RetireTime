using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.GetOtherRecurringGains;

public partial class GetOtherRecurringGainsHandler(
    IBeginnerGuideOtherRecurringGainRepository gainRepository,
    ILogger<GetOtherRecurringGainsHandler> logger) : IRequestHandler<GetOtherRecurringGainsQuery, List<OtherRecurringGainDto>>
{
    public async Task<List<OtherRecurringGainDto>> Handle(GetOtherRecurringGainsQuery request, CancellationToken cancellationToken)
    {
        LogStartingQuery(logger, request.UserId);

        try
        {
            var gains = await gainRepository.GetByUserIdAsync(request.UserId);

            var result = gains.Select(g => new OtherRecurringGainDto
            {
                Id = g.Id,
                SourceName = g.SourceName,
                Amount = g.Amount,
                FrequencyId = g.FrequencyId
            }).ToList();

            LogQuerySuccessful(logger, request.UserId, result.Count);

            return result;
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return [];
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting GetOtherRecurringGains query for UserId: {UserId}")]
    static partial void LogStartingQuery(ILogger<GetOtherRecurringGainsHandler> logger, long userId);

    [LoggerMessage(LogLevel.Information, "Successfully retrieved {Count} recurring gains for UserId: {UserId}")]
    static partial void LogQuerySuccessful(ILogger<GetOtherRecurringGainsHandler> logger, long userId, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred while retrieving recurring gains for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<GetOtherRecurringGainsHandler> logger, string exception, long userId);
}


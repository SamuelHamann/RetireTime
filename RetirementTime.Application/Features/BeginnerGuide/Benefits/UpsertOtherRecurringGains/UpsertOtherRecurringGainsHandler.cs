using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.BeginnerGuide.Benefits;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.UpsertOtherRecurringGains;

public partial class UpsertOtherRecurringGainsHandler(
    IBeginnerGuideOtherRecurringGainRepository gainRepository,
    ILogger<UpsertOtherRecurringGainsHandler> logger) : IRequestHandler<UpsertOtherRecurringGainsCommand, UpsertOtherRecurringGainsResult>
{
    public async Task<UpsertOtherRecurringGainsResult> Handle(UpsertOtherRecurringGainsCommand request, CancellationToken cancellationToken)
    {
        LogStartingUpsert(logger, request.UserId, request.HasOtherRecurringGains, request.Gains.Count);

        try
        {
            if (!request.HasOtherRecurringGains)
            {
                await gainRepository.DeleteByUserIdAsync(request.UserId);
                LogDeletedAllGains(logger, request.UserId);

                return new UpsertOtherRecurringGainsResult { Success = true, GainIds = [] };
            }

            foreach (var gainDto in request.Gains)
            {
                if (string.IsNullOrWhiteSpace(gainDto.SourceName))
                {
                    LogValidationFailed(logger, "Source name is required");
                    return new UpsertOtherRecurringGainsResult { Success = false, ErrorMessage = "Source name is required." };
                }

                if (gainDto.Amount <= 0)
                {
                    LogValidationFailed(logger, "Amount must be greater than 0");
                    return new UpsertOtherRecurringGainsResult { Success = false, ErrorMessage = "Amount must be greater than 0." };
                }

                if (gainDto.FrequencyId <= 0)
                {
                    LogValidationFailed(logger, "A valid frequency must be selected");
                    return new UpsertOtherRecurringGainsResult { Success = false, ErrorMessage = "A valid frequency must be selected." };
                }
            }

            var gains = request.Gains.Select(g => new BeginnerGuideOtherRecurringGain
            {
                UserId = request.UserId,
                SourceName = g.SourceName,
                Amount = g.Amount,
                FrequencyId = g.FrequencyId
            }).ToList();

            var savedGains = await gainRepository.UpsertGainsAsync(request.UserId, gains);

            LogUpsertSuccessful(logger, request.UserId, savedGains.Count);

            return new UpsertOtherRecurringGainsResult { Success = true, GainIds = savedGains.Select(g => g.Id).ToList() };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new UpsertOtherRecurringGainsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving your recurring gains. Please try again."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpsertOtherRecurringGains for UserId: {UserId}, HasGains: {HasGains}, Count: {Count}")]
    static partial void LogStartingUpsert(ILogger<UpsertOtherRecurringGainsHandler> logger, long userId, bool hasGains, int count);

    [LoggerMessage(LogLevel.Information, "Deleted all recurring gains for UserId: {UserId}")]
    static partial void LogDeletedAllGains(ILogger<UpsertOtherRecurringGainsHandler> logger, long userId);

    [LoggerMessage(LogLevel.Warning, "Validation failed: {Reason}")]
    static partial void LogValidationFailed(ILogger<UpsertOtherRecurringGainsHandler> logger, string reason);

    [LoggerMessage(LogLevel.Information, "Successfully upserted recurring gains for UserId: {UserId}, Count: {Count}")]
    static partial void LogUpsertSuccessful(ILogger<UpsertOtherRecurringGainsHandler> logger, long userId, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpsertOtherRecurringGainsHandler> logger, string exception, long userId);
}


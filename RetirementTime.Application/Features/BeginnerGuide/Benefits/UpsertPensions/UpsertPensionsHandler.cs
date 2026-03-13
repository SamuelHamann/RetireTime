using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.BeginnerGuide.Benefits;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.UpsertPensions;

public partial class UpsertPensionsHandler(
    IBeginnerGuidePensionRepository pensionRepository,
    ILogger<UpsertPensionsHandler> logger) : IRequestHandler<UpsertPensionsCommand, UpsertPensionsResult>
{
    public async Task<UpsertPensionsResult> Handle(UpsertPensionsCommand request, CancellationToken cancellationToken)
    {
        LogStartingUpsert(logger, request.UserId, request.HasPensions, request.Pensions.Count);

        try
        {
            if (!request.HasPensions)
            {
                await pensionRepository.DeleteByUserIdAsync(request.UserId);
                LogDeletedAllPensions(logger, request.UserId);

                return new UpsertPensionsResult { Success = true, PensionIds = [] };
            }

            foreach (var pensionDto in request.Pensions)
            {
                if (string.IsNullOrWhiteSpace(pensionDto.EmployerName))
                {
                    LogValidationFailed(logger, "Employer name is required");
                    return new UpsertPensionsResult { Success = false, ErrorMessage = "Employer name is required." };
                }

                if (pensionDto.PensionTypeId <= 0)
                {
                    LogValidationFailed(logger, "A valid pension type must be selected");
                    return new UpsertPensionsResult { Success = false, ErrorMessage = "A valid pension type must be selected." };
                }
            }

            var pensions = request.Pensions.Select(p => new BeginnerGuidePension
            {
                UserId = request.UserId,
                PensionTypeId = p.PensionTypeId,
                EmployerName = p.EmployerName
            }).ToList();

            var savedPensions = await pensionRepository.UpsertPensionsAsync(request.UserId, pensions);

            LogUpsertSuccessful(logger, request.UserId, savedPensions.Count);

            return new UpsertPensionsResult { Success = true, PensionIds = savedPensions.Select(p => p.Id).ToList() };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new UpsertPensionsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving your pensions. Please try again."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpsertPensions for UserId: {UserId}, HasPensions: {HasPensions}, Count: {Count}")]
    static partial void LogStartingUpsert(ILogger logger, long userId, bool hasPensions, int count);

    [LoggerMessage(LogLevel.Information, "Deleted all pensions for UserId: {UserId}")]
    static partial void LogDeletedAllPensions(ILogger logger, long userId);

    [LoggerMessage(LogLevel.Warning, "Validation failed: {Reason}")]
    static partial void LogValidationFailed(ILogger logger, string reason);

    [LoggerMessage(LogLevel.Information, "Successfully upserted pensions for UserId: {UserId}, Count: {Count}")]
    static partial void LogUpsertSuccessful(ILogger logger, long userId, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger logger, string exception, long userId);
}


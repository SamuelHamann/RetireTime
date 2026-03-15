using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.BeginnerGuide.Benefits;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Benefits.UpsertGovernmentPension;

public partial class UpsertGovernmentPensionHandler(
    IBeginnerGuideGovernmentPensionRepository governmentPensionRepository,
    ILogger<UpsertGovernmentPensionHandler> logger) : IRequestHandler<UpsertGovernmentPensionCommand, UpsertGovernmentPensionResult>
{
    public async Task<UpsertGovernmentPensionResult> Handle(UpsertGovernmentPensionCommand request, CancellationToken cancellationToken)
    {
        LogStartingUpsert(logger, request.UserId);

        try
        {
            if (request.YearsWorked < 0 || request.YearsWorked > 100)
            {
                LogValidationFailed(logger, "Years worked must be between 0 and 100");
                return new UpsertGovernmentPensionResult { Success = false, ErrorMessage = "Years worked must be between 0 and 100." };
            }

            if (request.HasSpecializedPublicSectorPension && string.IsNullOrWhiteSpace(request.SpecializedPensionName))
            {
                LogValidationFailed(logger, "Specialized pension name is required");
                return new UpsertGovernmentPensionResult { Success = false, ErrorMessage = "Specialized pension name is required." };
            }

            if (request.HasSpecializedPublicSectorPension &&
                request.SpecializedPensionName!.Length > 200)
            {
                LogValidationFailed(logger, "Specialized pension name cannot exceed 200 characters");
                return new UpsertGovernmentPensionResult { Success = false, ErrorMessage = "Specialized pension name cannot exceed 200 characters." };
            }

            var governmentPension = new BeginnerGuideGovernmentPension
            {
                UserId = request.UserId,
                YearsWorked = request.YearsWorked,
                HasSpecializedPublicSectorPension = request.HasSpecializedPublicSectorPension,
                SpecializedPensionName = request.HasSpecializedPublicSectorPension ? request.SpecializedPensionName : null
            };

            var saved = await governmentPensionRepository.UpsertAsync(governmentPension);

            LogUpsertSuccessful(logger, request.UserId, saved.Id);

            return new UpsertGovernmentPensionResult { Success = true, Id = saved.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new UpsertGovernmentPensionResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving your government pension data. Please try again."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpsertGovernmentPension for UserId: {UserId}")]
    static partial void LogStartingUpsert(ILogger<UpsertGovernmentPensionHandler> logger, long userId);

    [LoggerMessage(LogLevel.Warning, "Validation failed: {Reason}")]
    static partial void LogValidationFailed(ILogger<UpsertGovernmentPensionHandler> logger, string reason);

    [LoggerMessage(LogLevel.Information, "Successfully upserted government pension for UserId: {UserId}, Id: {Id}")]
    static partial void LogUpsertSuccessful(ILogger<UpsertGovernmentPensionHandler> logger, long userId, long id);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpsertGovernmentPensionHandler> logger, string exception, long userId);
}


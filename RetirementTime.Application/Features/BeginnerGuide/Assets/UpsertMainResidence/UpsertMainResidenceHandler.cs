using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.UpsertMainResidence;

public partial class UpsertMainResidenceHandler(
    IMainResidenceRepository mainResidenceRepository,
    IUserRepository userRepository,
    ILogger<UpsertMainResidenceHandler> logger) : IRequestHandler<UpsertMainResidenceCommand, UpsertMainResidenceResult>
{
    public async Task<UpsertMainResidenceResult> Handle(UpsertMainResidenceCommand request, CancellationToken cancellationToken)
    {
        LogStartingUpsertMainResidenceHandler(logger, request.UserId);

        try
        {
            // Verify user exists
            var user = await userRepository.GetUserById(request.UserId);
            if (user == null)
            {
                LogUserNotFound(logger, request.UserId);
                return new UpsertMainResidenceResult
                {
                    Success = false,
                    ErrorMessage = "User not found."
                };
            }

            // Validate required fields if HasMainResidence is true
            if (request.HasMainResidence)
            {
                if (!request.PurchasePrice.HasValue || !request.MonthlyMortgagePayments.HasValue || 
                    !request.MortgageLeft.HasValue || !request.YearlyInsurance.HasValue || 
                    !request.MortgageDuration.HasValue || !request.MortgageStartDate.HasValue)
                {
                    LogValidationFailed(logger, request.UserId);
                    return new UpsertMainResidenceResult
                    {
                        Success = false,
                        ErrorMessage = "Please fill in all required fields."
                    };
                }
            }

            // Check if record already exists
            var existingMainResidence = await mainResidenceRepository.GetByUserIdAsync(request.UserId);

            // Convert MortgageStartDate to UTC if it has a value
            DateTime? mortgageStartDateUtc = request.MortgageStartDate.HasValue
                ? DateTime.SpecifyKind(request.MortgageStartDate.Value, DateTimeKind.Utc)
                : null;

            MainResidence mainResidence;
            
            if (existingMainResidence != null)
            {
                // Update existing record
                existingMainResidence.HasMainResidence = request.HasMainResidence;
                existingMainResidence.PurchasePrice = request.PurchasePrice;
                existingMainResidence.MonthlyMortgagePayments = request.MonthlyMortgagePayments;
                existingMainResidence.MortgageLeft = request.MortgageLeft;
                existingMainResidence.YearlyInsurance = request.YearlyInsurance;
                existingMainResidence.MonthlyElectricityCosts = request.MonthlyElectricityCosts;
                existingMainResidence.MortgageDuration = request.MortgageDuration;
                existingMainResidence.MortgageStartDate = mortgageStartDateUtc;
                existingMainResidence.EstimatedValue = request.EstimatedValue;

                mainResidence = await mainResidenceRepository.UpdateAsync(existingMainResidence);
                LogSuccessfullyUpdatedMainResidence(logger, mainResidence.Id, request.UserId);
            }
            else
            {
                // Create new record
                var newMainResidence = new MainResidence
                {
                    UserId = request.UserId,
                    HasMainResidence = request.HasMainResidence,
                    PurchasePrice = request.PurchasePrice,
                    MonthlyMortgagePayments = request.MonthlyMortgagePayments,
                    MortgageLeft = request.MortgageLeft,
                    YearlyInsurance = request.YearlyInsurance,
                    MonthlyElectricityCosts = request.MonthlyElectricityCosts,
                    MortgageDuration = request.MortgageDuration,
                    MortgageStartDate = mortgageStartDateUtc,
                    EstimatedValue = request.EstimatedValue,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                mainResidence = await mainResidenceRepository.CreateAsync(newMainResidence);
                LogSuccessfullyCreatedMainResidence(logger, mainResidence.Id, request.UserId);
            }

            return new UpsertMainResidenceResult
            {
                Success = true,
                MainResidenceId = mainResidence.Id
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new UpsertMainResidenceResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving your data. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpsertMainResidence handler for UserId: {userId}")]
    static partial void LogStartingUpsertMainResidenceHandler(ILogger<UpsertMainResidenceHandler> logger, long userId);

    [LoggerMessage(LogLevel.Error, "User not found for UserId: {userId}")]
    static partial void LogUserNotFound(ILogger<UpsertMainResidenceHandler> logger, long userId);

    [LoggerMessage(LogLevel.Warning, "Validation failed for UserId: {userId}")]
    static partial void LogValidationFailed(ILogger<UpsertMainResidenceHandler> logger, long userId);

    [LoggerMessage(LogLevel.Information, "Successfully created MainResidence with ID: {mainResidenceId} for UserId: {userId}")]
    static partial void LogSuccessfullyCreatedMainResidence(ILogger<UpsertMainResidenceHandler> logger, long mainResidenceId, long userId);

    [LoggerMessage(LogLevel.Information, "Successfully updated MainResidence with ID: {mainResidenceId} for UserId: {userId}")]
    static partial void LogSuccessfullyUpdatedMainResidence(ILogger<UpsertMainResidenceHandler> logger, long mainResidenceId, long userId);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {userId} | Exception: {exception}")]
    static partial void LogErrorOccurred(ILogger<UpsertMainResidenceHandler> logger, string exception, long userId);
}


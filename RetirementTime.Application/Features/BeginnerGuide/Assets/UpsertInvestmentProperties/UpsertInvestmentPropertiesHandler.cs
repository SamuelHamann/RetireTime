using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.UpsertInvestmentProperties;

public partial class UpsertInvestmentPropertiesHandler(
    IInvestmentPropertyRepository investmentPropertyRepository,
    ILogger<UpsertInvestmentPropertiesHandler> logger) : IRequestHandler<UpsertInvestmentPropertiesCommand, UpsertInvestmentPropertiesResult>
{
    public async Task<UpsertInvestmentPropertiesResult> Handle(UpsertInvestmentPropertiesCommand request, CancellationToken cancellationToken)
    {
        LogStartingUpsert(logger, request.UserId, request.HasInvestmentProperties, request.Properties.Count);

        try
        {
            // If user doesn't have investment properties, delete all existing properties
            if (!request.HasInvestmentProperties)
            {
                await investmentPropertyRepository.DeleteByUserIdAsync(request.UserId);
                LogDeletedAllProperties(logger, request.UserId);
                
                return new UpsertInvestmentPropertiesResult
                {
                    Success = true,
                    PropertyIds = []
                };
            }

            // Validate all properties
            foreach (var propertyDto in request.Properties)
            {
                if (string.IsNullOrWhiteSpace(propertyDto.Name))
                {
                    LogValidationFailed(logger, "Property name is required");
                    return new UpsertInvestmentPropertiesResult
                    {
                        Success = false,
                        ErrorMessage = "Please enter a name for all investment properties."
                    };
                }

                if (propertyDto.PurchasePrice <= 0)
                {
                    LogValidationFailed(logger, "Invalid purchase price");
                    return new UpsertInvestmentPropertiesResult
                    {
                        Success = false,
                        ErrorMessage = "Please enter a valid purchase price for all properties."
                    };
                }

                if (propertyDto.MonthlyMortgagePayments < 0)
                {
                    LogValidationFailed(logger, "Invalid monthly mortgage payments");
                    return new UpsertInvestmentPropertiesResult
                    {
                        Success = false,
                        ErrorMessage = "Monthly mortgage payments cannot be negative."
                    };
                }

                if (propertyDto.MortgageLeft < 0)
                {
                    LogValidationFailed(logger, "Invalid mortgage left");
                    return new UpsertInvestmentPropertiesResult
                    {
                        Success = false,
                        ErrorMessage = "Mortgage left cannot be negative."
                    };
                }

                if (propertyDto.YearlyInsurance < 0)
                {
                    LogValidationFailed(logger, "Invalid yearly insurance");
                    return new UpsertInvestmentPropertiesResult
                    {
                        Success = false,
                        ErrorMessage = "Yearly insurance cannot be negative."
                    };
                }

                if (propertyDto.MortgageDuration <= 0)
                {
                    LogValidationFailed(logger, "Invalid mortgage duration");
                    return new UpsertInvestmentPropertiesResult
                    {
                        Success = false,
                        ErrorMessage = "Please enter a valid mortgage duration for all properties."
                    };
                }

                if (propertyDto.MonthlyCost < 0)
                {
                    LogValidationFailed(logger, "Invalid monthly cost");
                    return new UpsertInvestmentPropertiesResult
                    {
                        Success = false,
                        ErrorMessage = "Monthly cost cannot be negative."
                    };
                }

                if (propertyDto.MonthlyRevenue < 0)
                {
                    LogValidationFailed(logger, "Invalid monthly revenue");
                    return new UpsertInvestmentPropertiesResult
                    {
                        Success = false,
                        ErrorMessage = "Monthly revenue cannot be negative."
                    };
                }
            }
            
            // Prepare all properties for bulk insert
            var properties = request.Properties.Select(propertyDto => new BeginnerGuideInvestmentProperty
            {
                UserId = request.UserId,
                Name = propertyDto.Name,
                PurchasePrice = propertyDto.PurchasePrice,
                MonthlyMortgagePayments = propertyDto.MonthlyMortgagePayments,
                MortgageLeft = propertyDto.MortgageLeft,
                YearlyInsurance = propertyDto.YearlyInsurance,
                MonthlyElectricityCosts = propertyDto.MonthlyElectricityCosts,
                MortgageDuration = propertyDto.MortgageDuration,
                MortgageStartDate = DateTime.SpecifyKind(propertyDto.MortgageStartDate, DateTimeKind.Utc),
                EstimatedValue = propertyDto.EstimatedValue,
                MonthlyCost = propertyDto.MonthlyCost,
                MonthlyRevenue = propertyDto.MonthlyRevenue,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                User = null
            }).ToList();

            // Execute delete and inserts in a single transaction
            var savedProperties = await investmentPropertyRepository.UpsertPropertiesAsync(request.UserId, properties);
            var propertyIds = savedProperties.Select(p => p.Id).ToList();

            LogUpsertSuccessful(logger, request.UserId, propertyIds.Count);

            return new UpsertInvestmentPropertiesResult
            {
                Success = true,
                PropertyIds = propertyIds
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new UpsertInvestmentPropertiesResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving your investment properties. Please try again."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting upsert for UserId: {UserId}, HasInvestmentProperties: {HasInvestmentProperties}, PropertyCount: {PropertyCount}")]
    static partial void LogStartingUpsert(ILogger logger, long userId, bool hasInvestmentProperties, int propertyCount);

    [LoggerMessage(LogLevel.Information, "Deleted all investment properties for UserId: {UserId}")]
    static partial void LogDeletedAllProperties(ILogger logger, long userId);

    [LoggerMessage(LogLevel.Warning, "Validation failed: {Reason}")]
    static partial void LogValidationFailed(ILogger logger, string reason);

    [LoggerMessage(LogLevel.Information, "Successfully upserted investment properties for UserId: {UserId}, Count: {Count}")]
    static partial void LogUpsertSuccessful(ILogger logger, long userId, int count);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger logger, string exception, long userId);
}


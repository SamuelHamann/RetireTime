using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.BeginnerGuide.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Income.UpsertSelfEmployments;

public partial class UpsertSelfEmploymentsHandler(
    ISelfEmploymentRepository repository,
    ILogger<UpsertSelfEmploymentsHandler> logger) : IRequestHandler<UpsertSelfEmploymentsCommand, UpsertSelfEmploymentsResult>
{
    public async Task<UpsertSelfEmploymentsResult> Handle(UpsertSelfEmploymentsCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.UserId);

        try
        {
            // Convert DTOs to entities
            var selfEmployments = request.SelfEmployments.Select(dto => new BeginnerGuideSelfEmployment
            {
                UserId = request.UserId,
                BusinessName = dto.BusinessName,
                AnnualSalary = dto.AnnualSalary,
                AverageAnnualRevenueIncrease = dto.AverageAnnualRevenueIncrease,
                MonthlyDividends = dto.MonthlyDividends,
                AdditionalCompensations = dto.AdditionalCompensations.Select(comp => new BeginnerGuideSelfEmploymentAdditionalCompensation
                {
                    Name = comp.Name,
                    Amount = comp.Amount,
                    FrequencyId = comp.FrequencyId,
                    SelfEmploymentId = 0 // Will be set by EF Core
                }).ToList()
            }).ToList();

            // Upsert self-employments
            var savedSelfEmployments = await repository.UpsertSelfEmploymentsAsync(request.UserId, selfEmployments);
            
            LogSuccessfullyCompleted(logger, request.UserId, savedSelfEmployments.Count);
            
            return new UpsertSelfEmploymentsResult
            {
                Success = true,
                UserId = request.UserId,
                SelfEmploymentCount = savedSelfEmployments.Count
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new UpsertSelfEmploymentsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving self-employment data. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpsertSelfEmployments handler for UserId: {UserId}")]
    static partial void LogStartingHandler(ILogger<UpsertSelfEmploymentsHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully upserted {Count} self-employments for UserId: {UserId}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpsertSelfEmploymentsHandler> logger, long UserId, int Count);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpsertSelfEmploymentsHandler> logger, string Exception, long UserId);
}

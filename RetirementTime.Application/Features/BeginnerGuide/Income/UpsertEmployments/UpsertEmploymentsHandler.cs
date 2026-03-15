using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Entities.BeginnerGuide.Income;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.BeginnerGuide.Income.UpsertEmployments;

public partial class UpsertEmploymentsHandler(
    IEmploymentRepository repository,
    ILogger<UpsertEmploymentsHandler> logger) : IRequestHandler<UpsertEmploymentsCommand, UpsertEmploymentsResult>
{
    public async Task<UpsertEmploymentsResult> Handle(UpsertEmploymentsCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.UserId);

        try
        {
            // Convert DTOs to entities
            var employments = request.Employments.Select(dto => new BeginnerGuideEmployment
            {
                UserId = request.UserId,
                EmployerName = dto.EmployerName,
                AnnualSalary = dto.AnnualSalary,
                AverageAnnualWageIncrease = dto.AverageAnnualWageIncrease,
                AdditionalCompensations = dto.AdditionalCompensations.Select(comp => new BeginnerGuideAdditionalCompensation
                {
                    Name = comp.Name,
                    Amount = comp.Amount,
                    FrequencyId = comp.FrequencyId,
                    EmploymentId = 0 // Will be set by EF Core
                }).ToList()
            }).ToList();

            // Upsert employments
            var savedEmployments = await repository.UpsertEmploymentsAsync(request.UserId, employments);
            
            LogSuccessfullyCompleted(logger, request.UserId, savedEmployments.Count);
            
            return new UpsertEmploymentsResult
            {
                Success = true,
                UserId = request.UserId,
                EmploymentCount = savedEmployments.Count
            };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.UserId);
            return new UpsertEmploymentsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while saving employment data. Please try again later."
            };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting UpsertEmployments handler for UserId: {UserId}")]
    static partial void LogStartingHandler(ILogger<UpsertEmploymentsHandler> logger, long UserId);

    [LoggerMessage(LogLevel.Information, "Successfully upserted {Count} employments for UserId: {UserId}")]
    static partial void LogSuccessfullyCompleted(ILogger<UpsertEmploymentsHandler> logger, long UserId, int Count);

    [LoggerMessage(LogLevel.Error, "Error occurred for UserId: {UserId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<UpsertEmploymentsHandler> logger, string Exception, long UserId);
}

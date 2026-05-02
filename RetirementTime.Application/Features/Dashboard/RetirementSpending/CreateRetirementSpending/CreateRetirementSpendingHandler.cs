using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.RetirementSpending.CreateRetirementSpending;

public partial class CreateRetirementSpendingHandler(
    IRetirementSpendingRepository repository,
    IDashboardScenarioRepository scenarioRepository,
    IOnboardingEmploymentRepository employmentRepository,
    ILogger<CreateRetirementSpendingHandler> logger) : IRequestHandler<CreateRetirementSpendingCommand, CreateRetirementSpendingResult>
{
    public async Task<CreateRetirementSpendingResult> Handle(CreateRetirementSpendingCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);
        try
        {
            int defaultAge = 65;
            var scenario = await scenarioRepository.GetByIdAsync(request.ScenarioId);
            if (scenario is not null)
            {
                var employment = await employmentRepository.GetByUserId(scenario.UserId);
                if (employment?.PlannedRetirementAge is > 0)
                    defaultAge = employment.PlannedRetirementAge.Value;
            }

            var existing = await repository.GetByScenarioIdAsync(request.ScenarioId);
            int ageFrom = existing.Count > 0 ? existing.Max(e => e.AgeTo) + 1 : defaultAge;
            int ageTo   = ageFrom + 10;

            var entity = new Domain.Entities.Dashboard.Spending.RetirementSpending
            {
                ScenarioId = request.ScenarioId,
                Name       = request.Name,
                AgeFrom    = ageFrom,
                AgeTo      = ageTo
            };
            var created = await repository.CreateAsync(entity);
            LogSuccessfullyCompleted(logger, created.Id, request.ScenarioId);
            return new CreateRetirementSpendingResult { Success = true, RetirementSpendingId = created.Id };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new CreateRetirementSpendingResult { Success = false, ErrorMessage = "An error occurred. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting CreateRetirementSpending for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<CreateRetirementSpendingHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Created RetirementSpending {Id} for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<CreateRetirementSpendingHandler> logger, long Id, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error creating RetirementSpending for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<CreateRetirementSpendingHandler> logger, string Exception, long ScenarioId);
}

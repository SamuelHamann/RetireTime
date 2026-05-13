using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Entities.Dashboard.Spending;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Spending.SaveDiscretionaryExpenses;

public partial class SaveDiscretionaryExpensesHandler(
    ISpendingRepository repository,
    ILogger<SaveDiscretionaryExpensesHandler> logger) : IRequestHandler<SaveDiscretionaryExpensesCommand, BaseResult>
{
    public async Task<BaseResult> Handle(SaveDiscretionaryExpensesCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.ScenarioId);

        try
        {
            var entity = new SpendingDiscretionaryExpenses
            {
                ScenarioId                         = request.ScenarioId,
                RetirementTimelineId               = request.TimelineId,
                GymMembership                      = request.GymMembership,
                GymMembershipFrequencyId           = request.GymMembershipFrequencyId,
                Subscriptions                      = request.Subscriptions,
                SubscriptionsFrequencyId           = request.SubscriptionsFrequencyId,
                EatingOut                          = request.EatingOut,
                EatingOutFrequencyId               = request.EatingOutFrequencyId,
                Entertainment                      = request.Entertainment,
                EntertainmentFrequencyId           = request.EntertainmentFrequencyId,
                Travel                             = request.Travel,
                TravelFrequencyId                  = request.TravelFrequencyId,
                CharitableDonations                = request.CharitableDonations,
                CharitableDonationsFrequencyId     = request.CharitableDonationsFrequencyId,
                OtherDiscretionaryExpenses         = request.OtherDiscretionaryExpenses,
                OtherDiscretionaryExpensesFrequencyId = request.OtherDiscretionaryExpensesFrequencyId,
                UseGroupedEntry                    = request.UseGroupedEntry,
                GroupedAmount                      = request.GroupedAmount,
                GroupedFrequencyId                 = request.GroupedFrequencyId,
            };

            await repository.UpsertDiscretionaryExpensesAsync(entity);

            LogSuccessfullyCompleted(logger, request.ScenarioId);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.ScenarioId);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while saving. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting SaveDiscretionaryExpenses handler for ScenarioId: {ScenarioId}")]
    static partial void LogStartingHandler(ILogger<SaveDiscretionaryExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Information, "Successfully saved discretionary expenses for ScenarioId: {ScenarioId}")]
    static partial void LogSuccessfullyCompleted(ILogger<SaveDiscretionaryExpensesHandler> logger, long ScenarioId);

    [LoggerMessage(LogLevel.Error, "Error saving discretionary expenses for ScenarioId: {ScenarioId} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<SaveDiscretionaryExpensesHandler> logger, string Exception, long ScenarioId);
}

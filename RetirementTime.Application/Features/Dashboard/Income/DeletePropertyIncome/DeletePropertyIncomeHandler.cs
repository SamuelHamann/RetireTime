using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Income.DeletePropertyIncome;

public partial class DeletePropertyIncomeHandler(
    IPropertyIncomeRepository repository,
    ILogger<DeletePropertyIncomeHandler> logger) : IRequestHandler<DeletePropertyIncomeCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeletePropertyIncomeCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var success = await repository.DeleteAsync(request.Id);
            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = success };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while deleting the property income. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeletePropertyIncome handler for Id: {Id}")]
    static partial void LogStartingHandler(ILogger<DeletePropertyIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted property income with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeletePropertyIncomeHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while deleting property income with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeletePropertyIncomeHandler> logger, string Exception, long Id);
}


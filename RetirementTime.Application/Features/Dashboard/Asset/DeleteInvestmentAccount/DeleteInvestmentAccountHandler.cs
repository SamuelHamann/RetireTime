using MediatR;
using Microsoft.Extensions.Logging;
using RetirementTime.Application.Common;
using RetirementTime.Domain.Interfaces.Repositories;

namespace RetirementTime.Application.Features.Dashboard.Asset.DeleteInvestmentAccount;

public partial class DeleteInvestmentAccountHandler(
    IAssetsInvestmentAccountRepository repository,
    ILogger<DeleteInvestmentAccountHandler> logger) : IRequestHandler<DeleteInvestmentAccountCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeleteInvestmentAccountCommand request, CancellationToken cancellationToken)
    {
        LogStartingHandler(logger, request.Id);

        try
        {
            var success = await repository.DeleteAsync(request.Id);

            if (!success) { LogNotFound(logger, request.Id); return new BaseResult { Success = false, ErrorMessage = "Account record not found." }; }

            LogSuccessfullyCompleted(logger, request.Id);
            return new BaseResult { Success = true };
        }
        catch (Exception ex)
        {
            LogErrorOccurred(logger, ex.Message, request.Id);
            return new BaseResult { Success = false, ErrorMessage = "An error occurred while deleting the account. Please try again later." };
        }
    }

    [LoggerMessage(LogLevel.Information, "Starting DeleteInvestmentAccount handler for ID: {Id}")]
    static partial void LogStartingHandler(ILogger<DeleteInvestmentAccountHandler> logger, long Id);

    [LoggerMessage(LogLevel.Warning, "Investment account not found for ID: {Id}")]
    static partial void LogNotFound(ILogger<DeleteInvestmentAccountHandler> logger, long Id);

    [LoggerMessage(LogLevel.Information, "Successfully deleted investment account with ID: {Id}")]
    static partial void LogSuccessfullyCompleted(ILogger<DeleteInvestmentAccountHandler> logger, long Id);

    [LoggerMessage(LogLevel.Error, "Error occurred while deleting investment account with ID: {Id} | Exception: {Exception}")]
    static partial void LogErrorOccurred(ILogger<DeleteInvestmentAccountHandler> logger, string Exception, long Id);
}

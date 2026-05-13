using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.RetirementSpending.DeleteRetirementSpending;

public record DeleteRetirementSpendingCommand(long Id) : IRequest<BaseResult>;


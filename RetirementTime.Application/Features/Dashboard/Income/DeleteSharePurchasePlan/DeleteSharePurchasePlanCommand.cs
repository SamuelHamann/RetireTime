using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.DeleteSharePurchasePlan;

public record DeleteSharePurchasePlanCommand(long Id) : IRequest<BaseResult>;

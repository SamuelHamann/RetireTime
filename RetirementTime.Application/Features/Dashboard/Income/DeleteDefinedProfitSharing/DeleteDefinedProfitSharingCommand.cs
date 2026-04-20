using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.DeleteDefinedProfitSharing;

public record DeleteDefinedProfitSharingCommand(long Id) : IRequest<BaseResult>;

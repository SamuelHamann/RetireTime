using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Asset.DeleteHolding;

public record DeleteHoldingCommand(long Id) : IRequest<BaseResult>;

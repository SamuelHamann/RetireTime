using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Asset.CreateHolding;

public record CreateHoldingCommand(long AccountId) : IRequest<CreateHoldingResult>;

public record CreateHoldingResult : BaseResult
{
    public long HoldingId { get; init; }
}

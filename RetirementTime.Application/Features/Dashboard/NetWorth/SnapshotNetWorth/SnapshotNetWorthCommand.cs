using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.NetWorth.SnapshotNetWorth;

public record SnapshotNetWorthCommand : IRequest<SnapshotNetWorthResult>;

public record SnapshotNetWorthResult : BaseResult
{
    public int ScenariosProcessed { get; init; }
}

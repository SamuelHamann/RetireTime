using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.UserProgress.GetUserProgress;

public record GetUserProgressQuery : IRequest<GetUserProgressResult>
{
    public required long UserId { get; init; }
}

public record GetUserProgressResult : BaseResult
{
    public bool HasFinishedBeginnerGuide { get; init; }
}


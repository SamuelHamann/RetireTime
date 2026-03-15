using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.UserProgress.CompleteBeginnerGuide;

public record CompleteBeginnerGuideCommand : IRequest<CompleteBeginnerGuideResult>
{
    public required long UserId { get; init; }
}

public record CompleteBeginnerGuideResult : BaseResult;


using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Users.CompleteIntro;

public record CompleteIntroCommand : IRequest<CompleteIntroResult>
{
    public required long UserId { get; init; }
    public bool HasCompletedIntro { get; init; } = true;
}

public record CompleteIntroResult : BaseResult;


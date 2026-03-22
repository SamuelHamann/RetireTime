using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Users.GetIntroStatus;

public record GetIntroStatusQuery : IRequest<GetIntroStatusResult>
{
    public required long UserId { get; init; }
}

public record GetIntroStatusResult : BaseResult
{
    public bool HasCompletedIntro { get; init; }
}


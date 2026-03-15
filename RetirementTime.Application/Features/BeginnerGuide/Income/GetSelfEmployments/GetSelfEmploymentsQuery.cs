using MediatR;

namespace RetirementTime.Application.Features.BeginnerGuide.Income.GetSelfEmployments;

public class GetSelfEmploymentsQuery : IRequest<GetSelfEmploymentsResult>
{
    public required long UserId { get; set; }
}

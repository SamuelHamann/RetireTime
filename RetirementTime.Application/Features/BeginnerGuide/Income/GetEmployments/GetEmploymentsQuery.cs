using MediatR;

namespace RetirementTime.Application.Features.BeginnerGuide.Income.GetEmployments;

public class GetEmploymentsQuery : IRequest<GetEmploymentsResult>
{
    public required long UserId { get; set; }
}

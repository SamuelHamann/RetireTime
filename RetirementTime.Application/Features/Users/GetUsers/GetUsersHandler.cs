using MediatR;
using RetirementTime.Application.Features.Users.DTOs;

namespace RetirementTime.Application.Features.Users.GetUsers;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, List<GetUserDTO>>
{
    public async Task<List<GetUserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
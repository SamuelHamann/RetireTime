using MediatR;
using RetirementTime.Application.Features.Users.DTOs;

namespace RetirementTime.Application.Features.Users.GetUsers;

public record GetUsersQuery : IRequest<List<GetUserDTO>>;
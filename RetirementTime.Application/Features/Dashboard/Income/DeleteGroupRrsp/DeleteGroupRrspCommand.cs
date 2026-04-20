using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.DeleteGroupRrsp;

public record DeleteGroupRrspCommand(long Id) : IRequest<BaseResult>;

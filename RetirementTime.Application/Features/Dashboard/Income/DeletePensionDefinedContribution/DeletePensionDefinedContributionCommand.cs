using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.DeletePensionDefinedContribution;

public record DeletePensionDefinedContributionCommand(long Id) : IRequest<BaseResult>;

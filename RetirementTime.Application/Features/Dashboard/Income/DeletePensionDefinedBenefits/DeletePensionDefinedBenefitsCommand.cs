using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.DeletePensionDefinedBenefits;

public record DeletePensionDefinedBenefitsCommand(long Id) : IRequest<BaseResult>;

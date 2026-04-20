using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.DeleteOtherIncomeOrBenefits;

public record DeleteOtherIncomeOrBenefitsCommand(long Id) : IRequest<BaseResult>;

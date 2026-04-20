using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.DeleteSelfEmploymentIncome;

public record DeleteSelfEmploymentIncomeCommand(long Id) : IRequest<BaseResult>;

using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.DeleteEmploymentIncome;

public record DeleteEmploymentIncomeCommand(long Id) : IRequest<BaseResult>;

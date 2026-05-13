using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.DeletePropertyIncome;

public record DeletePropertyIncomeCommand(long Id) : IRequest<BaseResult>;


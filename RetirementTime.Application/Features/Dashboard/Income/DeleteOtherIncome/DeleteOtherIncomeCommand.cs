using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Income.DeleteOtherIncome;

public record DeleteOtherIncomeCommand(long Id) : IRequest<BaseResult>;

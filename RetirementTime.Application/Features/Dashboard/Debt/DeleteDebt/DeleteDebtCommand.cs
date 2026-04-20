using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.Debt.DeleteDebt;

public record DeleteDebtCommand(long Id) : IRequest<BaseResult>;

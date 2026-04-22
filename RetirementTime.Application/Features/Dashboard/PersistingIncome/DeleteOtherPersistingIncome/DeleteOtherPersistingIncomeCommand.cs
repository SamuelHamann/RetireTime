using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.DeleteOtherPersistingIncome;

public record DeleteOtherPersistingIncomeCommand(long IncomeId) : IRequest<DeleteOtherPersistingIncomeResult>;

public record DeleteOtherPersistingIncomeResult : BaseResult;


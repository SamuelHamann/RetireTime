using MediatR;
using RetirementTime.Application.Common;

namespace RetirementTime.Application.Features.Dashboard.PersistingIncome.DeleteRealEstateIncome;

public record DeleteRealEstateIncomeCommand(long IncomeId) : IRequest<DeleteRealEstateIncomeResult>;

public record DeleteRealEstateIncomeResult : BaseResult;


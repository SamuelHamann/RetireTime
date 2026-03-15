using MediatR;

namespace RetirementTime.Application.Features.BeginnerGuide.Assets.GetInvestmentProperties;

public class GetInvestmentPropertiesQuery : IRequest<List<InvestmentPropertyDto>>
{
    public long UserId { get; set; }
}


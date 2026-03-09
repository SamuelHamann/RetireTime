using MediatR;

namespace RetirementTime.Application.Features.Common.GetFrequencies;

public class GetFrequenciesQuery : IRequest<List<FrequencyDto>>
{
}

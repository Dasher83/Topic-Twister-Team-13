using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.Services
{
    public class CreateRoundGatewayService : ICreateRoundGatewayService
    {
        private ICreateRoundSubUseCase _useCase;

        public CreateRoundGatewayService(ICreateRoundSubUseCase useCase)
        {
            _useCase = useCase;
        }

        public RoundWithCategoriesDto Create(MatchDto matchDto)
        {
            Operation<RoundWithCategoriesDto> useCaseResult = _useCase.Create(matchDto);
            return useCaseResult.WasOk ? useCaseResult.Outcome : null;
        }
    }
}

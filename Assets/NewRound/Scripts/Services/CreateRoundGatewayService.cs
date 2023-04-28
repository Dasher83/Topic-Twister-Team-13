using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.Services
{
    public class CreateRoundGatewayService : ICreateRoundGatewayService
    {
        private ICreateRoundUseCase _useCase;

        public CreateRoundGatewayService(ICreateRoundUseCase useCase)
        {
            _useCase = useCase;
        }

        public RoundWithCategoriesDto Create(MatchDTO matchDto)
        {
            Result<RoundWithCategoriesDto> useCaseResult = _useCase.Create(matchDto);
            return useCaseResult.WasOk ? useCaseResult.Outcome : null;
        }
    }
}

using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.DTOs;


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
            return _useCase.Create(matchDto);
        }
    }
}

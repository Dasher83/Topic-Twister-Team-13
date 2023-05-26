using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.PlayTurn.Services
{
    public class EndTurnGategayService : IEndTurnGatewayService
    {
        IEndTurnUseCase _useCase;

        private EndTurnGategayService() { }

        public EndTurnGategayService(IEndTurnUseCase useCase)
        {
            _useCase = useCase;
        }

        public EndOfTurnDto EndTurn(int userId, int matchId, AnswerDto[] answerDtos)
        {
            Operation<EndOfTurnDto> useCaseOperation = _useCase
                .Execute(userId: userId, matchId: matchId, answerDtos: answerDtos);

            return useCaseOperation.WasOk ? useCaseOperation.Result : null;
        }
    }
}

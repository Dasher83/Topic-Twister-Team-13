using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.PlayTurn.Services
{
    public class StartTurnGatewayService : IStartTurnGatewayService
    {
        private IStartTurnUseCase _useCase;

        public StartTurnGatewayService(IStartTurnUseCase useCase)
        {
            _useCase = useCase;
        }

        public TurnDto StartTurn(int userId, int matchId)
        {
            Operation<TurnDto> useCaseOperation = _useCase.Execute(userId: userId, matchId: matchId);
            return useCaseOperation.WasOk ? useCaseOperation.Result : null;
        }
    }
}

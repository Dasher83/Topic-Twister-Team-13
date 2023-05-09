using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.PlayTurn.Shared.Interfaces
{
    public interface IStartTurnUseCase
    {
        Operation<TurnDto> Execute(int userId, int matchId);
    }
}

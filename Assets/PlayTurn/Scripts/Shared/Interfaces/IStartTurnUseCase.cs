using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.PlayTurn.Shared.Interfaces
{
    public interface IStartTurnUseCase
    {
        Operation<bool> Execute(int userId, MatchDto matchDto);
    }
}
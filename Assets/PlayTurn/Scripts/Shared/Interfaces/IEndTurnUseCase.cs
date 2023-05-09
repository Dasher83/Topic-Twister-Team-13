using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.PlayTurn.Shared.Interfaces
{
    public interface IEndTurnUseCase
    {
        Operation<TurnWithEvaluatedAnswersDto> Execute(int userId, int matchId, AnswerDto[] answerDtos);
    }
}
using TopicTwister.Shared.DTOs;


namespace TopicTwister.PlayTurn.Shared.Interfaces
{
    public interface IEndTurnPresenter
    {
        void UpdateView(EndOfTurnDto endOfTurnDto);
        void EndTurnEventHandler(int userId, int roundId, AnswerDto[] answerDtos);
    }
}

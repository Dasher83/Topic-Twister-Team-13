using TopicTwister.Shared.DTOs;


namespace TopicTwister.PlayTurn.Shared.Interfaces
{
    public interface IEndTurnGatewayService
    {
        EndOfTurnDto EndTurn(int userId, int matchId, AnswerDto[] answerDtos);
    }
}

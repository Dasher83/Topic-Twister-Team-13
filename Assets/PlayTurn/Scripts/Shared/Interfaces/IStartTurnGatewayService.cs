using TopicTwister.Shared.DTOs;


namespace TopicTwister.PlayTurn.Shared.Interfaces
{
    public interface IStartTurnGatewayService
    {
        TurnDto StartTurn(int userId, int matchId);
    }
}

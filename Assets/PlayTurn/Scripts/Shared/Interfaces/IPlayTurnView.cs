using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.PlayTurn.Shared.Interfaces
{
    public interface IPlayTurnView
    {
        event EventDelegates.IPlayTurnView.LoadEventHandler Load;
        event EventDelegates.IPlayTurnView.EndTurnEventHandler EndTurn;
        void ReceiveUpdateFromStartTurn(TurnDto turnDto);
        void ReceiveUpdateFromEndTurn(EndOfTurnDto endOfTurnDto);
    }
}

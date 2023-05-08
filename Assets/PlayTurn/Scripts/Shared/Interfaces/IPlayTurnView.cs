using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.PlayTurn.Views
{
    public interface IPlayTurnView
    {
        event EventDelegates.PlayTurnView.LoadEventHandler OnLoad;
        void ReceiveUpdate(TurnDto turnDto);
    }
}
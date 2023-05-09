using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.PlayTurn.Views
{
    public interface IPlayTurnView
    {
        event EventDelegates.IPlayTurnView.LoadEventHandler Load;
        void ReceiveUpdate(TurnDto turnDto);
    }
}

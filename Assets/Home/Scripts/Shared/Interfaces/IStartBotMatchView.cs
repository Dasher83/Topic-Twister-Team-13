using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.Home.Shared.Interfaces
{
    public interface IStartBotMatchView
    {
        event EventDelegates.IStartBotMatchView.StartMatchVersusBotEventHandler StartMatchVersusBot;
        void ReceiveUpdate(MatchDto matchDto);
    }
}

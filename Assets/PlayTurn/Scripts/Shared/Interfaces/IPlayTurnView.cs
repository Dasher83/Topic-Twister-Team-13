using System;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.PlayTurn.Views
{
    public interface IPlayTurnView
    {
        event EventDelegates.PlayTurnView.LoadEventHandler OnLoad;
        void ReceiveUpdate(TurnDto turn);
    }
}
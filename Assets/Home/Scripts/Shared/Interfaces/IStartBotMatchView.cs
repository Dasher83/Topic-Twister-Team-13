using System;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.Home.Shared.Interfaces
{
    public interface IStartBotMatchView
    {
        event Action StartMatchVersusBot;
        void ReceiveUpdate(MatchDto matchDto);
    }
}

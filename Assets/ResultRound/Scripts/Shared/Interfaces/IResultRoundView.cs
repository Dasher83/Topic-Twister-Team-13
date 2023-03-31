using System;


namespace TopicTwister.ResultRound.Shared.Interfaces
{
    public interface IResultRoundView
    {
        void LoadCategoryResultList();
        event Action OnLoad;
    }
}

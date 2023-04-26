using System;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ICreateRoundView
    {
        event EventHandler OnLoad;
        void UpdateNewRoundData(RoundWithCategoriesDto roundWithCategoriesDto);
    }
}

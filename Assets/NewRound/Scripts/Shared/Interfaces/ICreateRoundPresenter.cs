using System;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ICreateRoundPresenter
    {
        void CreateRound(object sender, EventArgs e);
        void UpdateRound(RoundWithCategoriesDto roundWithCategoriesDto);
    }
}

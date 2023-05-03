using System;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface IResumeMatchPresenter
    {
        void ResumeMatch(object sender, EventArgs e);
        void UpdateView(RoundWithCategoriesDto roundWithCategoriesDto);
    }
}

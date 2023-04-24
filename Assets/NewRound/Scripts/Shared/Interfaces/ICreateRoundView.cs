using System;
using System.Collections.Generic;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ICreateRoundView
    {
        event EventHandler OnLoad;
        void UpdateNewRoundData(RoundDto roundDto, List<CategoryDTO> categoryDtos);
    }
}

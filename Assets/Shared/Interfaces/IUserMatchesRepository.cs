﻿using System.Collections.Generic;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUserMatchesRepository
    {
        UserMatch Persist(UserMatch userMatch);
        UserMatch Get(int userId, int matchId);
        List<UserMatch> GetAll();
    }
}
﻿using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUserMatchesRepository
    {
        Operation<UserMatch> Save(UserMatch userMatch);
        Operation<UserMatch> Get(int userId, int matchId);
        Operation<List<UserMatch>> GetAll();
    }
}

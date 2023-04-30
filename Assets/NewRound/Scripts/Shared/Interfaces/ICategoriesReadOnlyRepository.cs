using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ICategoriesReadOnlyRepository
    {
        Result<List<Category>> GetRandomCategories(int numberOfCategories);
        Result<bool> Exists(string name);
        Result<bool> Exists(string[] names);
    }
}

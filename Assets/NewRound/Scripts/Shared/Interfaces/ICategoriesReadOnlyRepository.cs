using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ICategoriesReadOnlyRepository
    {
        Result<List<CategoryDto>> GetRandomCategories(int numberOfCategories);
        Result<bool> Exists(string name);
        Result<bool> Exists(string[] names);
    }
}

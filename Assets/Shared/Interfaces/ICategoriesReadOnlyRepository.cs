using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface ICategoriesReadOnlyRepository
    {
        Result<List<Category>> GetRandomCategories(int numberOfCategories);
        Result<List<Category>> GetMany(List<int> categoryIds);
        Result<bool> Exists(string name);
        Result<bool> Exists(string[] names);
    }
}

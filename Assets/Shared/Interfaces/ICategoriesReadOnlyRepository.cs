using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface ICategoriesReadOnlyRepository
    {
        Operation<List<Category>> GetRandomCategories(int numberOfCategories);
        Operation<List<Category>> GetMany(List<int> categoryIds);
        Operation<bool> Exists(string name);
        Operation<bool> Exists(string[] names);
    }
}

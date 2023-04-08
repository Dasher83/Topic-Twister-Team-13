using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces {
    public interface ICategoriesService
    {
        CategoryDTO[] GetRandomCategories(int numberOfCategories);
    }
} 

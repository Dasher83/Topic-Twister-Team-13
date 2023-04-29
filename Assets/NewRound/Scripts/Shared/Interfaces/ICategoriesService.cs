using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces {
    public interface ICategoriesService
    {
        CategoryDto[] GetRandomCategories(int numberOfCategories);
    }
} 

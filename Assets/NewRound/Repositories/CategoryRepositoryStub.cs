using TopicTwister.Backend.Shared.DTOs;
using TopicTwister.Backend.Shared.Interfaces;


namespace TopicTwister.Backend.Repositories
{
    public class CategoryRepositoryStub : ICategoryRepository
    {
        public CategoryDTO[] GetRandomCategories()
        {
            CategoryDTO[] result = new[]{
                new CategoryDTO("1", "Colores"  ),
                new CategoryDTO("2", "Animales" ),
                new CategoryDTO("3", "Paises"   ),
                new CategoryDTO("4", "Plantas"  ),
                new CategoryDTO("5", "Peliculas")
            };
            return result;
        }
    }
}

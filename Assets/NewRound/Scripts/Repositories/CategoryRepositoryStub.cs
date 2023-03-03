using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;


namespace TopicTwister.NewRound.Repositories
{
    public class CategoryRepositoryStub : ICategoryRepository
    {
        public bool Exists(string name)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(string[] names)
        {
            throw new System.NotImplementedException();
        }

        public CategoryDTO[] GetRandomCategories(int numberOfCategories)
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

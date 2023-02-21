namespace TopicTwister.Backend.Shared.DTOs
{
    public class CategoryDTO
    {
        private readonly string _id;
        private readonly string _name;

        public string Id => _id;
        public string Name => _name;

        private CategoryDTO() { }

        public CategoryDTO(string id, string name)
        {
            _id = id;
            _name = name;
        }
    }
}

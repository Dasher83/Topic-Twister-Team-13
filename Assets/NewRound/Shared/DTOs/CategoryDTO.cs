using Mono.Cecil.Cil;

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

        public override bool Equals(object obj)
        {
            CategoryDTO other = obj as CategoryDTO;

            if(this._id != other._id) return false;
            if(this._name != other._name) return false;
            return true;
        }
    }
}

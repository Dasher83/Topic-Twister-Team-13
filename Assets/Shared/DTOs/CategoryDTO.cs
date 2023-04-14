using System;
using UnityEngine;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class CategoryDTO
    {
        [SerializeField]
        private string _id;
        
        [SerializeField]
        private string _name;

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
            if (obj == null || GetType() != obj.GetType())
                return false;

            CategoryDTO other = (CategoryDTO)obj;
            return this._id == other._id &&
                this._name == other._name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _name);
        }
    }
}

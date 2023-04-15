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
    }
}

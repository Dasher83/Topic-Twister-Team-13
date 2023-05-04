using System;
using UnityEngine;


namespace TopicTwister.Shared.DAOs
{
    [Serializable]
    public class CategoryDaoJson
    {
        [SerializeField] private int _id;

        [SerializeField] private string _name;

        public int Id => _id;
        public string Name => _name;

        private CategoryDaoJson() { }

        public CategoryDaoJson(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            CategoryDaoJson other = (CategoryDaoJson)obj;
            return this._id == other._id &&
                this._name == other._name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _name);
        }
    } 
}

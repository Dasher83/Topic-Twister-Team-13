using System;
using UnityEngine;


namespace TopicTwister.NewRound.Shared.DAOs
{
    [Serializable]
    public class CategoryDao
    {
        [SerializeField] private int _id;

        [SerializeField] private string _name;

        public int Id => _id;
        public string Name => _name;

        private CategoryDao() { }

        public CategoryDao(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            CategoryDao other = (CategoryDao)obj;
            return this._id == other._id &&
                this._name == other._name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _name);
        }
    } 
}
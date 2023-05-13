using System;
using UnityEngine;


namespace TopicTwister.Shared.DAOs
{
    [Serializable]
    public class AnswerDaoJson
    {
        [SerializeField] private string _userInput;
        [SerializeField] private int _order;
        [SerializeField] private int _categoryId;
        [SerializeField] private int _userId;
        [SerializeField] private int _roundId;

        public string UserInput => _userInput;
        public int Order => _order;
        public int CategoryId => _categoryId;
        public int UserId => _userId;
        public int RoundId => _roundId;

        private AnswerDaoJson() { }

        public AnswerDaoJson(string userInput, int order, int categoryId, int userId, int roundId)
        {
            _userInput = userInput;
            _order = order;
            _categoryId = categoryId;
            _userId = userId;
            _roundId = roundId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            AnswerDaoJson other = (AnswerDaoJson)obj;

            bool userInputEquals = _userInput == other._userInput;
            bool orderEquals = _order == other._order;
            bool categoryIdEquals = _categoryId == other._categoryId;
            bool userIdEquals = _userId == other._userId;
            bool roundIdEquals = _roundId == other._roundId;

            return userInputEquals && orderEquals && categoryIdEquals && userIdEquals && roundIdEquals;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_userInput, _order, _categoryId, _userId, _roundId);
        }
    }
}

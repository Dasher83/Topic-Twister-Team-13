using System;
using UnityEngine;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class EvaluatedAnswerDto
    {
        [SerializeField] private CategoryDto _category;
        [SerializeField] private string _answer;
        [SerializeField] private bool _isCorrect;
        [SerializeField] private int _order;

        public CategoryDto Category => _category;
        public string Answer => _answer;
        public bool IsCorrect => _isCorrect;
        public int Order => _order;

        public EvaluatedAnswerDto(CategoryDto category, string answer, bool isCorrect, int order)
        {
            _category = category;
            _answer = answer;
            _isCorrect = isCorrect;
            _order = order;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            EvaluatedAnswerDto other = (EvaluatedAnswerDto)obj;
            return this._category == other._category &&
                this._answer == other._answer &&
                this._isCorrect == other._isCorrect &&
                this._order == other._order;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_category, _answer, _isCorrect, _order);
        }
    }
}

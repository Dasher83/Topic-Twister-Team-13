using System;
using UnityEngine;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class EvaluatedAnswerDTO
    {
        [SerializeField] private CategoryDTO _category;
        [SerializeField] private string _answer;
        [SerializeField] private bool _isCorrect;
        [SerializeField] private int _order;

        public CategoryDTO Category => _category;
        public string Answer => _answer;
        public bool IsCorrect => _isCorrect;
        public int Order => _order;

        public EvaluatedAnswerDTO(CategoryDTO category, string answer, bool isCorrect, int order)
        {
            _category = category;
            _answer = answer;
            _isCorrect = isCorrect;
            _order = order;
        }
    }
}

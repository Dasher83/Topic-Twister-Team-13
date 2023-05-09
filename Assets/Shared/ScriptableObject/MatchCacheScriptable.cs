using System.Collections.Generic;
using System;
using TopicTwister.Shared.DTOs;
using UnityEngine;
using System.Linq;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MatchCacheData", menuName = "ScriptableObjects/MatchCacheDate")]
    public class MatchCacheScriptable : ScriptableObject
    {
        public Action<int> UserInputChanged;

        [SerializeField] private MatchDto _matchDto;
        [SerializeField] private RoundWithCategoriesDto _roundWithCategoriesDto;
        [SerializeField] private TurnDto _turnDto;
        [SerializeField] private List<AnswerDraftDto> _turnAnswerDrafts;

        public void Initialize()
        {
            _matchDto = null;
            _roundWithCategoriesDto = null;
            _turnDto = null;
            _turnAnswerDrafts = new List<AnswerDraftDto>();
        }

        public void Initialize(AnswerDraftDto[] turnAnswerDrafts)
        {
            _turnAnswerDrafts = turnAnswerDrafts.ToList();
        }

        public MatchDto MatchDto
        {
            get => _matchDto;
            set => _matchDto = value;
        }

        public RoundWithCategoriesDto RoundWithCategoriesDto
        {
            get => _roundWithCategoriesDto;
            set => _roundWithCategoriesDto = value;
        }

        public List<AnswerDraftDto> TurnAnswerDrafts => _turnAnswerDrafts.ToList();

        public TurnDto TurnDto
        {
            get => _turnDto;
            set => _turnDto = value;
        }

        public void AddUserInput(string userInput, int index)
        {
            _turnAnswerDrafts[index].UserInput += userInput;
            UserInputChanged?.Invoke(index);
        }

        public void RemoveUserInput(int index)
        {
            if (string.IsNullOrEmpty(_turnAnswerDrafts[index].UserInput)) return;
            _turnAnswerDrafts[index].UserInput = _turnAnswerDrafts[index].UserInput.Substring(
                0, _turnAnswerDrafts[index].UserInput.Length - 1);
            UserInputChanged?.Invoke(index);
        }
    }
}

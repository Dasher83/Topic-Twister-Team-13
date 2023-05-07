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
        [SerializeField] private List<TurnAnswerDraftDto> _turnAnswerDrafts;

        public void Initialize()
        {
            _matchDto = null;
            _roundWithCategoriesDto = null;
            _turnAnswerDrafts = new List<TurnAnswerDraftDto>();
        }

        public void Initialize(TurnAnswerDraftDto[] turnAnswerDrafts)
        {
            _turnAnswerDrafts = turnAnswerDrafts.ToList();
        }

        public MatchDto MatchDto
        {
            get {  return _matchDto; }
            set { _matchDto = value; }
        }

        public RoundWithCategoriesDto RoundWithCategoriesDto
        {
            get { return _roundWithCategoriesDto; }
            set { _roundWithCategoriesDto = value; }
        }

        public List<TurnAnswerDraftDto> TurnAnswerDrafts => _turnAnswerDrafts.ToList();

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

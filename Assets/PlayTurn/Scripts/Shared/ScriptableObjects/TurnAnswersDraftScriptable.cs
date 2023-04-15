using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.PlayTurn.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.PlayTurn.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TurnAnswersDraftData", menuName = "ScriptableObjects/TurnAnswersDraftData")]
    public class TurnAnswersDraftScriptable : ScriptableObject
    {
        [SerializeField] private List<TurnAnswerDraftDTO> _turnAnswerDrafts;

        public List<TurnAnswerDraftDTO> TurnAnswerDrafts => _turnAnswerDrafts.ToList();

        public Action<int> UserInputChanged;

        public void Initialize(TurnAnswerDraftDTO[] turnAnswerDrafts)
        {
            _turnAnswerDrafts = turnAnswerDrafts.ToList();
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

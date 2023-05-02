using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TurnAnswersData", menuName = "ScriptableObjects/TurnAnswers")]
    public class TurnAnswersScriptable : ScriptableObject
    {
        [SerializeField] private List<TurnAnswerDto> _turnAnswers = new List<TurnAnswerDto>();

        public void AddAnswers(TurnAnswerDto[] turnAnswers)
        {
            foreach (TurnAnswerDto turnAnswer in turnAnswers)
            {
                _turnAnswers.Add(turnAnswer);
            }
        }

        public void ClearAnswers()
        {
            _turnAnswers.Clear();
        }


        public List<TurnAnswerDto> GetTurnAnswers()
        {
            return new List<TurnAnswerDto>(_turnAnswers);
        } 
    }
}
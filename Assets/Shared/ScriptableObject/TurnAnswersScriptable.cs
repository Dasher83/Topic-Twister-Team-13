using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TurnAnswersData", menuName = "ScriptableObjects/TurnAnswers")]
    public class TurnAnswersScriptable : ScriptableObject
    {
        [SerializeField] private List<AnswerDto> _turnAnswers = new List<AnswerDto>();

        public void AddAnswers(AnswerDto[] turnAnswers)
        {
            foreach (AnswerDto turnAnswer in turnAnswers)
            {
                _turnAnswers.Add(turnAnswer);
            }
        }

        public void ClearAnswers()
        {
            _turnAnswers.Clear();
        }


        public List<AnswerDto> GetTurnAnswers()
        {
            return new List<AnswerDto>(_turnAnswers);
        } 
    }
}

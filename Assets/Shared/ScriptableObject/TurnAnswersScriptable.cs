using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TurnAnswersData", menuName = "ScriptableObjects/TurnAnswers")]
    public class TurnAnswersScriptable : ScriptableObject
    {
        [SerializeField] private List<TurnAnswerDTO> _turnAnswers = new List<TurnAnswerDTO>();

        public void AddAnswers(TurnAnswerDTO[] turnAnswers)
        {
            foreach (TurnAnswerDTO turnAnswer in turnAnswers)
            {
                _turnAnswers.Add(turnAnswer);
            }
        }

        public void ClearAnswers()
        {
            _turnAnswers.Clear();
        }


        public List<TurnAnswerDTO> GetTurnAnswers()
        {
            return new List<TurnAnswerDTO>(_turnAnswers);
        } 
    }
}
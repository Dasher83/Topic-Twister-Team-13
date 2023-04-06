using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RoundAnswersData", menuName = "ScriptableObjects/RoundAnswers")]
    public class RoundAnswersScriptable : ScriptableObject
    {
        [SerializeField] private List<TurnAnswerDTO> _roundAnswers = new List<TurnAnswerDTO>();

        public void AddAnswers(TurnAnswerDTO[] roundAnswers)
        {
            foreach (TurnAnswerDTO roundAnswer in roundAnswers)
            {
                _roundAnswers.Add(roundAnswer);
            }
        }

        public void ClearAnswers()
        {
            _roundAnswers.Clear();
        }


        public List<TurnAnswerDTO> GetRoundAnswers()
        {
            return new List<TurnAnswerDTO>(_roundAnswers);
        } 
    }
}
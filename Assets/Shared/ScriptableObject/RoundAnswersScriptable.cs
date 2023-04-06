using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RoundAnswersData", menuName = "ScriptableObjects/RoundAnswers")]
    public class RoundAnswersScriptable : ScriptableObject
    {
        [SerializeField] private List<RoundAnswerDTO> _roundAnswers = new List<RoundAnswerDTO>();

        public void AddAnswers(RoundAnswerDTO[] roundAnswers)
        {
            foreach (RoundAnswerDTO roundAnswer in roundAnswers)
            {
                _roundAnswers.Add(roundAnswer);
            }
        }

        public void ClearAnswers()
        {
            _roundAnswers.Clear();
        }


        public List<RoundAnswerDTO> GetRoundAnswers()
        {
            return new List<RoundAnswerDTO>(_roundAnswers);
        } 
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects.FakeMatch
{
    [CreateAssetMenu(fileName = "FakeMatchData", menuName = "ScriptableObjects/FakeMatchDate")]
    public class FakeMatchScriptable : ScriptableObject
    {
        [SerializeField]
        private FakeRound[] _rounds = new FakeRound[3];
        private BotTurnAnswersGenerator _botTurnAnswerGenerator;

        public void AddRound(List<EvaluatedAnswerDTO> userAnswers, int roundNumber, string initialLetter)
        {
            _botTurnAnswerGenerator = new BotTurnAnswersGenerator(
                userAnswers: userAnswers,
                initialLetter: initialLetter);
            _rounds[roundNumber - 1] = new FakeRound(
                userAnswers: userAnswers,
                botAnswers: _botTurnAnswerGenerator.GenerateAnswers());
        }

        [Serializable]
        private class FakeRound
        {
            [SerializeField] private List<EvaluatedAnswerDTO> _userAnswers;
            [SerializeField] private List<EvaluatedAnswerDTO> _botAnswers;

            public FakeRound(List<EvaluatedAnswerDTO> userAnswers, List<EvaluatedAnswerDTO> botAnswers)
            {
                _userAnswers = userAnswers;
                _botAnswers = botAnswers;
            }

            public List<EvaluatedAnswerDTO> UserAnswer => _userAnswers.ToList();
            public List<EvaluatedAnswerDTO> BotAnswer => _botAnswers.ToList();
        }

        private class BotTurnAnswersGenerator
        {
            private List<EvaluatedAnswerDTO> _userAnswers;
            private string _initialLetter;


            public BotTurnAnswersGenerator(
                List<EvaluatedAnswerDTO> userAnswers,
                string initialLetter)
            {
                _userAnswers = userAnswers;
                _initialLetter = initialLetter;
            }

            public List<EvaluatedAnswerDTO> GenerateAnswers()
            {
                List<EvaluatedAnswerDTO> generatedAnswers = new List<EvaluatedAnswerDTO>();
                bool willAnswerCorrectly;
                EvaluatedAnswerDTO botAnswer;


                foreach (EvaluatedAnswerDTO evaluatedAnswer in _userAnswers)
                {
                    willAnswerCorrectly = UnityEngine.Random.value > 0.5f;
                    if (willAnswerCorrectly)
                    {
                        botAnswer = new EvaluatedAnswerDTO(
                            category: evaluatedAnswer.Category,
                            answer: $"{_initialLetter} test",
                            isCorrect: true,
                            order: evaluatedAnswer.Order);
                    }
                    else
                    {
                        botAnswer = new EvaluatedAnswerDTO(
                            category: evaluatedAnswer.Category,
                            answer: $"No se",
                            isCorrect: false,
                            order: evaluatedAnswer.Order);
                    }
                    generatedAnswers.Add(botAnswer);
                }
                
                return generatedAnswers;
            }
        } 
    }
}

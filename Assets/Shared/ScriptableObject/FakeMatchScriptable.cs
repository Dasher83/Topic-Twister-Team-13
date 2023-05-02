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
        private List<FakeRound> _rounds = new List<FakeRound>();
        private BotTurnAnswersGenerator _botTurnAnswerGenerator;
        private int _userPoints;
        private int _botPoints;

        public List<FakeRound> Rounds => _rounds.ToList();
        public int UserPoints => _userPoints;
        public int BotPoints => _botPoints;

        public void Initialize()
        {
            _rounds = new List<FakeRound>();
            _userPoints = 0;
            _botPoints = 0;
        }

        public void AddRound(List<EvaluatedAnswerDto> userAnswers, int roundNumber, string initialLetter)
        {
            _botTurnAnswerGenerator = new BotTurnAnswersGenerator(
                userAnswers: userAnswers,
                initialLetter: initialLetter);

            List<EvaluatedAnswerDto> botAnswers = _botTurnAnswerGenerator.GenerateAnswers();

            _rounds.Add(new FakeRound(
                userAnswers: userAnswers,
                botAnswers: botAnswers));

            int _userTurnCorrectAnswers = userAnswers.Count(answer => answer.IsCorrect);
            int _botTurnCorrectAnswers = botAnswers.Count(answer => answer.IsCorrect);

            if (_userTurnCorrectAnswers > _botTurnCorrectAnswers)
            {
                _userPoints++;
            }
            else if (_botTurnCorrectAnswers > _userTurnCorrectAnswers)
            {
                _botPoints++;
            }
            else
            {
                _userPoints++;
                _botPoints++;
            }
        }

        [Serializable]
        public class FakeRound
        {
            [SerializeField] private List<EvaluatedAnswerDto> _userAnswers;
            [SerializeField] private List<EvaluatedAnswerDto> _botAnswers;

            public FakeRound(List<EvaluatedAnswerDto> userAnswers, List<EvaluatedAnswerDto> botAnswers)
            {
                _userAnswers = userAnswers;
                _botAnswers = botAnswers;
            }

            public List<EvaluatedAnswerDto> UserAnswer => _userAnswers.ToList();
            public List<EvaluatedAnswerDto> BotAnswer => _botAnswers.ToList();
        }

        private class BotTurnAnswersGenerator
        {
            private List<EvaluatedAnswerDto> _userAnswers;
            private string _initialLetter;


            public BotTurnAnswersGenerator(
                List<EvaluatedAnswerDto> userAnswers,
                string initialLetter)
            {
                _userAnswers = userAnswers;
                _initialLetter = initialLetter;
            }

            public List<EvaluatedAnswerDto> GenerateAnswers()
            {
                List<EvaluatedAnswerDto> generatedAnswers = new List<EvaluatedAnswerDto>();
                bool willAnswerCorrectly;
                EvaluatedAnswerDto botAnswer;


                foreach (EvaluatedAnswerDto evaluatedAnswer in _userAnswers)
                {
                    willAnswerCorrectly = UnityEngine.Random.value > 0.5f;
                    if (willAnswerCorrectly)
                    {
                        botAnswer = new EvaluatedAnswerDto(
                            category: evaluatedAnswer.Category,
                            answer: $"{_initialLetter} test".ToUpper(),
                            isCorrect: true,
                            order: evaluatedAnswer.Order);
                    }
                    else
                    {
                        botAnswer = new EvaluatedAnswerDto(
                            category: evaluatedAnswer.Category,
                            answer: $"No se".ToUpper(),
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

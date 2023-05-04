using System.Collections.Generic;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.DTOs
{
    public struct AnswersToEvaluateDTO
    {
        public readonly char initialLetter;
        public readonly List<TurnAnswerDto> turnAnswers;

        public AnswersToEvaluateDTO(char initialLetter, List<TurnAnswerDto> turnAnswers)
        {
            this.initialLetter = initialLetter;
            this.turnAnswers = turnAnswers;
        }
    }
}

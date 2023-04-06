using System.Collections.Generic;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.DTOs
{
    public struct AnswersToEvaluateDTO
    {
        public readonly char initialLetter;
        public readonly List<TurnAnswerDTO> turnAnswers;

        public AnswersToEvaluateDTO(char initialLetter, List<TurnAnswerDTO> roundAnswers)
        {
            this.initialLetter = initialLetter;
            this.turnAnswers = roundAnswers;
        }
    }
}

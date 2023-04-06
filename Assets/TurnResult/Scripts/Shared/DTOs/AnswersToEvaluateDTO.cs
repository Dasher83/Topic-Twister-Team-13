using System.Collections.Generic;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.DTOs
{
    public struct AnswersToEvaluateDTO
    {
        public readonly char initialLetter;
        public readonly List<RoundAnswerDTO> roundAnswers;

        public AnswersToEvaluateDTO(char initialLetter, List<RoundAnswerDTO> roundAnswers)
        {
            this.initialLetter = initialLetter;
            this.roundAnswers = roundAnswers;
        }
    }
}

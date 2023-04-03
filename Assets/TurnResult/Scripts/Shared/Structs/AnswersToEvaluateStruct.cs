using System.Collections.Generic;
using TopicTwister.Shared.Structs;


namespace TopicTwister.ResultRound.Shared.Structs
{
    public struct AnswersToEvaluateStruct
    {
        public readonly char initialLetter;
        public readonly List<RoundAnswer> roundAnswers;

        public AnswersToEvaluateStruct(char initialLetter, List<RoundAnswer> roundAnswers)
        {
            this.initialLetter = initialLetter;
            this.roundAnswers = roundAnswers;
        }
    }
}

using System.Collections.Specialized;


namespace TopicTwister.ResultRound.Shared.Structs
{
    public struct AnswersToEvaluateStruct
    {
        public readonly char initialLetter;
        public readonly OrderedDictionary categoryToAnswerMap;

        private AnswersToEvaluateStruct(char initialLetter, OrderedDictionary categoryToAnswerMap)
        {
            this.initialLetter = initialLetter;
            this.categoryToAnswerMap = categoryToAnswerMap;
        }
    }
}
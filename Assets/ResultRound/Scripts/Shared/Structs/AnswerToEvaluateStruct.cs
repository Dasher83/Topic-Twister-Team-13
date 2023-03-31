using System.Collections.Specialized;


namespace TopicTwister.ResultRound.Shared.Structs
{
    public struct AnswerToEvaluateStruct
    {
        public readonly char initialLetter;
        public readonly OrderedDictionary categoryToAnswerMap;

        private AnswerToEvaluateStruct(char initialLetter, OrderedDictionary categoryToAnswerMap)
        {
            this.initialLetter = initialLetter;
            this.categoryToAnswerMap = categoryToAnswerMap;
        }
    }
}
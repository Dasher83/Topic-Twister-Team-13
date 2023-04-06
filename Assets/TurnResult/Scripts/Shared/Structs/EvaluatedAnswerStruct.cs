namespace TopicTwister.TurnResult.Shared.Structs
{
    public struct EvaluatedAnswerStruct
    {
        public readonly string category;
        public readonly string answer;
        public readonly bool isCorrect;
        public readonly int order;

        public EvaluatedAnswerStruct(string category, string answer, bool isCorrect, int order)
        {
            this.category = category;
            this.answer = answer;
            this.isCorrect = isCorrect;
            this.order = order;
        }
    }
}

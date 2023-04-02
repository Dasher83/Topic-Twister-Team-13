namespace TopicTwister.ResultRound.Shared.Structs
{
    public struct EvaluatedAnswerStruct
    {
        public readonly string category;
        public readonly string answer;
        public readonly bool isCorrect;

        public EvaluatedAnswerStruct(string category, string answer, bool isCorrect)
        {
            this.category = category;
            this.answer = answer;
            this.isCorrect = isCorrect;
        }
    }
}
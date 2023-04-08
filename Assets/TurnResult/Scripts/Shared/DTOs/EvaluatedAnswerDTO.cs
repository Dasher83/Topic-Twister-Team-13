using TopicTwister.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.DTOs
{
    public struct EvaluatedAnswerDTO
    {
        public readonly CategoryDTO category;
        public readonly string answer;
        public readonly bool isCorrect;
        public readonly int order;

        public EvaluatedAnswerDTO(CategoryDTO category, string answer, bool isCorrect, int order)
        {
            this.category = category;
            this.answer = answer;
            this.isCorrect = isCorrect;
            this.order = order;
        }
    }
}

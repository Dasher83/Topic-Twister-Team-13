using System.Collections.Generic;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.Shared.Interfaces
{
    public interface IBotAnswerDtosGenerator
    {
        List<AnswerDto> GenerateAnswers(List<AnswerDto> userAnswerDtos, char initialLetter);
    }
}

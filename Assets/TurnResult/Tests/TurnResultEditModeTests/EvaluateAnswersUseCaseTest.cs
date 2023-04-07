using System.Collections.Generic;
using NUnit.Framework;
using TopicTwister.Shared.DTOs;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.DTOs;
using TopicTwister.TurnResult.UseCases;
using TopicTwister.TurnResult.Repositories;


public class EvaluateAnswersUseCaseTest
{
    private IEvaluateAnswersUseCase _useCase;

    [SetUp]
    public void SetUp()
    {
        _useCase = new EvaluateAnswersUseCase(new WordsRepositoryJson(wordsResourceName: "WordsTest"));
    }

    [Test]
    public void Test_evaluate_all_empty_answers()
    {
        #region -- Arrange --
        char initialLetter = 'A';
        List<TurnAnswerDTO> roundAnswers = new List<TurnAnswerDTO>()
        {
            new TurnAnswerDTO(categoryId: "1", userInput: "", order: 0),
            new TurnAnswerDTO(categoryId: "2", userInput: "", order: 1),
            new TurnAnswerDTO(categoryId: "3", userInput: "", order: 2),
            new TurnAnswerDTO(categoryId: "4", userInput: "", order: 3),
            new TurnAnswerDTO(categoryId: "5", userInput: "", order: 4),
        };
        AnswersToEvaluateDTO answersToEvaluateStruct = new AnswersToEvaluateDTO(initialLetter, roundAnswers);
        #endregion

        #region -- Act --
        List<EvaluatedAnswerDTO> actualResult = _useCase.EvaluateAnswers(answersToEvaluateStruct);
        #endregion

        #region -- Assert --
        List<EvaluatedAnswerDTO> expectedResult = new List<EvaluatedAnswerDTO>();
        foreach (TurnAnswerDTO answer in roundAnswers)
        {
            expectedResult.Add(new EvaluatedAnswerDTO(
                answer.CategoryId,
                answer.UserInput,
                isCorrect: false,
                order: answer.Order));
        }

        Assert.AreEqual(expectedResult, actualResult);
        #endregion
    }

    [Test]
    public void Test_evaluate_all_incorrect_answers()
    {
        #region -- Arrange --
        char initialLetter = 'A';
        List<TurnAnswerDTO> roundAnswers = new List<TurnAnswerDTO>()
        {
            new TurnAnswerDTO(categoryId: "1", userInput: "notTest", order: 0),
            new TurnAnswerDTO(categoryId: "2", userInput: "notTest", order: 1),
            new TurnAnswerDTO(categoryId: "3", userInput: "notTest", order: 2),
            new TurnAnswerDTO(categoryId: "4", userInput: "notTest", order: 3),
            new TurnAnswerDTO(categoryId: "5", userInput: "notTest", order: 4),
        };
        AnswersToEvaluateDTO answersToEvaluateStruct = new AnswersToEvaluateDTO(initialLetter, roundAnswers);
        #endregion

        #region -- Act --
        List<EvaluatedAnswerDTO> actualResult = _useCase.EvaluateAnswers(answersToEvaluateStruct);
        #endregion

        #region -- Assert --
        List<EvaluatedAnswerDTO> expectedResult = new List<EvaluatedAnswerDTO>();
        foreach (TurnAnswerDTO answer in roundAnswers)
        {
            expectedResult.Add(new EvaluatedAnswerDTO(
                answer.CategoryId,
                answer.UserInput,
                isCorrect: false,
                order: answer.Order));
        }

        Assert.AreEqual(expectedResult, actualResult);
        #endregion
    }
}

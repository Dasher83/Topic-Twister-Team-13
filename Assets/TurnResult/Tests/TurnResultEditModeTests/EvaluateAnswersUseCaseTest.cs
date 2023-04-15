using System.Collections.Generic;
using NUnit.Framework;
using TopicTwister.Shared.DTOs;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.DTOs;
using TopicTwister.TurnResult.UseCases;
using TopicTwister.TurnResult.Repositories;
using System.Collections;
using System.Linq;
using System;

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
        List<TurnAnswerDTO> turnAnswers = new List<TurnAnswerDTO>()
        {
            new TurnAnswerDTO(category: new CategoryDTO("1", ""), userInput: "", order: 0),
            new TurnAnswerDTO(category: new CategoryDTO("2", ""), userInput: "", order: 1),
            new TurnAnswerDTO(category: new CategoryDTO("3", ""), userInput: "", order: 2),
            new TurnAnswerDTO(category: new CategoryDTO("4", ""), userInput: "", order: 3),
            new TurnAnswerDTO(category: new CategoryDTO("5", ""), userInput: "", order: 4),
        };
        AnswersToEvaluateDTO answersToEvaluateStruct = new AnswersToEvaluateDTO(initialLetter, turnAnswers);
        #endregion

        #region -- Act --
        List<EvaluatedAnswerDTO> actualResult = _useCase.EvaluateAnswers(answersToEvaluateStruct);
        #endregion

        #region -- Assert --
        List<EvaluatedAnswerDTO> expectedResult = new List<EvaluatedAnswerDTO>();
        foreach (TurnAnswerDTO answer in turnAnswers)
        {
            expectedResult.Add(new EvaluatedAnswerDTO(
                answer.Category,
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
        List<TurnAnswerDTO> turnAnswers = new List<TurnAnswerDTO>()
        {
            new TurnAnswerDTO(category: new CategoryDTO("1", ""), userInput: "notTest", order: 0),
            new TurnAnswerDTO(category: new CategoryDTO("2", ""), userInput: "notTest", order: 1),
            new TurnAnswerDTO(category: new CategoryDTO("3", ""), userInput: "notTest", order: 2),
            new TurnAnswerDTO(category: new CategoryDTO("4", ""), userInput: "notTest", order: 3),
            new TurnAnswerDTO(category: new CategoryDTO("5", ""), userInput: "notTest", order: 4),
        };
        AnswersToEvaluateDTO answersToEvaluateStruct = new AnswersToEvaluateDTO(initialLetter, turnAnswers);
        #endregion

        #region -- Act --
        List<EvaluatedAnswerDTO> actualResult = _useCase.EvaluateAnswers(answersToEvaluateStruct);
        #endregion

        #region -- Assert --
        List<EvaluatedAnswerDTO> expectedResult = new List<EvaluatedAnswerDTO>();
        foreach (TurnAnswerDTO answer in turnAnswers)
        {
            expectedResult.Add(new EvaluatedAnswerDTO(
                answer.Category,
                answer.UserInput,
                isCorrect: false,
                order: answer.Order));
        }

        Assert.AreEqual(expectedResult, actualResult);
        #endregion
    }

    [Test]
    public void Test_evaluate_all_correct_answers()
    {
        #region -- Arrange --
        string alphabet = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
        List<string> categoryIds = new List<string>()
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14"
        };

        foreach (string categoryId in categoryIds)
        {
            foreach (char letter in alphabet)
            {
                List<TurnAnswerDTO> turnAnswers = new List<TurnAnswerDTO>()
                {
                    new TurnAnswerDTO(
                        category: new CategoryDTO(categoryId, ""),
                        userInput: $"{letter} test", order: 0),
                    new TurnAnswerDTO(
                        category: new CategoryDTO(categoryId, ""),
                        userInput: $"{letter} test", order: 1),
                    new TurnAnswerDTO(
                        category: new CategoryDTO(categoryId, ""),
                        userInput: $"{letter} test", order: 2),
                    new TurnAnswerDTO(
                        category: new CategoryDTO(categoryId, ""),
                        userInput: $"{letter} test", order: 3),
                    new TurnAnswerDTO(
                        category: new CategoryDTO(categoryId, ""),
                        userInput: $"{letter} test", order: 4),
                };
                AnswersToEvaluateDTO answersToEvaluateStruct = new AnswersToEvaluateDTO(initialLetter: letter, turnAnswers);
        #endregion

                #region -- Act --
                List<EvaluatedAnswerDTO> actualResult = _useCase.EvaluateAnswers(answersToEvaluateStruct);
                #endregion

                #region -- Assert --
                List<EvaluatedAnswerDTO> expectedResult = new List<EvaluatedAnswerDTO>();
                foreach (TurnAnswerDTO answer in turnAnswers)
                {
                    expectedResult.Add(new EvaluatedAnswerDTO(
                        answer.Category,
                        answer.UserInput,
                        isCorrect: true,
                        order: answer.Order));
                }

                Assert.AreEqual(expectedResult, actualResult);
                #endregion
            }
        }
    }

    [Test]
    public void Test_evaluate_mixed_answers()
    {
        #region -- Arrange --
        string alphabet = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
        Random random = new Random();
        List<string> categoryIds = new List<string>()
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14"
        };
        List<string> randomCategoryIds = new List<string>(categoryIds.OrderBy(x => random.Next()).Take(5));

        foreach (char letter in alphabet)
        {
            List<TurnAnswerDTO> turnAnswers = new List<TurnAnswerDTO>()
            {
                new TurnAnswerDTO(
                    category: new CategoryDTO(randomCategoryIds[0], ""),
                    userInput: $"{letter} test", order: 0),
                new TurnAnswerDTO(
                    category: new CategoryDTO(randomCategoryIds[1], ""),
                    userInput: "", order: 1),
                new TurnAnswerDTO(
                    category: new CategoryDTO(randomCategoryIds[2], ""),
                    userInput: "notTest", order: 2),
                new TurnAnswerDTO(
                    category: new CategoryDTO(randomCategoryIds[3], ""),
                    userInput: "", order: 3),
                new TurnAnswerDTO(
                    category: new CategoryDTO(randomCategoryIds[4], ""),
                    userInput: $"{letter} test", order: 4),
            };
            AnswersToEvaluateDTO answersToEvaluateStruct = new AnswersToEvaluateDTO(initialLetter: letter, turnAnswers);
            #endregion

            #region -- Act --
            List<EvaluatedAnswerDTO> actualResult = _useCase.EvaluateAnswers(answersToEvaluateStruct);
            #endregion

            #region -- Assert --
            List<EvaluatedAnswerDTO> expectedResult = new List<EvaluatedAnswerDTO>()
            {
                new EvaluatedAnswerDTO(
                    category: turnAnswers[0].Category,
                    answer: turnAnswers[0].UserInput,
                    isCorrect: true,
                    order: turnAnswers[0].Order),
                new EvaluatedAnswerDTO(
                    category: turnAnswers[1].Category,
                    answer: turnAnswers[1].UserInput,
                    isCorrect: false,
                    order: turnAnswers[1].Order),
                new EvaluatedAnswerDTO(
                    category: turnAnswers[2].Category,
                    answer: turnAnswers[2].UserInput,
                    isCorrect: false,
                    order: turnAnswers[2].Order),
                new EvaluatedAnswerDTO(
                    category: turnAnswers[3].Category,
                    answer: turnAnswers[3].UserInput,
                    isCorrect: false,
                    order: turnAnswers[3].Order),
                new EvaluatedAnswerDTO(
                    category: turnAnswers[4].Category,
                    answer: turnAnswers[4].UserInput,
                    isCorrect: true,
                    order: turnAnswers[4].Order)
            };

            Assert.AreEqual(expectedResult, actualResult);
            #endregion
        }
    }
}

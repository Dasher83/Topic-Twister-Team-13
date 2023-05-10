using System.Collections.Generic;
using NUnit.Framework;
using TopicTwister.Shared.DTOs;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.DTOs;
using TopicTwister.TurnResult.UseCases;
using TopicTwister.TurnResult.Repositories;
using System.Linq;
using System;
using TopicTwister.Shared.Utils;


public class EvaluateAnswersUseCaseTest
{
    private IEvaluateAnswersUseCase _useCase;

    [SetUp]
    public void SetUp()
    {
        _useCase = new EvaluateAnswersUseCase(new WordsRepositoryJson(wordsResourceName: "JSON/TestData/WordsTest"));
    }

    [Test]
    public void Test_evaluate_all_empty_answers()
    {
        #region -- Arrange --
        char initialLetter = 'A';
        List<AnswerDto> turnAnswers = new List<AnswerDto>()
        {
            new AnswerDto(categoryDto: new CategoryDto(id: 1, ""), userInput: "", order: 0),
            new AnswerDto(categoryDto: new CategoryDto(id: 2, ""), userInput: "", order: 1),
            new AnswerDto(categoryDto: new CategoryDto(id: 3, ""), userInput: "", order: 2),
            new AnswerDto(categoryDto: new CategoryDto(id: 4, ""), userInput: "", order: 3),
            new AnswerDto(categoryDto: new CategoryDto(id: 5, ""), userInput: "", order: 4),
        };
        AnswersToEvaluateDTO answersToEvaluateStruct = new AnswersToEvaluateDTO(initialLetter, turnAnswers);
        #endregion

        #region -- Act --
        Operation<List<EvaluatedAnswerDto>> actualOperation = _useCase.EvaluateAnswers(answersToEvaluateStruct);
        #endregion

        #region -- Assert --
        List<EvaluatedAnswerDto> expectedOperation = new List<EvaluatedAnswerDto>();
        foreach (AnswerDto answer in turnAnswers)
        {
            expectedOperation.Add(new EvaluatedAnswerDto(
                answer.Category,
                answer.UserInput,
                isCorrect: false,
                order: answer.Order));
        }

        Assert.AreEqual(expectedOperation, actualOperation.Result);
        #endregion
    }

    [Test]
    public void Test_evaluate_all_incorrect_answers()
    {
        #region -- Arrange --
        char initialLetter = 'A';
        List<AnswerDto> turnAnswers = new List<AnswerDto>()
        {
            new AnswerDto(categoryDto: new CategoryDto(id: 1, ""), userInput: "notTest", order: 0),
            new AnswerDto(categoryDto: new CategoryDto(id: 2, ""), userInput: "notTest", order: 1),
            new AnswerDto(categoryDto: new CategoryDto(id: 3, ""), userInput: "notTest", order: 2),
            new AnswerDto(categoryDto: new CategoryDto(id: 4, ""), userInput: "notTest", order: 3),
            new AnswerDto(categoryDto: new CategoryDto(id: 5, ""), userInput: "notTest", order: 4),
        };
        AnswersToEvaluateDTO answersToEvaluateStruct = new AnswersToEvaluateDTO(initialLetter, turnAnswers);
        #endregion

        #region -- Act --
        Operation<List<EvaluatedAnswerDto>> actualOperation = _useCase.EvaluateAnswers(answersToEvaluateStruct);
        #endregion

        #region -- Assert --
        List<EvaluatedAnswerDto> expectedOperation = new List<EvaluatedAnswerDto>();
        foreach (AnswerDto answer in turnAnswers)
        {
            expectedOperation.Add(new EvaluatedAnswerDto(
                answer.Category,
                answer.UserInput,
                isCorrect: false,
                order: answer.Order));
        }

        Assert.AreEqual(expectedOperation, actualOperation.Result);
        #endregion
    }

    [Test]
    public void Test_evaluate_all_correct_answers()
    {
        #region -- Arrange --
        string alphabet = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
        List<int> categoryIds = new List<int>()
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14
        };

        foreach (int categoryId in categoryIds)
        {
            foreach (char letter in alphabet)
            {
                List<AnswerDto> turnAnswers = new List<AnswerDto>()
                {
                    new AnswerDto(
                        categoryDto: new CategoryDto(categoryId, ""),
                        userInput: $"{letter} test", order: 0),
                    new AnswerDto(
                        categoryDto: new CategoryDto(categoryId, ""),
                        userInput: $"{letter} test", order: 1),
                    new AnswerDto(
                        categoryDto: new CategoryDto(categoryId, ""),
                        userInput: $"{letter} test", order: 2),
                    new AnswerDto(
                        categoryDto: new CategoryDto(categoryId, ""),
                        userInput: $"{letter} test", order: 3),
                    new AnswerDto(
                        categoryDto: new CategoryDto(categoryId, ""),
                        userInput: $"{letter} test", order: 4),
                };
                AnswersToEvaluateDTO answersToEvaluateStruct = new AnswersToEvaluateDTO(initialLetter: letter, turnAnswers);
                #endregion

                #region -- Act --
                Operation<List<EvaluatedAnswerDto>> actualOperation = _useCase.EvaluateAnswers(answersToEvaluateStruct);
                #endregion

                #region -- Assert --
                List<EvaluatedAnswerDto> expectedOperation = new List<EvaluatedAnswerDto>();
                foreach (AnswerDto answer in turnAnswers)
                {
                    expectedOperation.Add(new EvaluatedAnswerDto(
                        answer.Category,
                        answer.UserInput,
                        isCorrect: true,
                        order: answer.Order));
                }

                Assert.AreEqual(expectedOperation, actualOperation.Result);
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
        List<int> categoryIds = new List<int>()
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14
        };
        List<int> randomCategoryIds = new List<int>(categoryIds.OrderBy(catgoryId => random.Next()).Take(5));

        foreach (char letter in alphabet)
        {
            List<AnswerDto> turnAnswers = new List<AnswerDto>()
            {
                new AnswerDto(
                    categoryDto: new CategoryDto(randomCategoryIds[0], ""),
                    userInput: $"{letter} test", order: 0),
                new AnswerDto(
                    categoryDto: new CategoryDto(randomCategoryIds[1], ""),
                    userInput: "", order: 1),
                new AnswerDto(
                    categoryDto: new CategoryDto(randomCategoryIds[2], ""),
                    userInput: "notTest", order: 2),
                new AnswerDto(
                    categoryDto: new CategoryDto(randomCategoryIds[3], ""),
                    userInput: "", order: 3),
                new AnswerDto(
                    categoryDto: new CategoryDto(randomCategoryIds[4], ""),
                    userInput: $"{letter} test", order: 4),
            };
            AnswersToEvaluateDTO answersToEvaluateStruct = new AnswersToEvaluateDTO(initialLetter: letter, turnAnswers);
            #endregion

            #region -- Act --
            Operation<List<EvaluatedAnswerDto>> actualOperation = _useCase.EvaluateAnswers(answersToEvaluateStruct);
            #endregion

            #region -- Assert --
            List<EvaluatedAnswerDto> expectedOperation = new List<EvaluatedAnswerDto>()
            {
                new EvaluatedAnswerDto(
                    category: turnAnswers[0].Category,
                    answer: turnAnswers[0].UserInput,
                    isCorrect: true,
                    order: turnAnswers[0].Order),
                new EvaluatedAnswerDto(
                    category: turnAnswers[1].Category,
                    answer: turnAnswers[1].UserInput,
                    isCorrect: false,
                    order: turnAnswers[1].Order),
                new EvaluatedAnswerDto(
                    category: turnAnswers[2].Category,
                    answer: turnAnswers[2].UserInput,
                    isCorrect: false,
                    order: turnAnswers[2].Order),
                new EvaluatedAnswerDto(
                    category: turnAnswers[3].Category,
                    answer: turnAnswers[3].UserInput,
                    isCorrect: false,
                    order: turnAnswers[3].Order),
                new EvaluatedAnswerDto(
                    category: turnAnswers[4].Category,
                    answer: turnAnswers[4].UserInput,
                    isCorrect: true,
                    order: turnAnswers[4].Order)
            };

            Assert.AreEqual(expectedOperation, actualOperation.Result);
            #endregion
        }
    }
}

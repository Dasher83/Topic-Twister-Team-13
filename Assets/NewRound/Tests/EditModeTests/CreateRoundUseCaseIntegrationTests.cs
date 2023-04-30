using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.UseCases;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace NewRoundTests
{
    public class CreateRoundUseCaseIntegrationTests
    {
        private ICreateRoundUseCase _useCase;
        private IRoundsRepository _roundRepository;
        private IMatchesRepository _matchesRepository;
        private ICategoriesReadOnlyRepository _categoriesReadOnlyRepository;
        private ILetterRepository _letterRepository;
        private IdtoMapper<Round, RoundWithCategoriesDto> _roundWithCategoriesDtoMapper;
        private const int MaxRounds = 3;
        private const int MaxCategories = 5;

        [Test]
        public void Test_ok_creation_of_all_rounds()
        {
            #region -- Arrange --
            DateTime startDateTime = DateTime.UtcNow;
            MatchDTO matchDto = new MatchDTO(id: 0, startDateTime: startDateTime);

            _useCase = new CreateRoundUseCase(
                roundsRepository: _roundRepository,
                matchesRepository: _matchesRepository,
                categoryRepository: _categoriesReadOnlyRepository,
                letterRepository: _letterRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
            #endregion

            #region -- Act --
            List<Result<RoundWithCategoriesDto>> actualResults = new List<Result<RoundWithCategoriesDto>>();
            for (int i = 0; i < MaxRounds; i++)
            {
                actualResults.Add(_useCase.Create(matchDto: matchDto));
            }
            #endregion

            #region -- Assert --
            List<RoundWithCategoriesDto> expectedDtos = new List<RoundWithCategoriesDto>();

            for (int i = 0; i < MaxRounds; i++)
            {
                RoundWithCategoriesDto expectedDto =
                    new RoundWithCategoriesDto(
                        roundDto: new RoundDto(
                            id: i,
                            roundNumber: i,
                            initialLetter: actualResults[i].Outcome.RoundDto.InitialLetter,
                            isActive: true),
                        categoryDtos: actualResults[i].Outcome.CategoryDtos);

                expectedDtos.Add(expectedDto);
            }

            for (int i = 0; i < MaxRounds; i++)
            {
                if (i < MaxRounds - 1)
                {
                    Assert.AreNotEqual(actualResults[i].Outcome, actualResults[i + 1].Outcome);
                }
                Assert.IsTrue(actualResults[i].WasOk);
                Assert.AreEqual(expected: expectedDtos[i], actual: actualResults[i].Outcome);
                Assert.AreEqual(expected: MaxCategories, actual: actualResults[i].Outcome.CategoryDtos.Count);
                Assert.IsFalse(actualResults[i].Outcome.CategoryDtos.Any(category => category == null));
                List<CategoryDto> duplicates = actualResults[i].Outcome.CategoryDtos
                    .GroupBy(category => category.Id)
                    .Where(category => category.Count() > 1)
                    .SelectMany(category => category)
                    .ToList();

                Assert.IsEmpty(duplicates);
            }
            #endregion
        }

        [Test]
        public void Test_fail_due_to_unknown_match()
        {
            #region -- Arrange --
            DateTime startDateTime = DateTime.UtcNow;
            MatchDTO matchDto = new MatchDTO(id: -1, startDateTime: startDateTime);

            _useCase = new CreateRoundUseCase(
                roundsRepository: _roundRepository,
                matchesRepository: _matchesRepository,
                categoryRepository: _categoriesReadOnlyRepository,
                letterRepository: _letterRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
            #endregion

            #region -- Act --
            Result<RoundWithCategoriesDto> useCaseOperationResult = _useCase.Create(matchDto: matchDto);
            #endregion

            #region -- Assert --
            Assert.IsFalse(useCaseOperationResult.WasOk);
            Assert.AreEqual(expected: $"Match not found with id: {matchDto.Id}", actual: useCaseOperationResult.ErrorMessage);
            #endregion
        }

        [Test]
        public void Test_fail_due_to_match_already_ended()
        {
            #region -- Arrange --
            DateTime startDateTime = DateTime.UtcNow - TimeSpan.FromSeconds(10);
            DateTime endDateTime = DateTime.UtcNow;

            MatchDTO matchDto = new MatchDTO(
                id: 0,
                startDateTime: startDateTime,
                endDateTime: endDateTime);

            _useCase = new CreateRoundUseCase(
                roundsRepository: _roundRepository,
                matchesRepository: _matchesRepository,
                categoryRepository: _categoriesReadOnlyRepository,
                letterRepository: _letterRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
            #endregion

            #region -- Act --
            Result<RoundWithCategoriesDto> useCaseOperationResult = _useCase.Create(matchDto: matchDto);
            #endregion

            #region -- Assert --
            Assert.IsFalse(useCaseOperationResult.WasOk);
            Assert.AreEqual(
                expected: $"Cannot create new round for inactive match with id: {matchDto.Id}",
                actual: useCaseOperationResult.ErrorMessage);
            #endregion
        }

        [Test]
        public void Test_fail_due_to_match_started_after_it_ended()
        {
            #region -- Arrange --
            DateTime startDateTime = DateTime.UtcNow;
            DateTime endDateTime = DateTime.UtcNow - TimeSpan.FromSeconds(10);

            MatchDTO matchDto = new MatchDTO(
                id: 0,
                startDateTime: startDateTime,
                endDateTime: endDateTime);

            _useCase = new CreateRoundUseCase(
                roundsRepository: _roundRepository,
                matchesRepository: _matchesRepository,
                categoryRepository: _categoriesReadOnlyRepository,
                letterRepository: _letterRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
            #endregion

            #region -- Act --
            Result<RoundWithCategoriesDto> useCaseOperationResult = _useCase.Create(matchDto: matchDto);
            #endregion

            #region -- Assert --
            Assert.IsFalse(useCaseOperationResult.WasOk);
            Assert.AreEqual(
                expected: $"Cannot create new round for invalid match with id: {matchDto.Id}",
                actual: useCaseOperationResult.ErrorMessage);
            #endregion
        }

        [Test]
        public void Test_fail_creation_of_more_than_three_rounds()
        {
            #region -- Arrange --
            DateTime startDateTime = DateTime.UtcNow;
            MatchDTO matchDto = new MatchDTO(id: 0, startDateTime: startDateTime);

            _useCase = new CreateRoundUseCase(
                roundsRepository: _roundRepository,
                matchesRepository: _matchesRepository,
                categoryRepository: _categoriesReadOnlyRepository,
                letterRepository: _letterRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
            #endregion

            #region -- Act --
            List<Result<RoundWithCategoriesDto>> actualResults = new List<Result<RoundWithCategoriesDto>>();
            for (int i = 0; i < MaxRounds + 1; i++)
            {
                actualResults.Add(_useCase.Create(matchDto: matchDto));
            }
            #endregion

            #region -- Assert --
            for (int i = 0; i < MaxRounds; i++)
            {
                Assert.IsTrue(actualResults[i].WasOk);
            }
            Assert.IsFalse(actualResults[MaxRounds].WasOk);
            Assert.AreEqual(
                expected: $"All rounds are already created for match with id: {matchDto.Id}",
                actual: actualResults[MaxRounds].ErrorMessage);
            #endregion
        }
    }
}

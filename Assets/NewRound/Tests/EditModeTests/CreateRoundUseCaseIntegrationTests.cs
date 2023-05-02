using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using TopicTwister.NewRound.Repositories;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Shared.Mappers;
using TopicTwister.NewRound.UseCases;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Repositories.IdGenerators;
using TopicTwister.Shared.TestUtils;
using TopicTwister.Shared.Utils;


namespace NewRoundTests
{
    public class CreateRoundUseCaseIntegrationTests
    {
        private ICreateRoundUseCase _useCase;
        private IRoundsRepository _roundsRepository;
        private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
        private IMatchesRepository _matchesRepository;
        private ICategoriesReadOnlyRepository _categoriesReadOnlyRepository;
        private ILetterReadOnlyRepository _letterRepository;
        private IdtoMapper<Round, RoundWithCategoriesDto> _roundWithCategoriesDtoMapper;
        private IdtoMapper<Match, MatchDto> _matchDtoMapper;
        private IUniqueIdGenerator _idGenerator;
        private IdaoMapper<Match, MatchDaoJson> _matchDaoMapper;
        private const int MaxRounds = 3;
        private const int MaxCategories = 5;

        [SetUp]
        public void SetUp()
        {
            _matchDaoMapper = new MatchDaoJsonMapper();
            _idGenerator = Substitute.For<IUniqueIdGenerator>();

            _matchDtoMapper = new MatchDtoMapper();
            _roundWithCategoriesDtoMapper = new RoundWithCategoriesDtoMapper(
                categoryDtoMapper: new CategoryDtoMapper(),
                roundDtoMapper: new RoundDtoMapper());
            _letterRepository = new LetterReadOnlyRepositoryInMemory();

            _categoriesReadOnlyRepository = new CategoriesReadOnlyRepositoryJson(
                categoriesResourceName: "TestData/Category",
                mapper: new CategoryDaoJsonMapper());

            _matchesReadOnlyRepository = new MatchesReadOnlyRepositoryJson(
                resourceName: "TestData/Matches",
                matchDaoMapper: _matchDaoMapper);

            _matchesRepository = new MatchesRepositoryJson(
                matchesResourceName: "TestData/Matches",
                idGenerator: new MatchesIdGenerator(_matchesReadOnlyRepository),
                matchDaoMapper: _matchDaoMapper);

            _roundsRepository = new RoundsRespositoryJson(
                resourceName: "TestData/Rounds",
                idGenerator: _idGenerator,
                matchesRepository: _matchesReadOnlyRepository,
                categoriesRepository: _categoriesReadOnlyRepository);

            _useCase = new CreateRoundUseCase(
                roundsRepository: _roundsRepository,
                matchesRepository: _matchesReadOnlyRepository,
                categoryRepository: _categoriesReadOnlyRepository,
                letterRepository: _letterRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
        }

        [TearDown]
        public void TearDown()
        {
            new MatchesDeleteJson().Delete();
            new UserMatchesDeleteJson().Delete();
            new RoundsDeleteJson().Delete();
        }

        [Test]
        public void Test_ok_creation_of_all_rounds()
        {
            #region -- Arrange --
            DateTime startDateTime = DateTime.UtcNow;
            Match match = new Match(startDateTime: startDateTime);
            match = _matchesRepository.Save(match).Outcome;
            MatchDto matchDto = _matchDtoMapper.ToDTO(match);

            int idsIndex = 0;
            int[] ids = new int[3] { 3, 8, 12 };
            _idGenerator.GetNextId().Returns(
                (args) =>
                {
                    int result = ids[idsIndex];
                    idsIndex++;
                    return result;
                });
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
                            id: ids[i],
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
            MatchDto matchDto = new MatchDto(id: -1, startDateTime: startDateTime);
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

            Match match = new Match(
                id: 0,
                startDateTime: startDateTime,
                endDateTime: endDateTime);

            match = _matchesRepository.Save(match).Outcome;
            MatchDto matchDto = _matchDtoMapper.ToDTO(match);
            #endregion

            #region -- Act --
            Result<RoundWithCategoriesDto> useCaseOperationResult = _useCase.Create(matchDto);
            #endregion

            #region -- Assert --
            Assert.IsFalse(useCaseOperationResult.WasOk);
            Assert.AreEqual(
                expected: $"Cannot create new round for inactive match with id: {match.Id}",
                actual: useCaseOperationResult.ErrorMessage);
            #endregion
        }

        [Test]
        public void Test_fail_due_to_match_started_after_it_ended()
        {
            #region -- Arrange --
            DateTime startDateTime = DateTime.UtcNow;
            DateTime endDateTime = DateTime.UtcNow - TimeSpan.FromSeconds(10);

            Match match = new Match(
                startDateTime: startDateTime,
                endDateTime: endDateTime);

            match = _matchesRepository.Save(match).Outcome;
            MatchDto matchDto = _matchDtoMapper.ToDTO(match);
            #endregion

            #region -- Act --
            Result<RoundWithCategoriesDto> useCaseOperationResult = _useCase.Create(matchDto);
            #endregion

            #region -- Assert --
            Assert.IsFalse(useCaseOperationResult.WasOk);
            Assert.AreEqual(
                expected: $"Cannot create new round for invalid match with id: {match.Id}",
                actual: useCaseOperationResult.ErrorMessage);
            #endregion
        }

        [Test]
        public void Test_fail_creation_of_more_than_three_rounds()
        {
            #region -- Arrange --
            DateTime startDateTime = DateTime.UtcNow;
            MatchDto matchDto = new MatchDto(id: 0, startDateTime: startDateTime);
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

using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.UseCases;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace NewRoundTests
{
    public class CreateRoundSubUseCaseUnitTests
    {
        private ICreateRoundSubUseCase _useCase;
        private IRoundsRepository _roundRepository;
        private IMatchesRepository _matchesRepository;
        private ICategoriesReadOnlyRepository _categoriesReadOnlyRepository;
        private ILetterReadOnlyRepository _letterRepository;
        private IdtoMapper<Round, RoundWithCategoriesDto> _roundWithCategoriesDtoMapper;
        private const int MaxRounds = 3;
        private const int MaxCategories = 5;

        [Test]
        public void Test_ok_creation_of_all_rounds()
        {
            #region -- Arrange --
            DateTime startDateTime = DateTime.UtcNow;
            MatchDto matchDto = new MatchDto(id: 0, startDateTime: startDateTime);
            Match match = new Match(matchDto.Id, matchDto.StartDateTime);
            List<Operation<RoundWithCategoriesDto>> actualResults = new List<Operation<RoundWithCategoriesDto>>();

            _roundRepository = Substitute.For<IRoundsRepository>();
            int nextFakeId = 0;
            _roundRepository.Save(Arg.Any<Round>()).Returns(
                (args) =>
                {
                    Round round = (Round)args[0];
                    Operation<Round> saveRoundOperation = Operation<Round>
                    .Success(
                        outcome: new Round(
                            id: nextFakeId,
                            roundNumber: round.RoundNumber,
                            initialLetter: round.InitialLetter,
                            isActive: round.IsActive,
                            match: match,
                            categories: round.Categories));

                    nextFakeId++;
                    return saveRoundOperation;
                });

            _roundRepository.GetMany(Arg.Any<int>()).Returns(
                (args) =>
                {
                    List<RoundWithCategoriesDto> roundWithCategoriesDtos = new List<RoundWithCategoriesDto>();

                    foreach (
                        RoundWithCategoriesDto roundWithCategoriesDto
                        in actualResults.Select(actualResult => actualResult.Outcome))
                    {
                        RoundDto roundDto = new RoundDto(
                            id: roundWithCategoriesDto.RoundDto.Id,
                            roundNumber: roundWithCategoriesDto.RoundDto.RoundNumber,
                            initialLetter: roundWithCategoriesDto.RoundDto.InitialLetter,
                            isActive: false,
                            matchId: match.Id);

                        RoundWithCategoriesDto newRoundWithCategoriesDto = new RoundWithCategoriesDto(
                            roundDto: roundDto,
                            categoryDtos: roundWithCategoriesDto.CategoryDtos);

                        roundWithCategoriesDtos.Add(newRoundWithCategoriesDto);
                    }

                    List<Round> rounds = roundWithCategoriesDtos
                    .Select(roundWithCategoriesDto => _roundWithCategoriesDtoMapper.FromDTO(roundWithCategoriesDto))
                    .ToList();

                    return Operation<List<Round>>.Success(outcome: rounds);
                });

            _matchesRepository = Substitute.For<IMatchesRepository>();
            _matchesRepository.Get(Arg.Any<int>()).Returns(
                (args) =>
                {
                    int id = (int)args[0];
                    if (id < 0)
                    {
                        return Operation<Match>.Failure(errorMessage: $"Match not found with id: {id}");
                    }
                    List<Round> rounds = new List<Round>();
                    foreach (Operation<RoundWithCategoriesDto> result in actualResults)
                    {
                        rounds.Add(_roundWithCategoriesDtoMapper.FromDTO(result.Outcome));
                    }
                    Match lambdaMatch = new Match(
                        id,
                        startDateTime,
                        endDateTime: null,
                        rounds: rounds);
                    return Operation<Match>.Success(outcome: lambdaMatch);
                });

            List<Category> categories = new List<Category>()
            {
                new Category(id: 1, name: "Category_1"),
                new Category(id: 2, name: "Category_2"),
                new Category(id: 3, name: "Category_3"),
                new Category(id: 4, name: "Category_4"),
                new Category(id: 5, name: "Category_5"),
                new Category(id: 6, name: "Category_6"),
                new Category(id: 7, name: "Category_7"),
                new Category(id: 8, name: "Category_8"),
                new Category(id: 9, name: "Category_9"),
                new Category(id: 10, name: "Category_10"),
            };
            List<Category>[] randomCategories = new List<Category>[MaxRounds];
            int randomCategoriesListIndex = 0;

            Random random = new Random();
            for(int i = 0; i < randomCategories.Length; i++)
            {
                randomCategories[i] = categories
                    .OrderBy(category => random.Next())
                    .Take(MaxCategories)
                    .ToList();
            }

            _categoriesReadOnlyRepository = Substitute.For<ICategoriesReadOnlyRepository>();
            _categoriesReadOnlyRepository.GetRandomCategories(Arg.Any<int>()).Returns(
                (args) =>
                {
                    Operation<List<Category>> getRandomCategoriesOperation = Operation<List<Category>>.Success(
                        outcome: randomCategories[randomCategoriesListIndex]);
                    randomCategoriesListIndex++;
                    return getRandomCategoriesOperation;
                });

            char[] letters = { 'l', 'd', 'm' };
            int lettersIndex = 0;

            _letterRepository = Substitute.For<ILetterReadOnlyRepository>();
            _letterRepository.GetRandomLetter().Returns(
                (args) =>
                {
                    Operation<char> getRandomLetterOperation = Operation<char>.Success(outcome: letters[lettersIndex]);
                    lettersIndex++;
                    return getRandomLetterOperation;
                });

            _roundWithCategoriesDtoMapper = Substitute.For<IdtoMapper<Round, RoundWithCategoriesDto>>();
            _roundWithCategoriesDtoMapper.ToDTO(Arg.Any<Round>()).Returns(
                (args) =>
                {
                    Round round = (Round)args[0];
                    List<CategoryDto> categoryDtos = new List<CategoryDto>();
                    foreach (Category category in round.Categories)
                    {
                        categoryDtos.Add(new CategoryDto(id: category.Id, name: category.Name));
                    }
                    return new RoundWithCategoriesDto(
                        roundDto: new RoundDto(
                            id: round.Id,
                            roundNumber: round.RoundNumber,
                            initialLetter: round.InitialLetter,
                            isActive: round.IsActive,
                            matchId: match.Id),
                        categoryDtos: categoryDtos);
                });

            _roundWithCategoriesDtoMapper.FromDTO(Arg.Any<RoundWithCategoriesDto>()).Returns(
                (args) =>
                {
                    RoundWithCategoriesDto roundWithCategoriesDto = (RoundWithCategoriesDto)args[0];
                    Round round = new Round(
                            id: roundWithCategoriesDto.RoundDto.Id,
                            roundNumber: roundWithCategoriesDto.RoundDto.RoundNumber,
                            initialLetter: roundWithCategoriesDto.RoundDto.InitialLetter,
                            isActive: roundWithCategoriesDto.RoundDto.IsActive,
                            match: match,
                            categories: new List<Category>());
                    return round;
                });

            _useCase = new CreateRoundSubUseCase(
                roundsRepository: _roundRepository,
                matchesReadOnlyRepository: _matchesRepository,
                categoryReadOnlyRepository: _categoriesReadOnlyRepository,
                letterReadOnlyRepository: _letterRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
            #endregion

            #region -- Act --
            for (int i = 0; i < MaxRounds; i++)
            {
                actualResults.Add(_useCase.Execute(matchDto: matchDto));
            }
            #endregion

            #region -- Assert --
            List<RoundWithCategoriesDto> expectedDtos = new List<RoundWithCategoriesDto>();
            IdtoMapper<Category, CategoryDto> categoryDtoMapper = Substitute.For<IdtoMapper<Category, CategoryDto>>();
            categoryDtoMapper.ToDTOs(Arg.Any<List<Category>>()).Returns(
                (args) =>
                {
                    List<Category> categories = (List<Category>)args[0];
                    List<CategoryDto> categoryDtos = new List<CategoryDto>();

                    foreach (Category category in categories)
                    {
                        categoryDtos.Add(new CategoryDto(id: category.Id, name: category.Name));
                    }
                    return categoryDtos;
                });

            for (int i = 0; i < MaxRounds; i++)
            {
                RoundWithCategoriesDto expectedDto =
                    new RoundWithCategoriesDto(
                        roundDto: new RoundDto(
                            id: i,
                            roundNumber: i,
                            initialLetter: letters[i],
                            isActive: true,
                            matchId: match.Id),
                        categoryDtos: categoryDtoMapper.ToDTOs(randomCategories[i]));

                expectedDtos.Add(expectedDto);
            }

            for (int i = 0; i < MaxRounds; i++)
            {
                if (i < MaxRounds - 1)
                {
                    Assert.AreNotEqual(actualResults[i].Outcome, actualResults[i+1].Outcome);
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
            _roundRepository = Substitute.For<IRoundsRepository>();
            _matchesRepository = Substitute.For<IMatchesRepository>();
            
            _matchesRepository.Get(Arg.Any<int>()).Returns(
                (args) =>
                {
                    int id = (int)args[0];
                    if (id < 0)
                    {
                        return Operation<Match>.Failure(errorMessage: $"Match not found with id: {id}");
                    }
                    return Operation<Match>.Success(outcome: new Match(id, startDateTime: startDateTime));
                });

            _categoriesReadOnlyRepository = Substitute.For<ICategoriesReadOnlyRepository>();
            _letterRepository = Substitute.For<ILetterReadOnlyRepository>();
            _roundWithCategoriesDtoMapper = Substitute.For<IdtoMapper<Round, RoundWithCategoriesDto>>();

            _useCase = new CreateRoundSubUseCase(
                roundsRepository: _roundRepository,
                matchesReadOnlyRepository: _matchesRepository,
                categoryReadOnlyRepository: _categoriesReadOnlyRepository,
                letterReadOnlyRepository: _letterRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
            #endregion

            #region -- Act --
            Operation<RoundWithCategoriesDto> useCaseOperation = _useCase.Execute(matchDto: matchDto);
            #endregion

            #region -- Assert --
            Assert.IsFalse(useCaseOperation.WasOk);
            Assert.AreEqual(expected: $"Match not found with id: {matchDto.Id}", actual: useCaseOperation.ErrorMessage);
            #endregion
        }

        [Test]
        public void Test_fail_due_to_match_already_ended()
        {
            #region -- Arrange --
            DateTime startDateTime = DateTime.UtcNow - TimeSpan.FromSeconds(10);
            DateTime endDateTime = DateTime.UtcNow;

            MatchDto matchDto = new MatchDto(
                id: 0,
                startDateTime: startDateTime,
                endDateTime: endDateTime);

            List<Operation<RoundWithCategoriesDto>> useCaseOperations = new List<Operation<RoundWithCategoriesDto>>();
            _roundRepository = Substitute.For<IRoundsRepository>();

            _roundRepository.GetMany(Arg.Any<int>()).Returns(
                (args) =>
                {
                    List<Round> rounds = useCaseOperations
                    .Select(useCaseResult => _roundWithCategoriesDtoMapper.FromDTO(useCaseResult.Outcome))
                    .ToList();

                    return Operation<List<Round>>.Success(
                        outcome: rounds);
                });

            _matchesRepository = Substitute.For<IMatchesRepository>();

            _matchesRepository.Get(Arg.Any<int>()).Returns(
                (args) =>
                {
                    int id = (int)args[0];
                    return Operation<Match>.Success(
                        outcome: new Match(
                            id: id,
                            startDateTime: startDateTime,
                            endDateTime: endDateTime));
                });

            _categoriesReadOnlyRepository = Substitute.For<ICategoriesReadOnlyRepository>();
            _letterRepository = Substitute.For<ILetterReadOnlyRepository>();
            _roundWithCategoriesDtoMapper = Substitute.For<IdtoMapper<Round, RoundWithCategoriesDto>>();

            _useCase = new CreateRoundSubUseCase(
                roundsRepository: _roundRepository,
                matchesReadOnlyRepository: _matchesRepository,
                categoryReadOnlyRepository: _categoriesReadOnlyRepository,
                letterReadOnlyRepository: _letterRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
            #endregion

            #region -- Act --
            Operation<RoundWithCategoriesDto> useCaseOperation = _useCase.Execute(matchDto: matchDto);
            useCaseOperations.Add(useCaseOperation);
            #endregion

            #region -- Assert --
            Assert.IsFalse(useCaseOperation.WasOk);
            Assert.AreEqual(
                expected: $"Cannot create new round for inactive match with id: {matchDto.Id}",
                actual: useCaseOperation.ErrorMessage);
            #endregion
        }

        [Test]
        public void Test_fail_due_to_match_started_after_it_ended()
        {
            #region -- Arrange --
            DateTime startDateTime = DateTime.UtcNow;
            DateTime endDateTime = DateTime.UtcNow - TimeSpan.FromSeconds(10);

            MatchDto matchDto = new MatchDto(
                id: 0,
                startDateTime: startDateTime,
                endDateTime: endDateTime);

            List<Operation<RoundWithCategoriesDto>> useCaseOperations = new List<Operation<RoundWithCategoriesDto>>();
            _roundRepository = Substitute.For<IRoundsRepository>();

            _roundRepository.GetMany(Arg.Any<int>()).Returns(
                (args) =>
                {
                    List<Round> rounds = useCaseOperations
                    .Select(useCaseResult => _roundWithCategoriesDtoMapper.FromDTO(useCaseResult.Outcome))
                    .ToList();

                    return Operation<List<Round>>.Success(
                        outcome: rounds);
                });

            _matchesRepository = Substitute.For<IMatchesRepository>();

            _matchesRepository.Get(Arg.Any<int>()).Returns(
                (args) =>
                {
                    int id = (int)args[0];
                    return Operation<Match>.Success(
                        outcome: new Match(
                            id: id,
                            startDateTime: startDateTime,
                            endDateTime: endDateTime));
                });

            _categoriesReadOnlyRepository = Substitute.For<ICategoriesReadOnlyRepository>();
            _letterRepository = Substitute.For<ILetterReadOnlyRepository>();
            _roundWithCategoriesDtoMapper = Substitute.For<IdtoMapper<Round, RoundWithCategoriesDto>>();

            _useCase = new CreateRoundSubUseCase(
                roundsRepository: _roundRepository,
                matchesReadOnlyRepository: _matchesRepository,
                categoryReadOnlyRepository: _categoriesReadOnlyRepository,
                letterReadOnlyRepository: _letterRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
            #endregion

            #region -- Act --
            Operation<RoundWithCategoriesDto> useCaseOperationResult = _useCase.Execute(matchDto: matchDto);
            useCaseOperations.Add(useCaseOperationResult);
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
            MatchDto matchDto = new MatchDto(id: 0, startDateTime: startDateTime);
            Match match = new Match(matchDto.Id, matchDto.StartDateTime);
            List<Operation<RoundWithCategoriesDto>> actualResults = new List<Operation<RoundWithCategoriesDto>>();

            _roundRepository = Substitute.For<IRoundsRepository>();
            int nextFakeId = 0;

            _roundRepository.Save(Arg.Any<Round>()).Returns(
                (args) =>
                {
                    Round round = (Round)args[0];
                    Operation<Round> saveRoundOperationResult = Operation<Round>
                    .Success(
                        outcome: new Round(
                            id: nextFakeId,
                            roundNumber: round.RoundNumber,
                            initialLetter: round.InitialLetter,
                            isActive: round.IsActive,
                            match: match,
                            categories: round.Categories));

                    nextFakeId++;
                    return saveRoundOperationResult;
                });

            _roundRepository.GetMany(Arg.Any<int>()).Returns(
                (args) =>
                {
                    List<RoundWithCategoriesDto> roundWithCategoriesDtos = new List<RoundWithCategoriesDto>();

                    foreach(
                        RoundWithCategoriesDto roundWithCategoriesDto
                        in actualResults.Select(actualResult => actualResult.Outcome))
                    {
                        RoundDto roundDto = new RoundDto(
                            id: roundWithCategoriesDto.RoundDto.Id,
                            roundNumber: roundWithCategoriesDto.RoundDto.RoundNumber,
                            initialLetter: roundWithCategoriesDto.RoundDto.InitialLetter,
                            isActive: false,
                            matchId: match.Id);

                        RoundWithCategoriesDto newRoundWithCategoriesDto = new RoundWithCategoriesDto(
                            roundDto: roundDto,
                            categoryDtos: roundWithCategoriesDto.CategoryDtos);

                        roundWithCategoriesDtos.Add(newRoundWithCategoriesDto);
                    }

                    List<Round> rounds = roundWithCategoriesDtos
                    .Select(roundWithCategoriesDto => _roundWithCategoriesDtoMapper.FromDTO(roundWithCategoriesDto))
                    .ToList();

                    return Operation<List<Round>>.Success(
                        outcome: rounds);
                });

            _matchesRepository = Substitute.For<IMatchesRepository>();
            _matchesRepository.Get(Arg.Any<int>()).Returns(
                (args) =>
                {
                    int id = (int)args[0];
                    if (id < 0)
                    {
                        return Operation<Match>.Failure(errorMessage: $"Match not found with id: {id}");
                    }
                    List<Round> rounds = new List<Round>();
                    foreach(Operation<RoundWithCategoriesDto> result in actualResults)
                    {
                        rounds.Add(_roundWithCategoriesDtoMapper.FromDTO(result.Outcome));
                    }
                    Match lambdaMatch = new Match(
                        id,
                        startDateTime,
                        endDateTime: null,
                        rounds: rounds);
                    return Operation<Match>.Success(outcome: lambdaMatch);
                });

            List<Category> categories = new List<Category>()
            {
                new Category(id: 1, name: "Category_1"),
                new Category(id: 2, name: "Category_2"),
                new Category(id: 3, name: "Category_3"),
                new Category(id: 4, name: "Category_4"),
                new Category(id: 5, name: "Category_5"),
                new Category(id: 6, name: "Category_6"),
                new Category(id: 7, name: "Category_7"),
                new Category(id: 8, name: "Category_8"),
                new Category(id: 9, name: "Category_9"),
                new Category(id: 10, name: "Category_10"),
            };
            List<Category>[] randomCategories = new List<Category>[MaxRounds];
            int randomCategoriesListIndex = 0;

            Random random = new Random();
            for (int i = 0; i < randomCategories.Length; i++)
            {
                randomCategories[i] = categories
                    .OrderBy(category => random.Next())
                    .Take(MaxCategories)
                    .ToList();
            }

            _categoriesReadOnlyRepository = Substitute.For<ICategoriesReadOnlyRepository>();
            _categoriesReadOnlyRepository.GetRandomCategories(Arg.Any<int>()).Returns(
                (args) =>
                {
                    Operation<List<Category>> getRandomCategoriesOperationResult = Operation<List<Category>>.Success(
                        outcome: randomCategories[randomCategoriesListIndex]);
                    randomCategoriesListIndex++;
                    return getRandomCategoriesOperationResult;
                });

            char[] letters = { 'l', 'd', 'm' };
            int lettersIndex = 0;

            _letterRepository = Substitute.For<ILetterReadOnlyRepository>();
            _letterRepository.GetRandomLetter().Returns(
                (args) =>
                {
                    Operation<char> getRandomLetterOperationResult = Operation<char>.Success(outcome: letters[lettersIndex]);
                    lettersIndex++;
                    return getRandomLetterOperationResult;
                });

            _roundWithCategoriesDtoMapper = Substitute.For<IdtoMapper<Round, RoundWithCategoriesDto>>();
            _roundWithCategoriesDtoMapper.ToDTO(Arg.Any<Round>()).Returns(
                (args) =>
                {
                    Round round = (Round)args[0];
                    List<CategoryDto> categoryDtos = new List<CategoryDto>();
                    foreach (Category category in round.Categories)
                    {
                        categoryDtos.Add(new CategoryDto(id: category.Id, name: category.Name));
                    }
                    return new RoundWithCategoriesDto(
                        roundDto: new RoundDto(
                            id: round.Id,
                            roundNumber: round.RoundNumber,
                            initialLetter: round.InitialLetter,
                            isActive: round.IsActive,
                            matchId: match.Id),
                        categoryDtos: categoryDtos);
                });

            _roundWithCategoriesDtoMapper.FromDTO(Arg.Any<RoundWithCategoriesDto>()).Returns(
                (args) =>
                {
                    RoundWithCategoriesDto roundWithCategoriesDto = (RoundWithCategoriesDto)args[0];
                    Round round = new Round(
                            id: roundWithCategoriesDto.RoundDto.Id,
                            roundNumber: roundWithCategoriesDto.RoundDto.RoundNumber,
                            initialLetter: roundWithCategoriesDto.RoundDto.InitialLetter,
                            isActive: roundWithCategoriesDto.RoundDto.IsActive,
                            match: match,
                            categories: new List<Category>());
                    return round;
                });

            _useCase = new CreateRoundSubUseCase(
                roundsRepository: _roundRepository,
                matchesReadOnlyRepository: _matchesRepository,
                categoryReadOnlyRepository: _categoriesReadOnlyRepository,
                letterReadOnlyRepository: _letterRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
            #endregion

            #region -- Act --
            for (int i = 0; i < MaxRounds + 1; i++)
            {
                actualResults.Add(_useCase.Execute(matchDto: matchDto));
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

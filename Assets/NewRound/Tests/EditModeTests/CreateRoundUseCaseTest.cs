using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.NewRound.Models;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Shared.Mappers;
using TopicTwister.NewRound.UseCases;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace NewRoundTests
{
    public class CreateRoundUseCaseTest
    {
        private ICreateRoundUseCase _useCase;
        private IRoundsRepository _roundRepository;
        private IMatchesRepository _matchesRepository;
        private IdtoMapper<Round, RoundWithCategoriesDto> _roundWithCategoriesDtoMapper;

        [Test]
        public void Test_ok_creation_of_round()
        {
            #region -- Arrange --
            DateTime startDateTime = DateTime.UtcNow;
            MatchDTO matchDto = new MatchDTO(id: 0, startDateTime: startDateTime);

            _roundRepository = Substitute.For<IRoundsRepository>();
            _roundRepository.Save(Arg.Any<Round>()).Returns(
                (args) =>
                {
                    Round round = (Round)args[0];
                    return Result<Round>.Success(
                        outcome: new Round(
                            id: 0,
                            roundNumber: round.RoundNumber,
                            initialLetter: round.InitialLetter,
                            isActive: round.IsActive,
                            categories: round.Categories));
                });

            _matchesRepository = Substitute.For<IMatchesRepository>();
            _matchesRepository.Get(Arg.Any<int>()).Returns(
                (args) =>
                {
                    int id = (int)args[0];
                    if (id < 0)
                    {
                        return Result<Match>.Failure(errorMessage: $"Match not found with id: {id}");
                    }
                    return Result<Match>.Success(outcome: new Match(id, startDateTime: startDateTime));
                });

            _roundWithCategoriesDtoMapper = Substitute.For<IdtoMapper<Round, RoundWithCategoriesDto>>();
            _roundWithCategoriesDtoMapper.ToDTO(Arg.Any<Round>()).Returns(
                (args) =>
                {
                    Round round = (Round)args[0];
                    List<CategoryDto> categoryDtos = new List<CategoryDto>();
                    foreach(Category category in round.Categories)
                    {
                        categoryDtos.Add(new CategoryDto(id: category.Id, name: category.Name));
                    }
                    return new RoundWithCategoriesDto(
                        roundDto: new RoundDto(
                            roundNumber: round.RoundNumber,
                            initialLetter: round.InitialLetter,
                            isActive: round.IsActive),
                        categoryDtos: categoryDtos);
                });

            _useCase = new CreateRoundUseCase(
                roundsRepository: _roundRepository,
                matchesRepository: _matchesRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
            #endregion

            #region -- Act --
            Result<RoundWithCategoriesDto> useCaseOperationResult = _useCase.Create(matchDto: matchDto);
            #endregion

            #region -- Assert --
            RoundWithCategoriesDto expectedDto = new RoundWithCategoriesDto(
                roundDto: new RoundDto(
                    id: 0,
                    roundNumber: 0,
                    initialLetter: useCaseOperationResult.Outcome.RoundDto.InitialLetter,
                    isActive: true),
                categoryDtos: useCaseOperationResult.Outcome.CategoryDtos);
            Assert.IsTrue(useCaseOperationResult.WasOk);
            Assert.AreEqual(expected: expectedDto, actual: useCaseOperationResult.Outcome);
            Assert.AreEqual(expected: 5, actual: useCaseOperationResult.Outcome.CategoryDtos.Count);
            Assert.IsFalse(useCaseOperationResult.Outcome.CategoryDtos.Any(category => category == null));

            List<CategoryDto> duplicates = useCaseOperationResult.Outcome.CategoryDtos
                .GroupBy(category => category.Id)
                .Where(category => category.Count() > 1)
                .SelectMany(category => category)
                .ToList();

            Assert.IsEmpty(duplicates);
            #endregion
        }

        [Test]
        public void Test_fail_due_to_unknown_match()
        {
            #region -- Arrange --
            DateTime startDateTime = DateTime.UtcNow;
            MatchDTO matchDto = new MatchDTO(id: -1, startDateTime: startDateTime);
            _roundRepository = Substitute.For<IRoundsRepository>();
            _matchesRepository = Substitute.For<IMatchesRepository>();
            
            _matchesRepository.Get(Arg.Any<int>()).Returns(
                (args) =>
                {
                    int id = (int)args[0];
                    if (id < 0)
                    {
                        return Result<Match>.Failure(errorMessage: $"Match not found with id: {id}");
                    }
                    return Result<Match>.Success(outcome: new Match(id, startDateTime: startDateTime));
                });

            _roundWithCategoriesDtoMapper = Substitute.For<IdtoMapper<Round, RoundWithCategoriesDto>>();

            _useCase = new CreateRoundUseCase(
                roundsRepository: _roundRepository,
                matchesRepository: _matchesRepository,
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
            _roundRepository = Substitute.For<IRoundsRepository>();
            _matchesRepository = Substitute.For<IMatchesRepository>();

            _matchesRepository.Get(Arg.Any<int>()).Returns(
                (args) =>
                {
                    int id = (int)args[0];
                    return Result<Match>.Success(
                        outcome: new Match(
                            id: id,
                            startDateTime: startDateTime,
                            endDateTime: endDateTime));
                });

            _roundWithCategoriesDtoMapper = Substitute.For<IdtoMapper<Round, RoundWithCategoriesDto>>();

            _useCase = new CreateRoundUseCase(
                roundsRepository: _roundRepository,
                matchesRepository: _matchesRepository,
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
            _roundRepository = Substitute.For<IRoundsRepository>();
            _matchesRepository = Substitute.For<IMatchesRepository>();

            _matchesRepository.Get(Arg.Any<int>()).Returns(
                (args) =>
                {
                    int id = (int)args[0];
                    return Result<Match>.Success(
                        outcome: new Match(
                            id: id,
                            startDateTime: startDateTime,
                            endDateTime: endDateTime));
                });

            _roundWithCategoriesDtoMapper = Substitute.For<IdtoMapper<Round, RoundWithCategoriesDto>>();

            _useCase = new CreateRoundUseCase(
                roundsRepository: _roundRepository,
                matchesRepository: _matchesRepository,
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
    }
}

using NSubstitute;
using NUnit.Framework;
using System;
using TopicTwister.NewRound.Models;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Shared.Mappers;
using TopicTwister.NewRound.UseCases;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories.Exceptions;


namespace NewRoundTests
{
    public class CreateRoundUseCaseTest
    {
        private ICreateRoundUseCase _useCase;
        private IRoundsRepository _roundRepository;
        private IMatchesRepository _matchesRepository;
        private IdtoMapper<Match, MatchDTO> _matchDtoMapper;
        private IdtoMapper<Round, RoundWithCategoriesDtoMapper> _roundWithCategoriesDtoMapper;

        // TODO: public void Test_ok_creation_of_match_and_usermatches()

        [Test]
        public void Test_fail_due_to_unknown_match()
        {
            #region -- Arrange --
            MatchDTO matchDto = new MatchDTO(id: -1, startDateTime: DateTime.UtcNow);
            _roundRepository = Substitute.For<IRoundsRepository>();
            _matchesRepository = Substitute.For<IMatchesRepository>();
            
            _matchesRepository.Get(Arg.Any<int>()).Returns(
                (args) =>
                {
                    int id = (int)args[0];
                    if (id < 0)
                    {
                        throw new MatchNotFoundByRespositoryException();
                    }
                    return new Match(id, startDateTime: DateTime.UtcNow);
                });

            _matchDtoMapper = Substitute.For<IdtoMapper<Match, MatchDTO>>();
            _roundWithCategoriesDtoMapper = Substitute.For<IdtoMapper<Round, RoundWithCategoriesDtoMapper>>();

            _useCase = new CreateRoundUseCase(
                roundsRepository: _roundRepository,
                matchesRepository: _matchesRepository,
                matchDtoMapper: _matchDtoMapper,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
            #endregion

            #region -- Act & Assert --
            RoundNotCreatedInUseCaseException exception = Assert.Throws<RoundNotCreatedInUseCaseException>(
                () => _useCase.Create(matchDto: matchDto));
            Assert.IsNotNull(exception);
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
                    return new Match(
                        id: id,
                        startDateTime: startDateTime,
                        endDateTime: endDateTime); ;
                });

            _matchDtoMapper = Substitute.For<IdtoMapper<Match, MatchDTO>>();
            _roundWithCategoriesDtoMapper = Substitute.For<IdtoMapper<Round, RoundWithCategoriesDtoMapper>>();

            _useCase = new CreateRoundUseCase(
                roundsRepository: _roundRepository,
                matchesRepository: _matchesRepository,
                matchDtoMapper: _matchDtoMapper,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);
            #endregion

            #region -- Act & Assert --
            NewRoundForInactiveMatchUseCaseException exception = Assert.Throws<NewRoundForInactiveMatchUseCaseException>(
                () => _useCase.Create(matchDto: matchDto));
            Assert.AreEqual(expected: $"matchDto.Id: {matchDto.Id}", actual: exception.Message);
            #endregion
        }
    }
}

using NSubstitute;
using NUnit.Framework;
using System;
using TopicTwister.NewRound.Models;
using TopicTwister.NewRound.Shared.Interfaces;
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
        private IdtoMapper<Match, MatchDTO> _mapper;

        /*[Test]
        public void Test_ok_creation_of_match_and_usermatches()
        {
            // Use the Assert class to test conditions
        }*/

        [Test]
        public void Test_fail_due_to_unknown_match()
        {
            #region -- Arrange --
            MatchDTO matchDto = new MatchDTO(id: -1, startDateTime: DateTime.UtcNow);
            _roundRepository = Substitute.For<IRoundsRepository>();
            _roundRepository.Save(Arg.Any<Round>()).Returns(
                (args) =>
                {
                    Round round = (Round)args[0];
                    if (round.Id < 0)
                    {
                        throw new RoundNotSavedByRepositoryException();
                    }
                    return new Round(id: round.Id);
                });
            _mapper = Substitute.For<IdtoMapper<Match, MatchDTO>>();
            _mapper.FromDTO(Arg.Any<MatchDTO>()).Returns(
                (args) =>
                {
                    MatchDTO lambdaMatchDto = (MatchDTO)args[0];
                    Match match = new Match(
                        id: lambdaMatchDto.Id,
                        startDateTime: lambdaMatchDto.StartDateTime,
                        endDateTime: lambdaMatchDto.EndDateTime);
                    return match;
                });

            _useCase = new CreateRoundUseCase(roundsRepository: _roundRepository, mapper: _mapper);
            #endregion

            #region -- Act & Assert --
            RoundNotCreatedInUseCaseException exception = Assert.Throws<RoundNotCreatedInUseCaseException>(() => _useCase.Create(matchDto: matchDto));
            Assert.IsNotNull(exception);
            Assert.IsNotNull(exception.InnerException);
            Assert.IsInstanceOf<RoundNotSavedByRepositoryException>(exception.InnerException);
            Assert.AreEqual(expected: $"matchId: {matchDto}", actual: exception.InnerException.Message);
            #endregion
        }
    }
}

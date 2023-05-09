using System;
using System.Collections.Generic;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Utils;


public class EndTurnUseCase : IEndTurnUseCase
{
    private IUsersReadOnlyRepository _usersReadOnlyRepository;
    private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    private IUserMatchesRepository _userMatchesRepository;
    private ITurnsRepository _turnsRepository;
    private IRoundsReadOnlyRepository _roundsReadOnlyRepository;

    public EndTurnUseCase(
        IUsersReadOnlyRepository usersReadOnlyRepository,
        IMatchesReadOnlyRepository matchesReadOnlyRepository,
        IUserMatchesRepository userMatchesRepository,
        ITurnsRepository turnsRepository,
        IRoundsReadOnlyRepository roundsReadOnlyRepository)
    {
        _usersReadOnlyRepository = usersReadOnlyRepository;
        _matchesReadOnlyRepository = matchesReadOnlyRepository;
        _userMatchesRepository = userMatchesRepository;
        _turnsRepository = turnsRepository;
        _roundsReadOnlyRepository = roundsReadOnlyRepository;
    }

    public Operation<TurnWithEvaluatedAnswersDto> Execute(int userId, int matchId, AnswerDto[] answerDtos)
    {
        Operation<User> getUserOperation = _usersReadOnlyRepository.Get(id: userId);

        if (getUserOperation.WasOk == false)
        {
            return Operation<TurnWithEvaluatedAnswersDto>.Failure(errorMessage: getUserOperation.ErrorMessage);
        }

        User user = getUserOperation.Result;

        Operation<Match> getMatchOperation = _matchesReadOnlyRepository.Get(id: matchId);

        if (getMatchOperation.WasOk == false)
        {
            return Operation<TurnWithEvaluatedAnswersDto>.Failure(errorMessage: getMatchOperation.ErrorMessage);
        }

        Match match = getMatchOperation.Result;

        Operation<UserMatch> getUserMatchOperation = _userMatchesRepository.Get(userId: userId, matchId: match.Id);

        if (getUserMatchOperation.WasOk == false)
        {
            return Operation<TurnWithEvaluatedAnswersDto>.Failure(
                errorMessage: $"User with id {userId} is not involved in match with id {matchId}");
        }

        Operation<List<Round>> getRoundsOperation = _roundsReadOnlyRepository.GetMany(matchId: match.Id);

        if (getRoundsOperation.WasOk == false)
        {
            return Operation<TurnWithEvaluatedAnswersDto>.Failure(errorMessage: getRoundsOperation.ErrorMessage);
        }

        match = new Match(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime,
            rounds: getRoundsOperation.Result);

        Round activeRound = match.ActiveRound;

        Operation<Turn> getTurnOperation = _turnsRepository.Get(userId: userId, roundId: activeRound.Id);

        if (getTurnOperation.WasOk == false)
        {
            return Operation<TurnWithEvaluatedAnswersDto>.Failure(
                errorMessage: $"Turn not found for user with id {user.Id} " +
                $"in round with id {activeRound.Id} in match with id {match.Id}");
        }

        throw new NotImplementedException();
    }
}
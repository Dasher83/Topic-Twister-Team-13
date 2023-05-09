using System;
using System.Collections.Generic;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Utils;


public class StartTurnUseCase : IStartTurnUseCase
{
    private IUsersReadOnlyRepository _usersReadOnlyRepository;
    private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    private IUserMatchesRepository _userMatchesRepository;
    private ITurnsRepository _turnsRepository;
    private IRoundsReadOnlyRepository _roundsReadOnlyRepository;
    private IdtoMapper<Turn,TurnDto> _turnDtoMapper;

    public StartTurnUseCase(
        IUsersReadOnlyRepository usersReadOnlyRepository,
        IMatchesReadOnlyRepository matchesReadOnlyRepository,
        IUserMatchesRepository userMatchesRepository,
        ITurnsRepository turnsRepository,
        IRoundsReadOnlyRepository roundsReadOnlyRepository,
        IdtoMapper<Turn, TurnDto> turnDtoMapper)
    {
        _usersReadOnlyRepository = usersReadOnlyRepository;
        _matchesReadOnlyRepository = matchesReadOnlyRepository;
        _userMatchesRepository = userMatchesRepository;
        _turnsRepository = turnsRepository;
        _roundsReadOnlyRepository = roundsReadOnlyRepository;
        _turnDtoMapper = turnDtoMapper;
    }

    public Operation<TurnDto> Execute(int userId, int matchId)
    {
        Operation<User> getUserOperation = _usersReadOnlyRepository.Get(id: userId);

        if(getUserOperation.WasOk == false)
        {
            return Operation<TurnDto>.Failure(errorMessage: getUserOperation.ErrorMessage);
        }

        Operation<Match> getMatchOperation = _matchesReadOnlyRepository.Get(id: matchId);

        if (getMatchOperation.WasOk == false)
        {
            return Operation<TurnDto>.Failure(errorMessage: getMatchOperation.ErrorMessage);
        }

        Match match = getMatchOperation.Result;
        List<Round> rounds = _roundsReadOnlyRepository.GetMany(match.Id).Result;

        match = new Match(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime,
            rounds: rounds);

        int activeRoundId = match.ActiveRound.Id;

        Operation<UserMatch> getUserMatchOperation = _userMatchesRepository.Get(userId: userId, matchId: matchId);

        if(getUserMatchOperation.WasOk == false)
        {
            return Operation<TurnDto>.Failure(errorMessage: $"User with id {userId} is not involved in match with id {matchId}");
        }

        Operation<Turn> getTurnOperation = _turnsRepository.Get(userId: userId, roundId: activeRoundId);
        
        if(getTurnOperation.WasOk == true)
        {
            return Operation<TurnDto>.Failure(
                errorMessage: $"Turn already exists for user with id {userId} " +
                $"in round with id {activeRoundId} in match with id {matchId}");
        }

        Operation<UserMatch[]> getUserMatchesOperation = _userMatchesRepository.GetMany(matchId: matchId);

        if (getUserMatchesOperation.WasOk == false)
        {
            return Operation<TurnDto>.Failure(errorMessage: getUserMatchesOperation.ErrorMessage);
        }

        UserMatch[] userMatches = getUserMatchesOperation.Result;
        UserMatch opponentUserMatch;

        if (userMatches[0].User.Id != userId)
        {
            opponentUserMatch = userMatches[0];
        }
        else
        {
            opponentUserMatch = userMatches[1];
        }

        if (opponentUserMatch.HasInitiative)
        {
            Operation<Turn> getOpponentTurnOperation = _turnsRepository
                .Get(userId: opponentUserMatch.User.Id, roundId: activeRoundId);

            if(getOpponentTurnOperation.WasOk == false)
            {
                string message = $"Turn can not be created for user with id {userId} " +
                $"in round with id {activeRoundId} in match with id {matchId} " +
                $"since user with id {opponentUserMatch.User.Id} has not finished his turn yet";

                return Operation<TurnDto>.Failure(errorMessage: message);
            }
        }

        User user = getUserOperation.Result;

        Turn newturn = new Turn(
            user: user,
            round: match.ActiveRound,
            startDateTime: DateTime.UtcNow);

        Operation<Turn> insertTurnOperation = _turnsRepository.Insert(newturn);

        if(insertTurnOperation.WasOk == false)
        {
            return Operation<TurnDto>.Failure(errorMessage: insertTurnOperation.ErrorMessage);
        }

        TurnDto turnDto = _turnDtoMapper.ToDTO(insertTurnOperation.Result);
        return Operation<TurnDto>.Success(result: turnDto);
    }
}
